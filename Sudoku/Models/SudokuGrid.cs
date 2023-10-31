using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Models
{
    public class SudokuGrid
    {
        public List<List<int>> Value { get; set; } = new List<List<int>>();
        public List<List<int>> Solution { get; set; } = new List<List<int>>();
        public string Difficulty { get; set; } = "Easy";
    }
}
