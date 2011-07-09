using Sudoku.Model;

namespace Sudoku.Solving {
    /// <summary>
    ///   The simplest strategy -- simply iterates through the cells and if a cell is solved, 
    ///   removes that number as a possibility from all other cells in the line, row, and square 
    ///   that contains the cell.
    /// </summary>
    public class DuplicateElimination : Strategy {
        protected override void OperateOn(SudokuModel model) {
            foreach (var cell in model.Cells) {
                EliminateDuplicates(cell);
            }
        }

        public static void EliminateDuplicates(Cell cell) {
            if (cell.IsSolved) {
                var value = cell.Value;
                foreach (var region in cell.IntersectingRegions) {
                    foreach (var other in region.Cells) {
                        if (other != cell) {
                            other.Eliminate(value);
                        }
                    }
                }
            }
        }

        public static void EliminateDuplicates(Cell cell, IRegion foundin) {
            if (cell.IsSolved) {
                var value = cell.Value;
                foreach (var region in cell.IntersectingRegions) {
                    if (region != foundin) {
                        foreach (var other in region.Cells) {
                            if (other != cell) {
                                other.Eliminate(value);
                            }
                        }
                    }
                }
            }
        }
    }
}