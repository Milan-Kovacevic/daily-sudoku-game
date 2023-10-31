using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sudoku.Services;
using Sudoku.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.ViewModels
{
    public partial class StandingsViewModel : ObservableObject, IViewModelBase
    {
        private readonly NavigationService _navigateToOptionsService;

        private List<StandingsItemViewModel> _playerStandings;
        public IEnumerable<StandingsItemViewModel> PlayerStandings { get => _playerStandings; }

        [ObservableProperty]
        private bool isDialogOpen;

        [ObservableProperty]
        private string dialogMessage;

        public string? PlayerPlacement { get; init; }
        public int PlayerScore { get; init; }
        public int PlayerGamesPlayed { get; init; }

        [RelayCommand]
        public void ReturnBack()
        {
            _navigateToOptionsService.Navigate();
        }

        [RelayCommand]
        public void ShowDialog()
        {
            DialogMessage = "Score after each game is calculated based upon different factors: number of mistakes made, number of used hints, game difficulty and finish time";
            IsDialogOpen = true;
        }

        public StandingsViewModel(SudokuPlayerStore sudokuPlayerStore, NavigationService navigateToOptionsService)
        {
            _navigateToOptionsService = navigateToOptionsService;
            _playerStandings = new List<StandingsItemViewModel>();
            isDialogOpen = false;
            dialogMessage = string.Empty;

            var currentPlayer = sudokuPlayerStore.CurrentPlayer;
            if (currentPlayer == null)
                return;
            PlayerPlacement = "1.";
            var players = sudokuPlayerStore.GetAllSorted();
            for (int i = 0; i < players.Count(); i++)
            {
                _playerStandings.Add(new StandingsItemViewModel(players.ElementAt(i)) { Position = i + 1 + "." });
                if (players.ElementAt(i).Equals(currentPlayer))
                    PlayerPlacement = (i + 1) + ".";
            }

            PlayerScore = currentPlayer.TotalScore;
            PlayerGamesPlayed = currentPlayer.Wins + currentPlayer.Loses;
        }
    }
}
