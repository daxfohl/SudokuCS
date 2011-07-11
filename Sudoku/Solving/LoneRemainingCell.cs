using System.Collections.Generic;
using Sudoku.Model;

namespace Sudoku.Solving {
    /// <summary>
    ///   This finds cells where that cell
    ///   is the only one in the region that contains a number N.  If it finds one, then it
    ///   can obviously set that cell to N.
    /// </summary>
    public class LoneRemainingCell : Strategy {
        readonly Dictionary<IRegion, int> _checkedIteration = new Dictionary<IRegion, int>();

        protected override void OperateOn(SudokuModel model) {
            foreach (var region in model.AllRegions) {
                if (GetCheckedIteration(region) < region.LastChangedIteration) {
                    _checkedIteration[region] = model.ChangeCount;
                    foreach (var i in region.UnsolvedValues) {
                        var timesFound = 0;
                        Cell? foundAt = null;
                        foreach (var cell in region.Cells) {
                            if ((model.GetPossibilitySetCell(cell.Column, cell.Row) & (1 << i)) != 0) {
                                ++timesFound;
                                foundAt = cell;
                                if (timesFound > 1) {
                                    break;
                                }
                            }
                        }
                        if (timesFound == 1) {
                            if (foundAt != null) {
                                model.SetValueOptimized(foundAt.Value.Column, foundAt.Value.Row, i, region);
                            }
                        }
                    }
                }
            }
        }

        int GetCheckedIteration(IRegion region) {
            int value;
            _checkedIteration.TryGetValue(region, out value);
            return value;
        }
    }
}