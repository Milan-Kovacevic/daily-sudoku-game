using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Exceptions
{
    public class SudokuProviderUnavailableException : ApplicationException
    {
        public SudokuProviderUnavailableException() : base() { }

        public SudokuProviderUnavailableException(string? message) : base(message) { }

        public SudokuProviderUnavailableException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
