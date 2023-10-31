using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Messages
{
    public record class SelectedGridCellMessage
    {
        public int GridNumber { get; set; }
        public int CellNumber { get; set; }
    }
}
