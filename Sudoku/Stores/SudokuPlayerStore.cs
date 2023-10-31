using Newtonsoft.Json;
using Sudoku.Models;
using Sudoku.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Stores
{
    public class SudokuPlayerStore
    {
        private const string SUDOKU_PLAYERS_JSON_FILE = "./Data/players.json";
        private HashSet<SudokuPlayer> _sudokuPlayers;

        public IEnumerable<SudokuPlayer> SudokuPlayers { get => _sudokuPlayers; }

        private SudokuPlayer? _currentPlayer;
        public SudokuPlayer? CurrentPlayer
        {
            get => _currentPlayer;
            set {
                _currentPlayer = value;
                CurrentPlayerChanged?.Invoke();
            }
        }
        public event Action? CurrentPlayerChanged;
        public bool IsUserLoggedIn { get => CurrentPlayer != null; }


        public void InsertPlayer(SudokuPlayer sudokuPlayer)
        {
            if (_sudokuPlayers.Contains(sudokuPlayer))
                return;
            _sudokuPlayers.Add(sudokuPlayer);
            SaveSudokuPlayers();
        }

        public bool UpdatePlayer(SudokuPlayer sudokuPlayer)
        {
            var foundPlayer = _sudokuPlayers.FirstOrDefault(p => p.Id == sudokuPlayer.Id);
            if (foundPlayer == null)
                return false;

            _sudokuPlayers.Remove(foundPlayer);
            _sudokuPlayers.Add(sudokuPlayer);
            SaveSudokuPlayers();
            return true;
        }

        public SudokuPlayer? FindByUsername(string username)
        {
            return SudokuPlayers.FirstOrDefault(p => p.Username == username);
        }

        public bool ExistsByUsername(string username)
        {
            return FindByUsername(username) != null;
        }

        public IEnumerable<SudokuPlayer> GetAllSorted()
        {
            var players = new List<SudokuPlayer>(_sudokuPlayers);
            // Sorts highests to lowest scores
            players.Sort((p1, p2)=> p2.TotalScore - p1.TotalScore);
            return players;
        }

        private readonly string _repositoryLocation;

        public SudokuPlayerStore(string repositoryLocation = SUDOKU_PLAYERS_JSON_FILE)
        {
            _repositoryLocation = repositoryLocation;
            _sudokuPlayers = new HashSet<SudokuPlayer>();
            if (!File.Exists(repositoryLocation))
            {
                // Creating initial file of sudoku players...
                File.Create(repositoryLocation);
            }
        }

        public void LoadSudokuPlayers()
        {
            try
            {
                var sudokuPlayers = JsonConvert.DeserializeObject<SudokuPlayer[]>(File.ReadAllText(_repositoryLocation));
                if (sudokuPlayers != null)
                    _sudokuPlayers = new HashSet<SudokuPlayer>(sudokuPlayers);
            }
            catch
            {
                // TODO: Handle
            }
        }

        private void SaveSudokuPlayers()
        {
            var playersJson = JsonConvert.SerializeObject(SudokuPlayers.ToArray(), Formatting.Indented);
            File.WriteAllText(_repositoryLocation, playersJson);
        }
    }
}
