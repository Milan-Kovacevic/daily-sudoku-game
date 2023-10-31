using CommunityToolkit.Mvvm.ComponentModel;
using Sudoku.Services;
using Sudoku.Services.SudokuProviders;
using Sudoku.Stores;
using Sudoku.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore navigationStore;
        private readonly SudokuBoardStore sudokuBoardStore;
        private readonly SelectedThemeStore selectedThemeStore;
        private readonly SudokuPlayerStore sudokuPlayerStore;

        private readonly SudokuGeneratorService sudokuGeneratorService;
        private readonly SudokuGuessService sudokuGuessService;
        private readonly SudokuBoardService sudokuBoardService;
        private readonly NavigationService navigateToNewGameService;
        private readonly NavigationService navigateToLoginService;
        private readonly NavigationService navigateToRegisterService;
        private readonly NavigationService navigateToOptionsService;
        private readonly NavigationService navigateToStandingsService;
        private readonly NavigationService navigateToAccountService;
        private readonly ThemeChangeService themeChangeService;
        private readonly SudokuPlayerService sudokuPlayerService;
        private readonly ISudokuProvider sudokuServiceProvider;

        public App()
        {
            navigationStore = new NavigationStore();
            sudokuBoardStore = new SudokuBoardStore();
            selectedThemeStore = new SelectedThemeStore();
            sudokuPlayerStore = new SudokuPlayerStore();
            sudokuServiceProvider = new DosukuRestProvider();

            sudokuGeneratorService = new SudokuGeneratorService(sudokuServiceProvider, sudokuBoardStore, sudokuPlayerStore);
            sudokuGuessService = new SudokuGuessService(sudokuBoardStore);
            sudokuBoardService = new SudokuBoardService(sudokuBoardStore, sudokuPlayerStore);
            themeChangeService = new ThemeChangeService(selectedThemeStore);
            sudokuPlayerService = new SudokuPlayerService(sudokuPlayerStore);
            navigateToNewGameService = new NavigationService(navigationStore, CreateSudokuGameViewModel);
            navigateToOptionsService = new NavigationService(navigationStore, CreateOptionsViewModel);
            navigateToLoginService = new NavigationService(navigationStore, CreateLoginViewModel);
            navigateToRegisterService = new NavigationService(navigationStore, CreateRegisterViewModel);
            navigateToStandingsService = new NavigationService(navigationStore, CreateStandingsViewModel);
            navigateToAccountService = new NavigationService(navigationStore, CreateAccountViewModel);
        }

        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            // Loading required stores...
            sudokuPlayerStore.LoadSudokuPlayers();
            // Creating initial view model
            navigationStore.CurrentViewModel = CreateOptionsViewModel();
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore)
            };
            MainWindow.Show();
        }

        #region ViewModel Initialization

        // TODO: Add dependency injection...
        private OptionsViewModel CreateOptionsViewModel()
        {
            return new OptionsViewModel(sudokuGeneratorService, navigateToNewGameService, navigateToLoginService, 
                navigateToRegisterService, navigateToStandingsService, navigateToAccountService, selectedThemeStore, themeChangeService, sudokuPlayerStore, sudokuPlayerService, sudokuBoardService);
        }

        private SudokuGameViewModel CreateSudokuGameViewModel()
        {
            return new SudokuGameViewModel(sudokuBoardStore, sudokuGuessService, navigateToOptionsService, sudokuPlayerService, sudokuPlayerStore, sudokuBoardService);
        }

        private LoginViewModel CreateLoginViewModel()
        {
            return new LoginViewModel(navigateToOptionsService, sudokuPlayerService);
        }

        private RegisterViewModel CreateRegisterViewModel()
        {
            return new RegisterViewModel(navigateToOptionsService, sudokuPlayerService);
        }

        private StandingsViewModel CreateStandingsViewModel()
        {
            return new StandingsViewModel(sudokuPlayerStore, navigateToOptionsService);
        }

        private AccountViewModel CreateAccountViewModel()
        {
            return new AccountViewModel(sudokuPlayerStore, sudokuPlayerService, navigateToOptionsService);
        }
        #endregion
    }
}
