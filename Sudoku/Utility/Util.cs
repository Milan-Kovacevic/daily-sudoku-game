using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Utility
{
    internal static class Util
    {
        /// <summary>
        /// Translates specific cooridates from grid format to matrix format
        /// </summary>
        /// <param name="grid">Number of grid in a sudoku board (value range 0-8)</param>
        /// <param name="cell">Number of cell inside specific grid (value range 0-8)</param>
        /// <returns>Standard matrix coordinates in tuple (row, column) of a sudoku board (value range 0-8)</returns>
        public static (int, int) TranslateGridToMatrixCoordinates(int grid, int cell)
        {
            int row = cell / 3 + (grid / 3) * 3;
            int column = cell % 3 + grid % 3 * 3;
            return (row, column);
        }

        /// <summary>
        /// Translates specific cooridates from matrix format to grid format
        /// </summary>
        /// <param name="row">Row number in a sudoku board (value range 0-8)</param>
        /// <param name="column">Column number in a sudoku board (value range 0-8)</param>
        /// <returns>Grid coordinates in tuple (grid, cell) of a sudoku board (value range 0-8)</returns>
        public static (int, int) TranslateMatrixToGridCoordinates(int row, int column)
        {
            int grid = (row / 3) * 3 + column / 3;
            int cell = (column % 3) + (row % 3) * 3;
            return (grid, cell);
        }

        /// <summary>
        /// Translates entire sudoku matrix to grid format of sudoku board
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>Grid format with values ranging from 0 - 9 (0 means that there is no initial value in a specific cell)</returns>
        public static int[,] TranslateMatrixToGridFormat(List<List<int>> matrix)
        {
            int[,] gridFormat = new int[9, 9];

            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    var position = TranslateMatrixToGridCoordinates(i, j);
                    gridFormat[position.Item1, position.Item2] = matrix[i][j];
                }
            }
            return gridFormat;
        }

        /// <summary>
        /// Computes SHA256 hash of a specific string <paramref name="input"/> passed as an argument
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Hashed version of <paramref name="input"/> string</returns>
        public static string ComputeHash(string input)
        {
            using (var sha256 = SHA256.Create())
            { 
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
}
