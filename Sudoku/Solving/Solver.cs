using System;
using Sudoku.Model;

namespace Sudoku.Solving {
    /// <summary>
    ///   The primary sudoku solver class
    /// </summary>
    public class Solver {
        readonly Strategy _eliminationStrategy;
        readonly SudokuModel _model;
        public DateTime FinishTime = DateTime.MinValue;
        public DateTime StartTime = DateTime.MinValue;

        public Solver(SudokuModel model) {
            _model = model;
            var multiStrategy = new MultiStrategy(
                new LoneRemainingCell(),
                new SetIsolation(),
                new TrialAndError(new LoneRemainingCell()));
            multiStrategy.ContainedStrategyFinished += HandleScanFinished;
            _eliminationStrategy = multiStrategy;
        }

        public TimeSpan RunTime {
            get {
                if (StartTime == DateTime.MinValue) {
                    return TimeSpan.Zero;
                }
                if (FinishTime == DateTime.MinValue) {
                    return DateTime.Now - StartTime;
                }
                return FinishTime - StartTime;
            }
        }

        public event Action<Strategy> ScanFinished;
        public event Action Finished;

        public void Solve() {
            StartTime = DateTime.Now;
            _eliminationStrategy.Run(_model);
            FinishTime = DateTime.Now;
            if (Finished != null) {
                Finished();
            }
        }

        void HandleScanFinished(Strategy e) {
            // bubble up the event
            if (ScanFinished != null) {
                ScanFinished(e);
            }
        }
    }
}