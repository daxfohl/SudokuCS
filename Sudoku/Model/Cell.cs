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

        public BitArray32 _possibilitySet;
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
                var oldVal = _possibilitySet;
                _possibilitySet = BitArray32.CreateWithNthBitOn(value);
                RemainingPossibilityCount = 1;
                _value = value;
                if (Changed != null) {
                    Changed(this, new CellChangedEventArgs(this, oldVal, _possibilitySet, null));
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
            get { return _model.Squares[this]; }
        }

        public int RemainingPossibilityCount { get; private set; }

        /// <summary>
        ///   A bitflag that shows which numbers are / aren't eliminated; ie, 001001001 would
        ///   mean numbers 0, 3, and 6 are still possible, but the rest are eliminated.
        /// </summary>
        public BitArray32 PossibilitySet {
            get { return _possibilitySet; }
            set {
                if (_possibilitySet == value) {
                    return;
                }
                var oldVal = _possibilitySet;
                _possibilitySet = value;
                RemainingPossibilityCount = value.HiBitCount;
                if (RemainingPossibilityCount == 1) {
                    _value = value.FirstHiBitPosition;
                }
                if (Changed != null) {
                    Changed(this, new CellChangedEventArgs(this, oldVal, _possibilitySet, null));
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
            var oldVal = _possibilitySet;
            _possibilitySet = BitArray32.CreateWithNthBitOn(val);
            RemainingPossibilityCount = 1;
            _value = val;
            if (Changed != null) {
                Changed(this, new CellChangedEventArgs(this, oldVal, _possibilitySet, foundin));
            }
        }

        public void Eliminate(int i) {
            if (!_possibilitySet[i]) {
                return;
            }
            var oldVal = _possibilitySet;
            _possibilitySet = _possibilitySet.ReplaceBit(i, false);
            if (--RemainingPossibilityCount == 1) {
                _value = _possibilitySet.FirstHiBitPosition;
            }
            if (Changed != null) {
                Changed(this, new CellChangedEventArgs(this, oldVal, _possibilitySet, null));
            }
        }

        #endregion

        public event EventHandler<CellChangedEventArgs> Changed;
    }

    public class CellChangedEventArgs : EventArgs {
        public Cell Cell;
        public BitArray32 EliminatedSet;
        public IRegion Foundin;
        public BitArray32 NewSet;
        public int NumEliminated;
        public BitArray32 OldSet;

        public CellChangedEventArgs(Cell cell, BitArray32 oldSet, BitArray32 newSet, IRegion foundin) {
            Cell = cell;
            OldSet = oldSet;
            NewSet = newSet;
            EliminatedSet = oldSet.Minus(newSet);
            NumEliminated = EliminatedSet.HiBitCount;
            Foundin = foundin;
        }
    }
}