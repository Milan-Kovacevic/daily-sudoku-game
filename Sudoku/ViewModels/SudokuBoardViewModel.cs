using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Sudoku.Messages;
using Sudoku.Models;
using Sudoku.Services;
using Sudoku.Stores;
using Sudoku.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Sudoku.ViewModels
{
    public partial class SudokuBoardViewModel : ObservableObject, IViewModelBase, IRecipient<SelectedGridCellMessage>, IRecipient<SudokuGuessMessage>, IRecipient<CurrentSudokuGridMessage>
    {
        private readonly SudokuBoardStore _sudokuBoardStore;
        private readonly SudokuPlayerStore _sudokuPlayerStore;
        private readonly SudokuGuessService _sudokuGuessService;
        private readonly SudokuPlayerService _sudokuPlayerService;
        private readonly DispatcherTimer gameClock;
        private int totalNumberOfGuesses;
        private int numberOfGuesses;
        private int numOfMistakes;

        public bool IsGameStarted { get; private set; }

        [ObservableProperty]
        private bool isBoardVisible;

        [ObservableProperty]
        private bool isPaused;

        [ObservableProperty]
        private bool canStartNewGame;

        [ObservableProperty]
        private bool isDialogOpen;
        [ObservableProperty]
        private string dialogTextTitle;
        [ObservableProperty]
        private string dialogTextSubtitle;

        [ObservableProperty]
        private string mistakes;

        [ObservableProperty]
        private int numberOfHints;
        [ObservableProperty]
        private bool canUseHint;

        [ObservableProperty]
        private string availableGames;

        [ObservableProperty]
        private bool isAlertActive;

        [ObservableProperty]
        private string totalScoreMessage;

        [ObservableProperty]
        private string gameDifficulty;

        [ObservableProperty]
        private string gameTime;
        private TimeSpan elapsedTime;

        partial void OnIsPausedChanged(bool value)
        {
            IsBoardVisible = !value;
        }

        private SelectedGridCellMessage? selectedGridCell;
        [RelayCommand]
        public void RemoveGuess()
        {
            if (selectedGridCell is null)
                return;
            int grid = selectedGridCell.GridNumber;
            int cell = selectedGridCell.CellNumber;

            SudokuBoard[grid].SudokuGrid[cell].DeleteCellValue();
        }

        [RelayCommand]
        public void UseHint()
        {
            if (selectedGridCell is null)
                return;
            int grid = selectedGridCell.GridNumber;
            int cell = selectedGridCell.CellNumber;

            if (SudokuBoard[grid].SudokuGrid[cell].AddHintValue())
            {
                NumberOfHints -= 1;
                if (NumberOfHints == 0)
                    CanUseHint = false;
            }
        }

        [RelayCommand]
        public void StartStop()
        {
            if (IsPaused)
            {
                gameClock.Start();
                IsPaused = false;
            }
            else
            {
                gameClock.Stop();
                IsPaused = true;
            }
        }

        [ObservableProperty]
        public ObservableCollection<SingleSudokuGridViewModel> sudokuBoard;

        public SudokuBoardViewModel(SudokuBoardStore sudokuBoardStore, SudokuGuessService sudokuGuessService, SudokuPlayerService sudokuPlayerService, SudokuPlayerStore sudokuPlayerStore)
        {
            _sudokuGuessService = sudokuGuessService;
            _sudokuPlayerService = sudokuPlayerService;
            _sudokuBoardStore = sudokuBoardStore;
            _sudokuPlayerStore = sudokuPlayerStore;

            gameTime = "00:00";
            isPaused = false;
            elapsedTime = TimeSpan.Zero;
            gameClock = new DispatcherTimer();
            gameClock.Interval = TimeSpan.FromSeconds(1);
            gameClock.Tick += OnGameClockTick;
            gameClock.Start();
            isBoardVisible = true;
            mistakes = $"0/{Constants.NUMBER_OF_REMAINING_MISTAKES}";
            numberOfHints = Constants.NUMBER_OF_REMAINING_HINTS;
            canUseHint = true;
            gameDifficulty = sudokuBoardStore.CurrentGrid?.Difficulty ?? "";
            availableGames = $"{sudokuBoardStore.NumberOfGrids}/{Constants.NUMBER_OF_GENERATED_GAMES}";
            numOfMistakes = 0;
            numberOfGuesses = 0;
            canStartNewGame = true;
            isAlertActive = false;
            totalScoreMessage = string.Empty;
            isDialogOpen = false;
            dialogTextTitle = string.Empty;
            dialogTextSubtitle = string.Empty;
            IsGameStarted = false;
            sudokuBoard = new ObservableCollection<SingleSudokuGridViewModel>();
            PopulateSudokuBoard();

            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        private void OnGameClockTick(object? sender, EventArgs e)
        {
            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));
            GameTime = elapsedTime.ToString(@"mm\:ss");
        }

        private void PopulateSudokuBoard()
        {
            var currGrid = _sudokuBoardStore.CurrentGrid;
            if (currGrid == null)
                return;

            var currGridValues = currGrid.Value;
            int totalNumberOfRevealed = 0;
            totalNumberOfGuesses = 0;
            var localBoard = new ObservableCollection<SingleSudokuGridViewModel>();
            int[,] translatedValues = Util.TranslateMatrixToGridFormat(currGridValues);
            for (int i = 0; i < 9; i++)
            {
                List<int> initialGridGuesses = new List<int>();
                for (int j = 0; j < 9; j++)
                {
                    if (translatedValues[i, j] != 0)
                        totalNumberOfRevealed++;

                    initialGridGuesses.Add(translatedValues[i, j]);
                }

                var singleSudokuGridVM = new SingleSudokuGridViewModel(_sudokuGuessService, i, initialGridGuesses);
                localBoard.Add(singleSudokuGridVM);
            }
            totalNumberOfGuesses = (9 * 9) - totalNumberOfRevealed;
            SudokuBoard = localBoard;
        }

        private void OnRemovePlayerGuess(int gridNumber, int cellNumber)
        {
            if (SudokuBoard[gridNumber].SudokuGrid[cellNumber].IsPreviousTextValid())
                numberOfGuesses--;
        }

        private void OnAddPlayerGuess(int gridNumber, int cellNumber, bool isCorrect)
        {
            if (isCorrect)
                numberOfGuesses++;
            else
            {
                if (SudokuBoard[gridNumber].SudokuGrid[cellNumber].IsPreviousTextValid())
                    numberOfGuesses--;
                numOfMistakes++;
                Mistakes = $"{numOfMistakes}/{Constants.NUMBER_OF_REMAINING_MISTAKES}";
            }

            if (numberOfGuesses >= totalNumberOfGuesses)
                HandleGameCompleted();
            else if (numOfMistakes == Constants.NUMBER_OF_REMAINING_MISTAKES)
                HandleGameOver();

            if (!IsGameStarted)
                IsGameStarted = true;
        }

        private void HandleGameCompleted()
        {
            if (numberOfGuesses < totalNumberOfGuesses)
                return;

            DialogTextTitle = "Congratulations";
            CanStartNewGame = _sudokuBoardStore.HasNewGrid;
            if (!CanStartNewGame)
                DialogTextSubtitle = "You have finished all challenges for today. Take a break.";
            else
                DialogTextSubtitle = $"You have successfully completed this challenge in {GameTime}";

            var points = CalculateNumberOfPoints(true);
            TotalScoreMessage = $"Total score: {((points < 0) ? "-" : "+")}{points} points";
            gameClock.Stop();
            IsDialogOpen = true;

            if (_sudokuPlayerStore.IsUserLoggedIn)
            {
                IsAlertActive = true;
                _sudokuPlayerService.UpdateScore(points, true);
            }
        }

        private void HandleGameOver()
        {
            DialogTextTitle = "Game Over!";
            DialogTextSubtitle = $"You have made {Constants.NUMBER_OF_REMAINING_MISTAKES} mistakes and lost the game.";
            CanStartNewGame = _sudokuBoardStore.HasNewGrid;
            if (!CanStartNewGame)
                DialogTextSubtitle += " There are no more challenges left.";

            var points = CalculateNumberOfPoints(false);
            TotalScoreMessage = $"Total score: {((points < 0) ? "-" : "+")}{points} points";

            gameClock.Stop();
            IsDialogOpen = true;
            if (_sudokuPlayerStore.IsUserLoggedIn)
            {
                IsAlertActive = true;
                _sudokuPlayerService.UpdateScore(points, false);
            }
        }

        private int CalculateNumberOfPoints(bool isCompleted)
        {
            int score = 0;
            if (GameDifficulty == "Hard")
            {
                score += numberOfGuesses * 10;
                score += 200;
                if (isCompleted)
                    score += (int)(600 / elapsedTime.TotalSeconds * 75);
            }
            else if (GameDifficulty == "Medium")
            {
                score += numberOfGuesses * 7;
                score += 150;
                if (isCompleted)
                    score += (int)(600 / elapsedTime.TotalSeconds * 50);
            }
            else
            {
                score += numberOfGuesses * 5;
                score += 75;
                if (isCompleted)
                    score += (int)(600 / elapsedTime.TotalSeconds * 30);
            }

            score += numOfMistakes * -30;
            score += (Constants.NUMBER_OF_REMAINING_HINTS - NumberOfHints) * -10;
            if (!isCompleted)
                score += -60;

            return score;
        }

        public void Receive(SelectedGridCellMessage message)
        {
            selectedGridCell = message;
        }

        public void Receive(SudokuGuessMessage message)
        {
            if (message.IsValueRemoved)
                OnRemovePlayerGuess(message.GridNumber, message.CellNumber);
            else
                OnAddPlayerGuess(message.GridNumber, message.CellNumber, message.IsValid);
        }

        public void Receive(CurrentSudokuGridMessage _)
        {
            PopulateSudokuBoard();
            numberOfGuesses = 0;
            numOfMistakes = 0;
            NumberOfHints = Constants.NUMBER_OF_REMAINING_HINTS;
            CanUseHint = true;
            Mistakes = $"0/{Constants.NUMBER_OF_REMAINING_MISTAKES}";
            IsDialogOpen = false;
            IsAlertActive = false;
            elapsedTime = TimeSpan.Zero;
            GameTime = "00:00";
            IsPaused = false;
            gameClock.Start();
            IsGameStarted = false;
            GameDifficulty = _sudokuBoardStore?.CurrentGrid?.Difficulty ?? "";
            AvailableGames = $"{_sudokuBoardStore?.NumberOfGrids ?? 0}/{Constants.NUMBER_OF_GENERATED_GAMES}";
        }

        public void Dispose()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }
    }
}
