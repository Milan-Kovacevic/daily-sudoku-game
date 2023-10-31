using Sudoku.Models;
using Sudoku.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Services
{
    public class SudokuGeneratorService
    {
        private readonly ISudokuProvider _sudokuProvider;
        private readonly SudokuBoardStore _sudokuBoardStore;
        private readonly SudokuPlayerStore _sudokuPlayerStore;

        public SudokuGeneratorService(ISudokuProvider sudokuProvider, SudokuBoardStore sudokuBoardStore, SudokuPlayerStore sudokuPlayerStore)
        {
            _sudokuProvider = sudokuProvider;
            _sudokuBoardStore = sudokuBoardStore;
            _sudokuPlayerStore = sudokuPlayerStore;
        }

        public Task GenerateSudokuBoard(int numOfGrids)
        {
            var playerId = _sudokuPlayerStore.CurrentPlayer?.Id;
            if (playerId == null)
                return GenerateSudokuBoardForGuest(numOfGrids);
            else
                return GenerateSudokuBoardForPlayer(numOfGrids, (Guid)playerId);
        }

        private async Task GenerateSudokuBoardForGuest(int numOfGrids)
        {
            if (_sudokuBoardStore.IsBoardLoaded)
                return;
            var newBoard = await _sudokuProvider.GetSudokuBoard(numOfGrids);
            _sudokuBoardStore.CurrentBoard = newBoard;
        }

        private async Task GenerateSudokuBoardForPlayer(int numOfGrids, Guid playerId)
        {
            if (_sudokuBoardStore.IsBoardLoaded)
                return;
            var newBoard = await _sudokuProvider.GetSudokuBoard(numOfGrids);
            _sudokuBoardStore.CurrentBoard = newBoard;
            _sudokuBoardStore.SavePlayerBoard(playerId);
        }
    }
}
