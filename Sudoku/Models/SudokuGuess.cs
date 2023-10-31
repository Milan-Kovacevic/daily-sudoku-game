using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Models
{
    public class SudokuGuess
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public required int GridNumber { get; set; }
        public required int CellNumber { get; set; }
        public int Value { get; set; }

        public SudokuGuess(int row, int column, int value)
        {
            Row = row;
            Column = column;
            Value = value;
        }
    }
}
