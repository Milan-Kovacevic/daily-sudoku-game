using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sudoku.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.ViewModels
{
    public partial class LoginViewModel : ObservableValidator, IViewModelBase
    {
        [ObservableProperty]
        [Required(ErrorMessage = "Username is required!")]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string username;

        [ObservableProperty]
        [Required(ErrorMessage = "Password is required!")]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string password;

        [ObservableProperty]
        private bool isLogging;

        [ObservableProperty]
        private bool isDialogOpen;

        [ObservableProperty]
        private string dialogMessage;

        private readonly NavigationService _navigateToOptionsService;
        private readonly SudokuPlayerService _sudokuPlayerService;

        public IRelayCommand LoginCommand { get; }

        public LoginViewModel(NavigationService navigateToOptionsService, SudokuPlayerService sudokuPlayerService)
        {
            _navigateToOptionsService = navigateToOptionsService;
            _sudokuPlayerService = sudokuPlayerService;
            username = string.Empty;
            password = string.Empty;
            isLogging = false;
            isDialogOpen = false;
            dialogMessage = string.Empty;
            LoginCommand = new AsyncRelayCommand(Login, CanLogin);
        }
        public async Task Login()
        {
            IsLogging = true;
            
            try
            {
                await Task.Delay(1000);
                _sudokuPlayerService.Login(Username, Password);
                _navigateToOptionsService.Navigate();
            }
            catch(Exception ex)
            {
                DialogMessage = ex.Message;
                IsDialogOpen = true;
            }
            finally
            {
                IsLogging = false;
            }
        }

        public bool CanLogin()
        {
            return Username != string.Empty
                && Password != string.Empty
                && !HasErrors;
        }

        [RelayCommand]
        public void ReturnBack()
        {
            _navigateToOptionsService.Navigate();
        }
    }
}
