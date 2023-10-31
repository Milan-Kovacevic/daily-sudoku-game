using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json.Linq;
using Sudoku.Messages;
using Sudoku.Models;
using Sudoku.Stores;
using Sudoku.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Sudoku.Services
{
    public class SudokuGuessService
    {
        private readonly SudokuBoardStore _sudokuBoardStore;

        public SudokuGuessService(SudokuBoardStore sudokuBoardStore)
        {
            _sudokuBoardStore = sudokuBoardStore;
        }

        public void SendAddGuessMessage(int gridNumber, int cellNumber, int value, bool isValid)
        {
            SudokuGuessMessage guess = new SudokuGuessMessage(gridNumber, cellNumber, value, isValid);
            WeakReferenceMessenger.Default.Send(guess);
        }

        public void SendRemoveGuessMessage(int gridNumber, int cellNumber)
        {
            SudokuGuessMessage guess = new SudokuGuessMessage(gridNumber, cellNumber);
            WeakReferenceMessenger.Default.Send(guess);
        }

        public bool ValidateGuess(int gridNumber, int cellNumber, int value)
        {
            var grid = _sudokuBoardStore.CurrentGrid;
            var coordiantes = Util.TranslateGridToMatrixCoordinates(gridNumber, cellNumber);
            if (grid == null)
                return false;

            return value == grid.Solution[coordiantes.Item1][coordiantes.Item2];
        }

        public int RevealGridCellNumber(int gridNumber, int cellNumber)
        {
            var grid = _sudokuBoardStore.CurrentGrid;
            if (grid == null)
                return 0;
            var matrixCoordinates = Util.TranslateGridToMatrixCoordinates(gridNumber, cellNumber);
            return grid.Solution[matrixCoordinates.Item1][matrixCoordinates.Item2];
        }
    }
}
