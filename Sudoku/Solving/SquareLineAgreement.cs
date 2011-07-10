using System.Linq;
using Sudoku.Model;
using Sudoku.Types;

namespace Sudoku.Solving {
    /// <summary>
    ///   For each region r1 and r2, and each possible number n
    ///   This strategy determines if n is possible in any of the set of cells (r1 minus r2), and
    ///   if not, then we know it must be in (r1 intersection r2), so we can
    ///   eliminate it in (r2 minus r1).
    ///   We only check on r1 is a square and r2 is an intersecting line, or vice versa, since
    ///   other cases (r1 a line and r2 a line, or any non-intersecting regions) are trivial.
    /// </summary>
    class SquareLineAgreement : Strategy {
        protected override void OperateOn(SudokuModel model) {
            foreach (var square in model.Squares) {
                foreach (var line in square.IntersectingLines) {
                    AgreeRegions(model, square, line);
                    AgreeRegions(model, line, square);
                }
            }
        }

        static void AgreeRegions(SudokuModel model, IRegion r1, IRegion r2) {
            // For each number in the puzzle,
            for (var num = 0; num < model.Size; ++num) {
                // determine if the number is in the set of cells {r1 minus r2}
                var num1 = num;
                var found = r1.Cells.Any(cell => !r2.Contains(cell) && (cell._possibilitySet & (1 << num1)) != 0);
                // if not found, then we know it must be in {r1 intersection r2}, so we can
                // eliminate it in {r2 minus r1}
                if (found) {
                    continue;
                }
                foreach (var cell in r2.Cells.Where(cell => !r1.Contains(cell))) {
                    cell.Eliminate(num);
                }
            }
        }
    }
}