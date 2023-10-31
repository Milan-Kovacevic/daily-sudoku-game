using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Sudoku.Exceptions;
using Sudoku.Services;
using Sudoku.Stores;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sudoku.ViewModels
{
    public partial class OptionsViewModel : ObservableObject, IViewModelBase
    {
        [ObservableProperty]
        private string currentTheme;

        [ObservableProperty]
        private bool isDark;
        partial void OnIsDarkChanged(bool value)
        {
            if (value == true)
                _themeChangeService.SetDarkTheme();
            else
                _themeChangeService.SetLightTheme();
            CurrentTheme = _selectedThemeStore.ThemeString;
        }

        [ObservableProperty]
        private bool isLoadingGame;

        [ObservableProperty]
        private string newGameText;

        [ObservableProperty]
        private bool isDialogOpen;

        [ObservableProperty]
        private string dialogMessage;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private bool isUserLoggedIn;

        [ObservableProperty]
        private string playerName;
        partial void OnPlayerNameChanged(string value)
        {
            IsUserLoggedIn = value != Utility.Constants.DEFAULT_GUEST_DISPLAY_NAME;
        }

        [RelayCommand]
        public async Task StartNewGame()
        {
            IsLoadingGame = true;
            NewGameText = "Loading...";
            try
            {
                _sudokuBoardService.LoadPlayerBoard();
                await _sudokuGeneratorService.GenerateSudokuBoard(Utility.Constants.NUMBER_OF_GENERATED_GAMES);
                await Task.Delay(1000);
                _navigateToNewGameService.Navigate();
            }
            catch (SudokuProviderUnavailableException)
            {
                DialogMessage = "Unable to create new game. Service provider is not available. Try again later.";
                IsDialogOpen = true;
            }
            catch (DailyNumberOfGamesExceededException)
            {
                DialogMessage = "Daily number of games are exceeded. You can play unlimited as a guest, or try again tomorrow with this account.";
                IsDialogOpen = true;
            }
            finally
            {
                IsLoadingGame = false;
                NewGameText = "Start New Game";
            }

        }

        public IRelayCommand LoginCommand { get; }
        public IRelayCommand LogoutCommand { get; }
        public IRelayCommand RegisterCommand { get; }
        public IRelayCommand StandingsCommand { get; }
        public IRelayCommand AccountInformationsCommand { get; }

        private readonly SudokuGeneratorService _sudokuGeneratorService;
        private readonly SudokuBoardService _sudokuBoardService;
        private readonly NavigationService _navigateToNewGameService;
        private readonly NavigationService _navigateToLoginService;
        private readonly NavigationService _navigateToRegisterService;
        private readonly NavigationService _navigateToStandingsService;
        private readonly NavigationService _navigateToAccountService;
        private readonly ThemeChangeService _themeChangeService;
        private readonly SelectedThemeStore _selectedThemeStore;
        private readonly SudokuPlayerService _sudokuPlayerService;

        public OptionsViewModel(SudokuGeneratorService sudokuGeneratorService,
            NavigationService navigateToNewGameService, NavigationService navigateToLoginService,
            NavigationService navigateToRegisterService, NavigationService navigateToStandingsService, NavigationService navigateToAccountService, SelectedThemeStore selectedThemeStore,
            ThemeChangeService themeChangeService, SudokuPlayerStore sudokuPlayerStore, SudokuPlayerService sudokuPlayerService, SudokuBoardService sudokuBoardService)
        {
            _sudokuGeneratorService = sudokuGeneratorService;
            _sudokuBoardService = sudokuBoardService;
            _navigateToNewGameService = navigateToNewGameService;
            _navigateToLoginService = navigateToLoginService;
            _navigateToRegisterService = navigateToRegisterService;
            _navigateToStandingsService = navigateToStandingsService;
            _navigateToAccountService = navigateToAccountService;
            _themeChangeService = themeChangeService;
            _selectedThemeStore = selectedThemeStore;
            _sudokuPlayerService = sudokuPlayerService;

            selectedThemeStore.LoadTheme();
            themeChangeService.ApplyCurrentTheme();
            currentTheme = string.Empty;
            isLoadingGame = false;
            newGameText = "Start New Game";
            isDialogOpen = false;
            dialogMessage = string.Empty;
            playerName = sudokuPlayerStore.CurrentPlayer?.DisplayName ?? Utility.Constants.DEFAULT_GUEST_DISPLAY_NAME;
            isUserLoggedIn = playerName != Utility.Constants.DEFAULT_GUEST_DISPLAY_NAME;
            isDark = _selectedThemeStore.IsDark;
            currentTheme = _selectedThemeStore.ThemeString;

            LoginCommand = new RelayCommand(Login, CanLogin);
            LogoutCommand = new RelayCommand(Logout);
            RegisterCommand = new RelayCommand(Register);
            StandingsCommand = new RelayCommand(Standings);
            AccountInformationsCommand = new RelayCommand(AccountInformations);
        }

        public void Login()
        {
            _navigateToLoginService.Navigate();
        }
        public bool CanLogin()
        {
            return !IsUserLoggedIn;
        }

        public void Logout()
        {
            PlayerName = Utility.Constants.DEFAULT_GUEST_DISPLAY_NAME;
            _sudokuPlayerService.Logout();
        }

        public void Register()
        {
            _navigateToRegisterService.Navigate();
        }

        public void Standings()
        {
            _navigateToStandingsService.Navigate();
        }

        public void AccountInformations()
        {
            _navigateToAccountService.Navigate();
        }
    }
}
