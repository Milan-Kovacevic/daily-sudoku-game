using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Messages
{
    public record class SudokuGuessMessage
    {
        public int GridNumber { get; init; }
        public int CellNumber { get; init; }
        public int Value { get; init; }
        public bool IsValid { get; init; }
        public bool IsValueRemoved { get; init; }

        public SudokuGuessMessage(int grid, int cell, int value = -1, bool isValid = false)
        {
            GridNumber = grid;
            CellNumber = cell;
            Value = value;

            if (value == -1)
                IsValueRemoved = true;
            IsValid = isValid;
        }
    }
}
