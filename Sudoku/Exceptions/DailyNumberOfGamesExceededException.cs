using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Exceptions
{
    internal class DailyNumberOfGamesExceededException : ApplicationException
    {
        public DailyNumberOfGamesExceededException() : base() { }

        public DailyNumberOfGamesExceededException(string? message) : base(message) { }

        public DailyNumberOfGamesExceededException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
