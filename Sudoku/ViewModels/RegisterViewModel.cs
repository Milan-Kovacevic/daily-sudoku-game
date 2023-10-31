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
    public partial class RegisterViewModel : ObservableValidator, IViewModelBase
    {
        [ObservableProperty]
        private bool isDialogOpen;

        [ObservableProperty]
        private string dialogMessage;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [Required(ErrorMessage = "Display name is required!")]
        private string displayName;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [Required(ErrorMessage = "Username is required!")]
        private string username;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [NotifyPropertyChangedFor(nameof(RepeatPassword))]
        [MinLength(6, ErrorMessage = "Password should be at least 6 characters!")]
        [Required(ErrorMessage = "Password is required")]
        private string password;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [CustomValidation(typeof(RegisterViewModel), nameof(ValidateRepeatPassword))]
        private string repeatPassword;

        // Custom validation
        public static ValidationResult? ValidateRepeatPassword(string password, ValidationContext context)
        {
            var instance = (RegisterViewModel)context.ObjectInstance;
            var isValid = instance.Password == password;

            if (isValid)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Passwords must match!");
        }

        [ObservableProperty]
        private bool isRegistering;

        public IRelayCommand RegisterCommand { get; }

        public async Task Register()
        {
            try
            {
                IsRegistering = true;
                await Task.Delay(1000);
                _sudokuPlayerService.Register(Username, Password, DisplayName);
                _navigateToOptionsService.Navigate();
            }
            catch(Exception ex)
            {
                DialogMessage = ex.Message;
                IsDialogOpen = true;
            }
            finally
            {
                IsRegistering = false;
            }
        }

        public bool CanRegister()
        {
            return DisplayName != string.Empty 
                && Username != string.Empty 
                && Password != string.Empty 
                && RepeatPassword != string.Empty
                && !HasErrors;
        }

        [RelayCommand]
        public void ReturnBack()
        {
            _navigateToOptionsService.Navigate();
        }

        private readonly NavigationService _navigateToOptionsService;
        private readonly SudokuPlayerService _sudokuPlayerService;

        public RegisterViewModel(NavigationService navigateToOptionsService, SudokuPlayerService sudokuPlayerService)
        {
            _navigateToOptionsService = navigateToOptionsService;
            _sudokuPlayerService = sudokuPlayerService;
            displayName = string.Empty;
            username = string.Empty;
            password = string.Empty;
            repeatPassword = string.Empty;
            isDialogOpen = false;
            dialogMessage = string.Empty;
            isRegistering = false;

            RegisterCommand = new AsyncRelayCommand(Register, CanRegister);
        }
    }
}
