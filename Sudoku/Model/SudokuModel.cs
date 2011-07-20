using System;
using System.Collections.Generic;
using System.Linq;
using Sudoku.Solving;
using Sudoku.Types;

namespace Sudoku.Model {
    /// <summary>
    ///   The main sudoku model.
    /// </summary>
    public class SudokuModel {
        readonly List<Region> _allRegions;
        readonly Column[] _cols;
        readonly int[,] _lastChangedIteration;
        readonly int[,] _possibilitySetCell;
        readonly int _possibilitySetModel;
        readonly Region[,][] _regions;
        readonly int[,] _remainingPossibilityCount;
        readonly Row[] _rows;
        readonly int _size;
        readonly int _sizeCubed;
        readonly int _sizeSqrt;
        readonly int _sizeSquared;
        readonly Square[,] _squares;
        readonly int[,] _value;


        /// <summary>
        ///   Standard constructor
        /// </summary>
        /// <param name = "size">Length of one side of the square</param>
        public SudokuModel(int size) {
            // Initialize our variables
            _size = size;
            _sizeSqrt = Convert.ToInt32(Math.Sqrt(size));
            _sizeSquared = size * size;
            _sizeCubed = _sizeSquared * size;
            _possibilitySetModel = (1 << _size) - 1;
            _possibilitySetCell = new int[Size,Size];
            _remainingPossibilityCount = new int[Size,Size];
            _value = new int[Size,Size];
            _lastChangedIteration = new int[Size,Size];
            _regions = new Region[Size,Size][];

            // Initialize the Cell objects, and the cache values
            for (var col = 0; col < size; ++col) {
                for (var row = 0; row < size; ++row) {
                    Cell(col, row);
                }
            }
            CellChanged += HandleCellChanged;

            // Initialize the Row and Column objects
            _rows = new Row[size];
            _cols = new Column[size];
            for (var i = 0; i < size; ++i) {
                _rows[i] = new Row(this, i);
                _cols[i] = new Column(this, i);
            }

            // Initialize the Square objects
            _squares = new Square[_sizeSqrt,_sizeSqrt];
            for (var sqrCol = 0; sqrCol < _sizeSqrt; ++sqrCol) {
                for (var sqrRow = 0; sqrRow < _sizeSqrt; ++sqrRow) {
                    _squares[sqrCol, sqrRow] = new Square(this, sqrCol, sqrRow);
                }
            }

            // Finally initialize the Regions object
            _allRegions = new List<Region>(_cols);
            _allRegions.AddRange(_rows);
            _allRegions.AddRange(_squares.Cast<Square>());
        }

        /// <summary>
        ///   Copy constructor
        /// </summary>
        /// <param name = "model"></param>
        public SudokuModel(SudokuModel model) {
            // Initialize our variables
            var size = model.Size;
            _size = size;
            _sizeSqrt = Convert.ToInt32(Math.Sqrt(size));
            _sizeSquared = size * size;
            _sizeCubed = _sizeSquared * size;
            _possibilitySetModel = (1 << _size) - 1;
            _possibilitySetCell = new int[Size,Size];
            _remainingPossibilityCount = new int[Size,Size];
            _value = new int[Size,Size];
            _lastChangedIteration = new int[Size,Size];
            _regions = new Region[Size,Size][];

            // Initialize the Cell objects, and the cache values
            for (var col = 0; col < size; ++col) {
                for (var row = 0; row < size; ++row) {
                    CloneCell(model, col, row);
                }
            }
            CellChanged += HandleCellChanged;

            // Initialize the Row and Column objects
            _rows = new Row[size];
            _cols = new Column[size];
            for (var i = 0; i < size; ++i) {
                _rows[i] = new Row(this, model.Rows[i]);
                _cols[i] = new Column(this, model.Columns[i]);
            }

            // Initialize the Square objects
            _squares = new Square[_sizeSqrt,_sizeSqrt];
            for (var sqrCol = 0; sqrCol < _sizeSqrt; ++sqrCol) {
                for (var sqrRow = 0; sqrRow < _sizeSqrt; ++sqrRow) {
                    _squares[sqrCol, sqrRow] = new Square(this, model.Squares[sqrCol, sqrRow]);
                }
            }

            // Finally initialize the Regions object
            _allRegions = new List<Region>(_cols);
            _allRegions.AddRange(_rows);
            _allRegions.AddRange(_squares.Cast<Square>());

            SolvedCount = model.SolvedCount;
            EliminatedCount = model.EliminatedCount;
            ChangeCount = model.ChangeCount;
            LastChangedCell = model.LastChangedCell;
        }

        /// <summary>
        ///   Length of a side of the puzzle (or also the number of numbers available).
        /// </summary>
        public int Size { get { return _size; } }

        /// <summary>
        ///   Length of a side of a square in the puzzle, 
        ///   or the number of squares on a side of a puzzle.
        /// </summary>
        public int SizeSqrt { get { return _sizeSqrt; } }

        /// <summary>
        ///   Number of cells in the puzzle.
        /// </summary>
        public int SizeSquared { get { return _sizeSquared; } }

        public int SizeCubed { get { return _sizeCubed; } }

        public Row[] Rows { get { return _rows; } }

        public Column[] Columns { get { return _cols; } }

        public Square[,] Squares { get { return _squares; } }

        public IEnumerable<Region> AllRegions { get { return _allRegions; } }

        public int SolvedCount { get; private set; }

        public int EliminatedCount { get; private set; }

        public int PossibilitySetModel { get { return _possibilitySetModel; } }

        public Cell LastChangedCell { get; private set; }

        public int ChangeCount { get; private set; }

        /// <summary>
        ///   Check for any cell that has no remaining possibilities, or any region that has no
        ///   cells that has a certain number.
        /// </summary>
        /// <returns>true if valid</returns>
        public bool IsConsistent() {
            for (var row = 0; row < Size; ++row) {
                for (var col = 0; col < Size; ++col) {
                    if (_possibilitySetCell[col, row] == 0) {
                        return false;
                    }
                }
            }
            foreach (var region in _allRegions) {
                var totalSet = region.Cells.Aggregate(0, (current, cell) => current | _possibilitySetCell[cell.Column, cell.Row]);
                if (totalSet != _possibilitySetModel) {
                    return false;
                }
            }
            return true;
        }

        public event Action<int, int> ModelChanged;

        public Square GetSquare(int col, int row) {
            var squareCol = col / _sizeSqrt;
            var squareRow = row / _sizeSqrt;
            return _squares[squareCol, squareRow];
        }


        /// <summary>
        ///   We update our cache variables and bubble up the event.
        /// </summary>
        void HandleCellChanged(int col, int row, Region foundIn) {
            EliminatedCount += 1;
            LastChangedCell = new Cell(col, row);
            var value = _value[col, row];
            foreach (var region in GetIntersectingRegions(col, row)) {
                region.LastChangedIteration = ChangeCount;
                if (value != -1) {
                    region.SetValueSolved(value);
                }
            }
            ++ChangeCount;
            if (value != -1) {
                ++SolvedCount;
                DuplicateElimination.EliminateDuplicates(this, col, row, foundIn);
            }
            if (ModelChanged != null) {
                ModelChanged(col, row);
            }
        }


        public event Action<int, int, Region> CellChanged;

        void Cell(int col, int row) {
            _possibilitySetCell[col, row] = PossibilitySetModel;
            _remainingPossibilityCount[col, row] = Size;
            _value[col, row] = -1;
            _lastChangedIteration[col, row] = -1;
            _regions[col, row] = null;
            CellChanged = null;
        }

        void CloneCell(SudokuModel other, int col, int row) {
            _possibilitySetCell[col, row] = other.GetPossibilitySetCell(col, row);
            _remainingPossibilityCount[col, row] = other.GetRemainingPossibilityCount(col, row);
            _value[col, row] = other.GetValue(col, row);
            _lastChangedIteration[col, row] = other.GetLastChangedIteration(col, row);
            _regions[col, row] = null;
            CellChanged = null;
        }


        /// <summary>
        ///   The value of a solved cell (returns -1 if the cell is not solved).
        /// </summary>
        public int GetValue(int col, int row) {
            return _value[col, row];
        }

        public void SetValue(int col, int row, int value) {
            if (_value[col, row] == value) {
                return;
            }
            _possibilitySetCell[col, row] = 1 << value;
            _remainingPossibilityCount[col, row] = 1;
            _value[col, row] = value;
            if (CellChanged != null) {
                CellChanged(col, row, null);
            }
        }

        public bool IsSolved(int col, int row) {
            return _value[col, row] != -1;
        }

        public int GetRemainingPossibilityCount(int col, int row) {
            return _remainingPossibilityCount[col, row];
        }

        /// <summary>
        ///   A bitflag that shows which numbers are / aren't eliminated; ie, 001001001 would
        ///   mean numbers 0, 3, and 6 are still possible, but the rest are eliminated.
        /// </summary>
        public int GetPossibilitySetCell(int col, int row) {
            return _possibilitySetCell[col, row];
        }

        public void SetPossibilitySetCell(int col, int row, int value) {
            if (_possibilitySetCell[col, row] == value) {
                return;
            }
            _possibilitySetCell[col, row] = value;
            _remainingPossibilityCount[col, row] = value.HiBitCount();
            if (_remainingPossibilityCount[col, row] == 1) {
                _value[col, row] = value.FirstHiBitPosition();
            }
            if (CellChanged != null) {
                CellChanged(col, row, null);
            }
        }

        /// <summary>
        ///   Returns the Row, Column, and Square that contain that cell.
        /// </summary>
        public IEnumerable<Region> GetIntersectingRegions(int col, int row) {
            if (_regions[col, row] == null) {
                _regions[col, row] = new Region[3];
                _regions[col, row][0] = _cols[col];
                _regions[col, row][1] = _rows[row];
                _regions[col, row][2] = GetSquare(col, row);
            }
            return _regions[col, row];
        }

        public void SetLastChangedIteration(int col, int row, int value) {
            _lastChangedIteration[col, row] = value;
        }

        public int GetLastChangedIteration(int col, int row) {
            return _lastChangedIteration[col, row];
        }

        public void SetValueOptimized(int col, int row, int val, Region foundin) {
            if (_value[col, row] == val) {
                return;
            }
            _possibilitySetCell[col, row] = 1 << val;
            _remainingPossibilityCount[col, row] = 1;
            _value[col, row] = val;
            if (CellChanged != null) {
                CellChanged(col, row, foundin);
            }
        }

        public void Eliminate(int col, int row, int i) {
            var mask = 1 << i;
            if ((_possibilitySetCell[col, row] & mask) == 0) {
                return;
            }
            _possibilitySetCell[col, row] = _possibilitySetCell[col, row] & ~mask;
            if (--_remainingPossibilityCount[col, row] == 1) {
                _value[col, row] = _possibilitySetCell[col, row].FirstHiBitPosition();
            }
            if (CellChanged != null) {
                CellChanged(col, row, null);
            }
        }
    }
}