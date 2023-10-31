using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Models
{
    public class SudokuBoard
    {
        public HashSet<SudokuGrid> Grids { get; set; } = new HashSet<SudokuGrid>();
    }
}
