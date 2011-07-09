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
        readonly Dictionary<IRegion, int> _checkedIteration = new Dictionary<IRegion, int>();

        protected override void OperateOn(SudokuModel model) {
            // We could look at all 2^N possible sets on all 3N of the model's regions, 
            // but it is more efficient to only use the
            // possiblity set of each cell, and isolate that set on each intersecting region.
            foreach (var cell in model.Cells) {
                if (!cell.IsSolved) {
                    foreach (var region in cell.IntersectingRegions) {
                        var checkedIter = GetCheckedIteration(region);
                        if (checkedIter < region.LastChangedIteration) {
                            _checkedIteration[region] = checkedIter;
                            IsolateSet(cell.PossibilitySet, region);
                        }
                    }
                }
            }
        }

        static void IsolateSet(BitArray32 set, IRegion region) {
            var cellsNotContainedBySet = region.Cells.Where(cell => !set.Contains(cell.PossibilitySet)).ToList();
            var numCellsContainedBySet = region.Cells.Count - cellsNotContainedBySet.Count;
            if (numCellsContainedBySet != set.HiBitCount) {
                return;
            }
            // we now know that each of the numbers in the set must be exclusively in
            // one of the contained cells, so we eliminate that set from all other cells'
            // possibility bits.
            var mask = ~set;
            foreach (var cell in cellsNotContainedBySet) {
                cell.PossibilitySet &= mask;
            }
        }

        int GetCheckedIteration(IRegion region) {
            if (!_checkedIteration.ContainsKey(region)) {
                _checkedIteration[region] = -1;
            }
            return _checkedIteration[region];
        }
    }
}