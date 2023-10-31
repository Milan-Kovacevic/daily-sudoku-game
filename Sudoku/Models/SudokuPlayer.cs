using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Models
{
    public class SudokuPlayer
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public int Wins { get; set; } = 0;
        public int Loses { get; set; } = 0;
        public int TotalScore { get; set; } = 0;

        public override bool Equals(object? obj)
        {
            if (this == obj)
                return true;
            if (obj is not SudokuPlayer player || obj == null)
                return false;
            return player.Id.Equals(this.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() * 2 ^ 19 - 1;
        }
    }
}
