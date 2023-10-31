using Sudoku.Models;
using Sudoku.Stores;
using Sudoku.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Services
{
    public class SudokuPlayerService
    {
        const string INVALID_LOGIN_CREDENTIALS_MESSAGE = "Login credentials are invalid";
        const string PLAYER_ALREADY_EXIST_MESSAGE = "Username is already taken";
        private readonly SudokuPlayerStore _sudokuPlayerStore;

        public SudokuPlayerService(SudokuPlayerStore sudokuPlayerStore)
        {
            _sudokuPlayerStore = sudokuPlayerStore;
        }

        public bool IsPlayerLoggedIn()
        {
            return _sudokuPlayerStore.IsUserLoggedIn;
        }

        public void Login(string username, string password)
        {

            SudokuPlayer player = _sudokuPlayerStore.FindByUsername(username) ?? throw new Exception(INVALID_LOGIN_CREDENTIALS_MESSAGE);
            if (player.Password != Util.ComputeHash(password))
                throw new Exception(INVALID_LOGIN_CREDENTIALS_MESSAGE);

            _sudokuPlayerStore.CurrentPlayer = player;
        }

        public void Logout()
        {
            _sudokuPlayerStore.CurrentPlayer = null;
        }

        public void Register(string username, string password, string displayName)
        {
            if (username.ToLower() == Constants.DEFAULT_GUEST_DISPLAY_NAME.ToLower())
                throw new Exception(PLAYER_ALREADY_EXIST_MESSAGE);
            if (_sudokuPlayerStore.ExistsByUsername(username))
                throw new Exception(PLAYER_ALREADY_EXIST_MESSAGE);
            SudokuPlayer newPlayer = new SudokuPlayer()
            {
                Id = Guid.NewGuid(),
                Username = username,
                Password = Util.ComputeHash(password),
                DisplayName = displayName,
            };

            _sudokuPlayerStore.InsertPlayer(newPlayer);
        }

        public bool UpdateAccountInfo(string username, string displayName, string password)
        {
            var currentPlayer = _sudokuPlayerStore.CurrentPlayer;
            SudokuPlayer player = new SudokuPlayer()
            {
                Id = currentPlayer?.Id ?? Guid.NewGuid(),
                Username = username,
                Password = Util.ComputeHash(password),
                DisplayName = displayName,
                Loses = currentPlayer?.Loses ?? 0,
                Wins = currentPlayer?.Wins ?? 0,
                TotalScore = currentPlayer?.TotalScore ?? 0
            };

            var isSuccessfull = _sudokuPlayerStore.UpdatePlayer(player);
            if (isSuccessfull)
                _sudokuPlayerStore.CurrentPlayer = player;
            return isSuccessfull;
        }

        public void UpdateScore(int points, bool isCompleted)
        {
            var currentPlayer = _sudokuPlayerStore.CurrentPlayer;
            if (currentPlayer == null)
                return;
            currentPlayer.TotalScore += points;
            if (isCompleted)
                currentPlayer.Wins += 1;
            else
                currentPlayer.Loses += 1;
            _sudokuPlayerStore.UpdatePlayer(currentPlayer);
        }
    }
}
