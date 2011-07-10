using System;
using System.Collections.Generic;
using Sudoku.Types;

namespace Sudoku.Model {
    /// <summary>
    ///   Represents a single cell in the model
    /// </summary>
    public class Cell {
        readonly int _col;
        readonly SudokuModel _model;
        readonly int _row;
        int _lastChangedIteration = -1;

        int _possibilitySet;
        IRegion[] _regions;
        int _value = -1;

        public Cell(SudokuModel model, int col, int row) {
            _model = model;
            _col = col;
            _row = row;
            _possibilitySet = model.PossibilitySet;
            RemainingPossibilityCount = model.Size;
        }

        public Cell(SudokuModel model, Cell cell) {
            _model = model;
            _col = cell._col;
            _row = cell._row;
            _possibilitySet = cell._possibilitySet;
            RemainingPossibilityCount = cell.RemainingPossibilityCount;
            _value = cell._value;
            _lastChangedIteration = cell._lastChangedIteration;
        }

        #region Properties and Methods

        /// <summary>
        ///   The value of a solved cell (returns -1 if the cell is not solved).
        /// </summary>
        public int Value {
            get { return _value; }
            set {
                if (_value == value) {
                    return;
                }
                _possibilitySet = 1 << value;
                RemainingPossibilityCount = 1;
                _value = value;
                if (Changed != null) {
                    Changed(this, null);
                }
            }
        }

        public bool IsSolved {
            get { return _value != -1; }
        }

        public int RowIndex {
            get { return _row; }
        }

        public int ColumnIndex {
            get { return _col; }
        }

        public Row Row {
            get { return _model.Rows[_row]; }
        }

        public Column Column {
            get { return _model.Columns[_col]; }
        }

        public Square Square {
            get { return _model.GetSquare(this); }
        }

        public int RemainingPossibilityCount { get; private set; }

        /// <summary>
        ///   A bitflag that shows which numbers are / aren't eliminated; ie, 001001001 would
        ///   mean numbers 0, 3, and 6 are still possible, but the rest are eliminated.
        /// </summary>
        public int PossibilitySet {
            get { return _possibilitySet; }
            set {
                if (_possibilitySet == value) {
                    return;
                }
                _possibilitySet = value;
                RemainingPossibilityCount = value.HiBitCount();
                if (RemainingPossibilityCount == 1) {
                    _value = value.FirstHiBitPosition();
                }
                if (Changed != null) {
                    Changed(this, null);
                }
            }
        }

        /// <summary>
        ///   Returns the Row, Column, and Square that contain that cell.
        /// </summary>
        public IEnumerable<IRegion> IntersectingRegions {
            get {
                if (_regions == null) {
                    _regions = new IRegion[3];
                    _regions[0] = Column;
                    _regions[1] = Row;
                    _regions[2] = Square;
                }
                return _regions;
            }
        }

        public int LastChangedIteration {
            get { return _lastChangedIteration; }
            set { _lastChangedIteration = value; }
        }

        public void SetValueOptimized(int val, IRegion foundin) {
            if (_value == val) {
                return;
            }
            _possibilitySet = 1 << val;
            RemainingPossibilityCount = 1;
            _value = val;
            if (Changed != null) {
                Changed(this, foundin);
            }
        }

        public void Eliminate(int i) {
            var mask = 1 << i;
            if ((_possibilitySet & mask) == 0) {
                return;
            }
            _possibilitySet = _possibilitySet & ~mask;
            if (--RemainingPossibilityCount == 1) {
                _value = _possibilitySet.FirstHiBitPosition();
            }
            if (Changed != null) {
                Changed(this, null);
            }
        }

        #endregion

        public event Action<Cell, IRegion> Changed;
    }
}