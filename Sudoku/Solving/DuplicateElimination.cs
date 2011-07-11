using Sudoku.Model;

namespace Sudoku.Solving {
    /// <summary>
    ///   The simplest strategy -- simply iterates through the cells and if a cell is solved, 
    ///   removes that number as a possibility from all other cells in the line, row, and square 
    ///   that contains the cell.
    /// </summary>
    public class DuplicateElimination : Strategy {
        protected override void OperateOn(SudokuModel model) {
            for (var row = 0; row < model.Size; ++row) {
                for (var col = 0; col < model.Size; ++col) {
                    EliminateDuplicates(model, col, row);
                }
            }
        }

        public static void EliminateDuplicates(SudokuModel model, int col, int row) {
            if (model.IsSolved(col, row)) {
                var value = model.GetValue(col, row);
                foreach (var region in model.GetIntersectingRegions(col, row)) {
                    foreach (var other in region.Cells) {
                        if (other.Column != col || other.Row != row) {
                            model.Eliminate(other.Column, other.Row, value);
                        }
                    }
                }
            }
        }
        public static void EliminateDuplicates(SudokuModel model, int col, int row, IRegion foundin) {
            if (model.IsSolved(col, row)) {
                var value = model.GetValue(col, row);
                foreach (var region in model.GetIntersectingRegions(col, row)) {
                    if (region != foundin) {
                        foreach (var other in region.Cells) {
                            if (other.Column != col || other.Row != row) {
                                model.Eliminate(other.Column, other.Row, value);
                            }
                        }
                    }
                }
            }
        }
    }
}