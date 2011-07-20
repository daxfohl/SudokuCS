using System;
using System.Collections.Generic;
using Sudoku.Model;

namespace Sudoku.Solving {
    /// <summary>
    ///   This finds cells where that cell
    ///   is the only one in the region that contains a number N.  If it finds one, then it
    ///   can obviously set that cell to N.
    /// </summary>
    public class LoneRemainingCell : Strategy {
        readonly Dictionary<Tuple<SudokuModel, Region>, int> _checkedIteration = new Dictionary<Tuple<SudokuModel, Region>, int>();

        protected override void OperateOn(SudokuModel model) {
            for (var iType = 0; iType < 3; ++iType) {
                var type = (RegionType)iType;
                for (var i = 0; i < model.Size; ++i) {
                    var region = new Region(type, i);
                    var tuple = new Tuple<SudokuModel, Region>(model, region);
                    if (GetCheckedIteration(tuple) < model.GetLastChangedIterRegion(type, i)) {
                        _checkedIteration[tuple] = model.ChangeCount;
                        foreach (var unsolved in model.GetUnsolvedValues(type, i)) {
                            var timesFound = 0;
                            Cell? foundAt = null;
                            foreach (var cell in model.GetCells(type, i)) {
                                if ((model.GetPossibilitySetCell(cell.Column, cell.Row) & (1 << unsolved)) != 0) {
                                    ++timesFound;
                                    foundAt = cell;
                                    if (timesFound > 1) {
                                        break;
                                    }
                                }
                            }
                            if (timesFound == 1) {
                                if (foundAt != null) {
                                    model.SetValueOptimized(foundAt.Value.Column, foundAt.Value.Row, unsolved, type, i);
                                }
                            }
                        }
                    }
                }
            }
        }

        int GetCheckedIteration(Tuple<SudokuModel, Region> tuple) {
            int value;
            _checkedIteration.TryGetValue(tuple, out value);
            return value;
        }
    }
}