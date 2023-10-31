using Sudoku.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Services
{
    public interface ISudokuProvider
    {
        Task<SudokuBoard> GetSudokuBoard(int numberOfGrids);
    }
}
