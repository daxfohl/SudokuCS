using System.Collections;
using System.Collections.Generic;

namespace Sudoku.Model {
    public abstract class Region {
        readonly SudokuModel _model;
        readonly BitArray _solved;

        protected Region(SudokuModel model) {
            _model = model;
            _solved = new BitArray(model.Size);
            LastChangedIteration = -1;
        }

        protected SudokuModel Model { get { return _model; } }

        #region Region Members

        public abstract Cell[] Cells { get; }
        public int LastChangedIteration { get; set; }

        public IEnumerable<int> UnsolvedValues {
            get {
                for (var i = 0; i < _model.Size; ++i) {
                    if (!_solved[i]) {
                        yield return i;
                    }
                }
            }
        }

        public abstract bool Contains(Cell cell);

        public void SetValueSolved(int value) {
            _solved[value] = true;
        }

        #endregion
    }
}