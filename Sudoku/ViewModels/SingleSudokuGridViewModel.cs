using CommunityToolkit.Mvvm.ComponentModel;
using Sudoku.Models;
using Sudoku.Services;
using Sudoku.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.ViewModels
{
    public partial class SingleSudokuGridViewModel : ObservableObject
    {
        private readonly SudokuGuessService _sudokuGuessService;
        public List<SingleGridCellViewModel> SudokuGrid { get; set; }

        public SingleSudokuGridViewModel(SudokuGuessService sudokuGuessService, int gridNumber, List<int> initialGridGuesses)
        {
            _sudokuGuessService = sudokuGuessService;
            SudokuGrid = new List<SingleGridCellViewModel>();
            InitializeSudokuGrid(gridNumber, initialGridGuesses);
        }

        private void InitializeSudokuGrid(int gridNumber, List<int> initialGridGuesses)
        {
            for (int i = 0; i < 9; i++)
            {
                bool isReadOnly = initialGridGuesses[i] != 0;
                var singleCellVM = new SingleGridCellViewModel(_sudokuGuessService)
                {
                    GridNumber = gridNumber,
                    CellNumber = i,
                    IsTextReadOnly = isReadOnly,
                    CellText = initialGridGuesses[i] != 0 ? $"{initialGridGuesses[i]}" : string.Empty,
                };
                
                SudokuGrid.Add(singleCellVM);
            };
        }
    }
}
