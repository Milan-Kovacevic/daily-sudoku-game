using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sudoku.Models;
using Sudoku.Services;
using Sudoku.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Sudoku.ViewModels
{
    public partial class SudokuGameViewModel : ObservableObject, IViewModelBase
    {
        [ObservableProperty]
        private SudokuBoardViewModel sudokuBoardBinding;

        [RelayCommand]
        public void ReturnBack()
        {
            _navigateToOptionsService.Navigate();
            _sudokuBoardService.RemoveCurrentBoardGrid();
        }

        [RelayCommand]
        public void GoBack()
        {
            if(SudokuBoardBinding.IsGameStarted)
                _sudokuBoardService.RemoveCurrentBoardGrid();
            _navigateToOptionsService.Navigate();
        }

        [RelayCommand]
        public void StartNewGame()
        {
            _sudokuBoardService.RemoveCurrentBoardGrid();
        }

        public void Dispose()
        {
            SudokuBoardBinding.Dispose();
            SudokuBoardBinding = null;
        }

        private readonly NavigationService _navigateToOptionsService;
        private readonly SudokuBoardService _sudokuBoardService;

        public SudokuGameViewModel(SudokuBoardStore sudokuBoardStore, SudokuGuessService sudokuGuessService, NavigationService navigateToOptionsService, 
            SudokuPlayerService sudokuPlayerService, SudokuPlayerStore sudokuPlayerStore, SudokuBoardService sudokuBoardService)
        {
            _navigateToOptionsService = navigateToOptionsService;
            _sudokuBoardService = sudokuBoardService;
            sudokuBoardBinding = new SudokuBoardViewModel(sudokuBoardStore, sudokuGuessService, sudokuPlayerService, sudokuPlayerStore);
        }
    }
}
