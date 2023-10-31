using Sudoku.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Services
{
    public class SudokuBoardService
    {
        private readonly SudokuBoardStore _sudokuBoardStore;
        private readonly SudokuPlayerStore _sudokuPlayerStore;

        public SudokuBoardService(SudokuBoardStore sudokuBoardStore, SudokuPlayerStore sudokuPlayerStore)
        {
            _sudokuBoardStore = sudokuBoardStore;
            _sudokuPlayerStore = sudokuPlayerStore;
        }

        public void LoadPlayerBoard()
        {
            if (!_sudokuPlayerStore.IsUserLoggedIn)
                return;

            var playerId = _sudokuPlayerStore.CurrentPlayer?.Id;
            if (playerId is null)
                return;

            _sudokuBoardStore.LoadPlayerBoard((Guid)playerId);
        }

        public void RemoveCurrentBoardGrid()
        {
            var playerId = _sudokuPlayerStore.CurrentPlayer?.Id;
            _sudokuBoardStore.RemoveCurrentBoardGrid(playerId);
        }

        public void ClearSudokuBoard()
        {
            _sudokuBoardStore.CurrentBoard = null;
        }
    }
}
