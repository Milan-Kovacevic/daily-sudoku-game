using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using Sudoku.Exceptions;
using Sudoku.Messages;
using Sudoku.Models;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Sudoku.Stores
{
    public class SudokuBoardStore
    {
        private const string SUDOKU_PLAYER_BOARDS_FOLDER = "./Data/Boards";
        private readonly string _repositoryFolder;

        private SudokuBoard? _currentBoard;
        public SudokuBoard? CurrentBoard
        {
            get => _currentBoard;
            set
            {
                if (value == null)
                    return;
                _currentBoard = value;
                if (_currentBoard.Grids.Count > 0)
                    CurrentGrid = _currentBoard.Grids.ElementAt(0);
            }
        }
        public bool HasNewGrid { get => _currentBoard?.Grids.Count >= 2; }
        public bool IsBoardLoaded { get => CurrentBoard != null && CurrentBoard.Grids.Count > 0; }
        public int NumberOfGrids { get => CurrentBoard?.Grids.Count ?? 0; }

        private SudokuGrid? _currentGrid;
        public SudokuGrid? CurrentGrid
        {
            get => _currentGrid;
            private set
            {
                if (value != null)
                {
                    _currentGrid = value;
                    NotifyCurrentGridChanged(value);
                }
            }
        }

        private void NotifyCurrentGridChanged(SudokuGrid value)
        {
            var message = new CurrentSudokuGridMessage(value);
            WeakReferenceMessenger.Default.Send(message);
        }

        public SudokuBoardStore(string repositoryFolder = SUDOKU_PLAYER_BOARDS_FOLDER)
        {
            _repositoryFolder = repositoryFolder;
            if (!Directory.Exists(repositoryFolder))
            {
                // Creating initial directory of sudoku boards...
                Directory.CreateDirectory(repositoryFolder);
            }
        }

        private record class PlayerSudokuBoard(DateOnly Date, SudokuBoard Board);


        // Loads board for player from datasource
        public void LoadPlayerBoard(Guid playerId)
        {
            var playerBoardLocation = Path.Combine(_repositoryFolder, playerId.ToString());
            if (!File.Exists(playerBoardLocation))
                return;

            var playerBoard = JsonConvert.DeserializeObject<PlayerSudokuBoard>(File.ReadAllText(playerBoardLocation));
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            bool isDailyBoardLoaded = playerBoard is not null && playerBoard.Date == currentDate;
            if (!isDailyBoardLoaded)
                return;
            if (playerBoard is not null && playerBoard.Board.Grids.Count == 0)
                throw new DailyNumberOfGamesExceededException();

            CurrentBoard = playerBoard?.Board;
        }

        public void RemoveCurrentBoardGrid(Guid? playerId = null)
        {
            var grids = CurrentBoard?.Grids;
            if (grids is null || CurrentGrid == null)
                return;

            grids.Remove(CurrentGrid);
            if (grids.Count == 0)
                CurrentGrid = null;
            else
                CurrentGrid = grids.ElementAt(0);

            if (playerId is not null)
                SavePlayerBoard((Guid)playerId);
        }

        public void SavePlayerBoard(Guid playerId)
        {
            if (CurrentBoard is null)
                return;

            var playerBoardLocation = Path.Combine(_repositoryFolder, playerId.ToString());
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            var playerBoard = new PlayerSudokuBoard(currentDate, CurrentBoard);
            var json = JsonConvert.SerializeObject(playerBoard, Formatting.Indented);
            File.WriteAllText(playerBoardLocation, json);
        }

    }
}
