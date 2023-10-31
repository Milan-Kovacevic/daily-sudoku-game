using Sudoku.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Messages
{
    public record class CurrentSudokuGridMessage(SudokuGrid Grid)
    {
    }
}
