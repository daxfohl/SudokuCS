using System;
using Sudoku.Model;

namespace Sudoku.Solving {
    /// <summary>
    ///   This strategy runs through each strategy in a list, and if something is ever eliminated,
    ///   it immediately start back to the first strategy in the list and goes for another round.
    ///   For efficiency's sake, pass in the strategies from most efficient to least efficient.
    /// </summary>
    class MultiStrategy : Strategy {
        readonly Strategy[] _strategies;

        /// <summary>
        ///   For efficiency's sake, pass in the strategies from most efficient to least efficient.
        /// </summary>
        /// <param name = "strategies"></param>
        public MultiStrategy(params Strategy[] strategies) {
            _strategies = new Strategy[strategies.Length];
            strategies.CopyTo(_strategies, 0);
        }

        public event Action<Strategy> ContainedStrategyFinished;

        protected override void OperateOn(SudokuModel model) {
            if (_strategies.Length == 0) {
                return;
            }
            var i = 0;
            do {
                var strategy = _strategies[i];
                strategy.Run(model);
                if (ContainedStrategyFinished != null) {
                    ContainedStrategyFinished(strategy);
                }
                // If we've eliminated something, jump back to the first strategy; otherwise
                // go to the next strategy.
                i = (strategy.EliminatedLastRun != 0) ? 0 : (i + 1);
            } while (i < _strategies.Length);
        }
    }
}