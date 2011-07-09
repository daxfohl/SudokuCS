using System.Collections;
using System.Collections.Generic;
using Sudoku.Types;

namespace Sudoku.Model {
    /// <summary>
    /// The abstract interface for Row, Column, and Square
    /// </summary>
    public interface IRegion {
        ICountable<Cell> Cells { get; }
        int LastChangedIteration { get; set; }
        IEnumerable<int> UnsolvedValues { get; }
        bool Contains(Cell cell);
        void SetValueSolved(int value);
    }

    public abstract class Region : IRegion {
        readonly SudokuModel _model;
        readonly BitArray _solved;

        protected Region(SudokuModel model) {
            _model = model;
            _solved = new BitArray(model.Size);
            LastChangedIteration = -1;
        }

        public abstract ICountable<Cell> Cells { get; }
        public int LastChangedIteration { get; set; }

        public IEnumerable<int> UnsolvedValues {
            get {
                for (int i = 0; i < _model.Size; ++i) {
                    if (!_solved[i]) {
                        yield return i;
                    }
                }
            }
        }

        protected SudokuModel Model {
            get { return _model; }
        }

        public abstract bool Contains(Cell cell);

        public void SetValueSolved(int value) {
            _solved[value] = true;
        }
    }
}