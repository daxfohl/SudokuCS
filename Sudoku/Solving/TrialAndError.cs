using System.Threading.Tasks;
using Sudoku.Model;
using Sudoku.Types;

namespace Sudoku.Solving {
    /// <summary>
    ///   This strategy sets an unsolved cell to a possible value, then runs the elimination
    ///   strategies to determine if an error results.  If an inconsistency results, then we 
    ///   can eliminate that value from that cell's possibilites.
    /// </summary>
    class TrialAndError : Strategy {
        readonly Strategy _elimStrategy;
        int _index;
        /// <summary>
        ///   The constructor
        /// </summary>
        /// <param name = "elimStrategies">The elimination strategies to run</param>
        public TrialAndError(params Strategy[] elimStrategies) {
            _elimStrategy = new MultiStrategy(elimStrategies);
        }

        protected override void OperateOn(SudokuModel model) {
            // we retain the index, as it's more efficient 
            // to keep operating in the same area than to start over at cell[0,0]
            // every time.
            var sz = model.SizeSquared;
            Parallel.For(0, 625, (i, loopState) => {
                int index;
                lock (this) {
                    index = _index;
                    _index += 36;
                    _index %= sz;
                }
                var col = index % 25;
                var row = index / 25;
                if (OperateOn(col, row, model)) {
                    loopState.Stop();
                }
                // go to the next cell
            });
            // if we've gone through the whole model
            // without eliminating anything, then there's no use
            // trying anymore.
        }

        void tOperateOn(SudokuModel model) {
            // we retain the index, as it's more efficient 
            // to keep operating in the same area than to start over at cell[0,0]
            // every time.
            var sz = model.SizeSquared;
            for (int i = 0; i < sz; ++i) {
                int index;
                lock (this) {
                    index = _index;
                    _index += 36;
                    _index %= sz;
                }
                var col = index % 25;
                var row = index / 25;
                if (OperateOn(col, row, model)) {
                    return;
                }
                // go to the next cell
            }
            // if we've gone through the whole model
            // without eliminating anything, then there's no use
            // trying anymore.
        }

        bool OperateOn(int col, int row, SudokuModel model) {
            if (!model.IsSolved(col, row)) {
                // obviously can't eliminate anything in a solved cell.
                // try each int i that is a possibility for the cell, and see if an error results
                foreach (var i in model.GetPossibilitySetCell(col, row).HighBitPositions()) {
                    var clone = new SudokuModel(model);
                    clone.SetValue(col, row, i);
                    _elimStrategy.Run(clone);
                    if (clone.IsConsistent()) {
                        continue;
                    }
                    // If so, we can eliminate that from the original model.
                    model.Eliminate(col, row, i);
                    return true;
                    // return immediately after eliminating something, since
                    // this is a slow algorithm, and we want our faster
                    // algorithms to see if they make some more eliminations first.
                }
            }
            return false;
        }
    }
}