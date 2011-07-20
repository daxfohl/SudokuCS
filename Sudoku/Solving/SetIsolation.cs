using System.Collections.Generic;
using System.Linq;
using Sudoku.Model;
using Sudoku.Types;

namespace Sudoku.Solving {
    /// <summary>
    ///   This strategy isolates sets.  ie, given a set of possible numbers {1,2,3}, and a
    ///   region, if there are three cells whose remaining possibilities are a subset of {1,2,3}
    ///   we know that one of those cells must be 1, another must be 2, and another must be 3,
    ///   so we can eliminate possiblities 1, 2, and 3 from any other cell in the region.
    /// </summary>
    public class SetIsolation : Strategy {
        readonly Dictionary<Region, int> _checkedIteration = new Dictionary<Region, int>();

        protected override void OperateOn(SudokuModel model) {
            // We could look at all 2^N possible sets on all 3N of the model's regions, 
            // but it is more efficient to only use the
            // possiblity set of each cell, and isolate that set on each intersecting region.
            for (var row = 0; row < model.Size; ++row) {
                for (var col = 0; col < model.Size; ++col) {
                    if (!model.IsSolved(col, row)) {
                        foreach (var region in model.GetIntersectingRegions(col, row)) {
                            var checkedIter = GetCheckedIteration(region);
                            if (checkedIter < region.LastChangedIteration) {
                                _checkedIteration[region] = checkedIter;
                                IsolateSet(model.GetPossibilitySetCell(col, row), region, model);
                            }
                        }
                    }
                }
            }
        }

        static void IsolateSet(int set, Region region, SudokuModel model) {
            var cellsNotContainedBySet = region.Cells.Where(cell => (model.GetPossibilitySetCell(cell.Column, cell.Row) | set) != set).ToList();
            var numCellsContainedBySet = region.Cells.Length - cellsNotContainedBySet.Count;
            if (numCellsContainedBySet != set.HiBitCount()) {
                return;
            }
            // we now know that each of the numbers in the set must be exclusively in
            // one of the contained cells, so we eliminate that set from all other cells'
            // possibility bits.
            var mask = ~set;
            foreach (var cell in cellsNotContainedBySet) {
                var poss = model.GetPossibilitySetCell(cell.Column, cell.Row);
                model.SetPossibilitySetCell(cell.Column, cell.Row, poss & mask);
            }
        }

        int GetCheckedIteration(Region region) {
            if (!_checkedIteration.ContainsKey(region)) {
                _checkedIteration[region] = -1;
            }
            return _checkedIteration[region];
        }
    }
}