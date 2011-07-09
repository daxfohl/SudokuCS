using System;
using Sudoku.Model;

namespace Sudoku.Solving {
    /// <summary>
    ///   Abstract base class that is a strategy for eliminating possibilities.
    /// </summary>
    public abstract class Strategy {
        /// <summary>
        ///   Number of possibilities that were eliminated in the last run of the strategy.
        /// </summary>
        public int EliminatedLastRun;

        /// <summary>
        ///   Number of times the strategy has been run.
        /// </summary>
        public int Runs;

        /// <summary>
        ///   Amount of time the strategy took to complete the previous run.
        /// </summary>
        public TimeSpan TimeLastRun = TimeSpan.Zero;

        /// <summary>
        ///   Total number of possibilities that this strategy has eliminated in its lifetime.
        /// </summary>
        public int TotalEliminated;

        /// <summary>
        ///   Total amount of time that the strategy has taken to complete all runs it has done so far.
        /// </summary>
        public TimeSpan TotalTime = TimeSpan.Zero;

        public void Run(SudokuModel model) {
            var initTime = DateTime.Now;
            var initElim = model.EliminatedCount;
            OperateOn(model);
            TimeLastRun = DateTime.Now - initTime;
            TotalTime += TimeLastRun;
            EliminatedLastRun = model.EliminatedCount - initElim;
            TotalEliminated += EliminatedLastRun;
            ++Runs;
        }

        protected abstract void OperateOn(SudokuModel model);
    }
}