using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sudoku.Models;
using Sudoku.Services;
using Sudoku.Stores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.ViewModels
{
    public partial class AccountViewModel : ObservableValidator, IViewModelBase
    {
        [ObservableProperty]
        private bool isDialogOpen;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(ChangeInformationsCommand))]
        [Required(ErrorMessage = "Display name is required!")]
        private string displayName;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(ChangeInformationsCommand))]
        [NotifyPropertyChangedFor(nameof(RepeatPassword))]
        [MinLength(6, ErrorMessage = "Password should be at least 6 characters!")]
        [Required(ErrorMessage = "Password is required")]
        private string password;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(ChangeInformationsCommand))]
        [CustomValidation(typeof(AccountViewModel), nameof(ValidateRepeatPassword))]
        private string repeatPassword;

        // Custom validation
        public static ValidationResult? ValidateRepeatPassword(string password, ValidationContext context)
        {
            var instance = (AccountViewModel)context.ObjectInstance;
            var isValid = instance.Password == password;

            if (isValid)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Passwords must match!");
        }

        public IRelayCommand ChangeInformationsCommand { get; }

        [RelayCommand]
        public void ConfirmChange()
        {
            if(_sudokuPlayerService.UpdateAccountInfo(Username, DisplayName, Password))
            {
                _navigateToOptionsService.Navigate();
            }
        }

        [RelayCommand]
        public void ReturnBack()
        {
            _navigateToOptionsService.Navigate();
        }

        private readonly SudokuPlayerStore _sudokuPlayerStore;
        private readonly SudokuPlayerService _sudokuPlayerService;
        private readonly NavigationService _navigateToOptionsService;

        public AccountViewModel(SudokuPlayerStore sudokuPlayerStore, SudokuPlayerService sudokuPlayerService, NavigationService navigateToOptionsService)
        {
            _sudokuPlayerStore = sudokuPlayerStore;
            _sudokuPlayerService = sudokuPlayerService;
            _navigateToOptionsService = navigateToOptionsService;
            var currentPlayer = sudokuPlayerStore.CurrentPlayer;
            if (currentPlayer == null)
                throw new Exception("Internal error. Unable to access logged player account");
            displayName = currentPlayer.DisplayName;
            username = currentPlayer.Username;
            password = string.Empty;
            repeatPassword = string.Empty;
            isDialogOpen = false;
            ChangeInformationsCommand = new RelayCommand(ChangeInformations, CanChangeInformations);
        }

        private void ChangeInformations()
        {
            IsDialogOpen = true;
        }

        private bool CanChangeInformations()
        {
            return Username != string.Empty 
                && Password != string.Empty 
                && RepeatPassword != string.Empty 
                && DisplayName != string.Empty 
                && !HasErrors;
        }
    }
}
