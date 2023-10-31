using CommunityToolkit.Mvvm.ComponentModel;
using Sudoku.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.ViewModels
{
    public partial class StandingsItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private string playerName;

        [ObservableProperty]
        private string displayNameCode;

        [ObservableProperty]
        private int playerWins;

        [ObservableProperty]
        private int playerLoses;

        [ObservableProperty]
        private int totalScore;

        public required string Position { get; set; }

        public StandingsItemViewModel(SudokuPlayer player)
        {
            playerName = player.DisplayName;
            displayNameCode = "" + player.DisplayName[0];
            playerWins = player.Wins;
            playerLoses = player.Loses;
            totalScore = player.TotalScore;
        }
    }
}
