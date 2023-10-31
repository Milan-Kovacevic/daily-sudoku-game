using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json.Linq;
using Sudoku.Messages;
using Sudoku.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.ViewModels
{
    public partial class SingleGridCellViewModel : ObservableObject
    {
        public required int GridNumber { get; init; }
        public required int CellNumber { get; init; }

        public string PreviousCellText { get; private set; }
        public bool PreviousIsTextInvalid { get; private set; }

        [ObservableProperty]
        private string cellText;
        [ObservableProperty]
        private bool isTextInvalid;
        [ObservableProperty]
        private bool isTextReadOnly;

        private readonly SudokuGuessService _sudokuGuessService;

        public SingleGridCellViewModel(SudokuGuessService sudokuGuessService)
        {
            _sudokuGuessService = sudokuGuessService;
            cellText = string.Empty;
            isTextInvalid = false;
            isTextReadOnly = false;

            PreviousCellText = string.Empty;
            PreviousIsTextInvalid = false;
        }

        public bool IsPreviousTextValid()
        {
            return !PreviousIsTextInvalid && PreviousCellText != string.Empty;
        }

        public bool IsValueEmpty()
        {
            return CellText == string.Empty;
        }

        internal void AddCellValue(int value)
        {
            if (value < 1 || value > 9 || IsTextReadOnly)
                return;
            if (int.TryParse(CellText, out var num) && num == value)
                return;
            IsTextInvalid = false;
            bool isValueCorrect = _sudokuGuessService.ValidateGuess(GridNumber, CellNumber, value);
            PreviousIsTextInvalid = IsTextInvalid;
            IsTextInvalid = !isValueCorrect;
            PreviousCellText = CellText;
            CellText = $"{value}";
            _sudokuGuessService.SendAddGuessMessage(GridNumber, CellNumber, value, isValueCorrect);
        }

        internal bool AddHintValue()
        {
            if (IsTextReadOnly || (!IsValueEmpty() && !IsTextInvalid))
                return false;
            var hintValue = _sudokuGuessService.RevealGridCellNumber(GridNumber, CellNumber);
            PreviousIsTextInvalid = IsTextInvalid;
            IsTextInvalid = false;
            PreviousCellText = CellText;
            CellText = $"{hintValue}";
            _sudokuGuessService.SendAddGuessMessage(GridNumber, CellNumber, hintValue, true);
            return true;
        }

        internal void DeleteCellValue()
        {
            if (IsTextReadOnly || IsValueEmpty())
                return;

            PreviousCellText = CellText;
            CellText = string.Empty;
            PreviousIsTextInvalid = IsTextInvalid;
            IsTextInvalid = false;
            _sudokuGuessService.SendRemoveGuessMessage(GridNumber, CellNumber);
        }

        internal void SendCellSelectedMessage()
        {
            if (IsTextReadOnly)
                return;
            var selecteCellMessage = new SelectedGridCellMessage() { GridNumber = this.GridNumber, CellNumber = this.CellNumber };
            WeakReferenceMessenger.Default.Send(selecteCellMessage);
        }
    }
}
