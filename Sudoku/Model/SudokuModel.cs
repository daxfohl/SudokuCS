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
        readonly Cell[,][] _cellsRegion;
        readonly Region[,][] _intersectingRegions;
        readonly int[,] _lastChangedIterCell;
        readonly int[,] _lastChangedIterRegion;
        readonly int[,] _possibilitySetCell;
        readonly int _possibilitySetModel;
        readonly int[,] _remainingPossibilityCount;
        readonly int _size;
        readonly int _sizeCubed;
        readonly int _sizeSqrt;
        readonly int _sizeSquared;
        readonly int[,] _solved;
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
            _possibilitySetCell = new int[Size, Size];
            _remainingPossibilityCount = new int[Size, Size];
            _value = new int[Size, Size];
            _lastChangedIterCell = new int[Size, Size];

            // Initialize the Cell objects, and the cache values
            for (var col = 0; col < size; ++col) {
                for (var row = 0; row < size; ++row) {
                    _possibilitySetCell[col, row] = PossibilitySetModel;
                    _remainingPossibilityCount[col, row] = Size;
                    _value[col, row] = -1;
                    _lastChangedIterCell[col, row] = -1;
                    CellChanged = null;
                }
            }
            CellChanged += HandleCellChanged;

            _solved = new int[3, _size];
            _lastChangedIterRegion = new int[3, _size];
            _cellsRegion = new Cell[3, _size][];
            _intersectingRegions = new Region[Size, Size][];
            for (var i = 0; i < _size; ++i) {
                _lastChangedIterRegion[0, i] = -1;
                _lastChangedIterRegion[1, i] = -1;
                _lastChangedIterRegion[2, i] = -1;
                var colCells = new Cell[_size];
                for (var j = 0; j < _size; ++j) {
                    colCells[j] = new Cell(i, j);
                }
                _cellsRegion[(int)RegionType.Column, i] = colCells;
                var rowCells = new Cell[_size];
                for (var j = 0; j < _size; ++j) {
                    rowCells[j] = new Cell(j, i);
                }
                _cellsRegion[(int)RegionType.Row, i] = rowCells;
                var sqCells = new Cell[_size];
                var sqrCol = i % _sizeSqrt;
                var sqrRow = i / _sizeSqrt;
                var startCol = sqrCol * _sizeSqrt;
                var startRow = sqrRow * _sizeSqrt;
                for (var c = 0; c < _sizeSqrt; ++c) {
                    for (var r = 0; r < _sizeSqrt; ++r) {
                        sqCells[r * _sizeSqrt + c] = new Cell(c + startCol, r + startRow);
                    }
                }
                _cellsRegion[(int)RegionType.Square, i] = sqCells;
            }
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
            _possibilitySetCell = new int[Size, Size];
            _remainingPossibilityCount = new int[Size, Size];
            _value = new int[Size, Size];
            _lastChangedIterCell = new int[Size, Size];

            // Initialize the Cell objects, and the cache values
            for (var col = 0; col < size; ++col) {
                for (var row = 0; row < size; ++row) {
                    _possibilitySetCell[col, row] = model.GetPossibilitySetCell(col, row);
                    _remainingPossibilityCount[col, row] = model.GetRemainingPossibilityCount(col, row);
                    _value[col, row] = model.GetValue(col, row);
                    _lastChangedIterCell[col, row] = model.GetLastChangedIterCell(col, row);
                    CellChanged = null;
                }
            }
            CellChanged += HandleCellChanged;

            _solved = new int[3, _size];
            _lastChangedIterRegion = new int[3, _size];
            _cellsRegion = new Cell[3, _size][];
            _intersectingRegions = new Region[Size, Size][];
            for (var i = 0; i < _size; ++i) {
                _lastChangedIterRegion[0, i] = model._lastChangedIterRegion[0, i];
                _lastChangedIterRegion[1, i] = model._lastChangedIterRegion[0, i];
                _lastChangedIterRegion[2, i] = model._lastChangedIterRegion[0, i];
                SetLastChangedIterRegion(RegionType.Column, i, -1);
                SetLastChangedIterRegion(RegionType.Row, i, -1);
                SetLastChangedIterRegion(RegionType.Square, i, -1);
                _solved[0, i] = model._solved[0, i];
                _solved[1, i] = model._solved[1, i];
                _solved[2, i] = model._solved[2, i];
                var colCells = new Cell[_size];
                for (var j = 0; j < _size; ++j) {
                    colCells[j] = new Cell(i, j);
                }
                _cellsRegion[(int)RegionType.Column, i] = colCells;
                var rowCells = new Cell[_size];
                for (var j = 0; j < _size; ++j) {
                    rowCells[j] = new Cell(j, i);
                }
                _cellsRegion[(int)RegionType.Row, i] = rowCells;
                var sqCells = new Cell[_size];
                var sqrCol = i % _sizeSqrt;
                var sqrRow = i / _sizeSqrt;
                var startCol = sqrCol * _sizeSqrt;
                var startRow = sqrRow * _sizeSqrt;
                for (var c = 0; c < _sizeSqrt; ++c) {
                    for (var r = 0; r < _sizeSqrt; ++r) {
                        sqCells[r * _sizeSqrt + c] = new Cell(c + startCol, r + startRow);
                    }
                }
                _cellsRegion[(int)RegionType.Square, i] = sqCells;
            }
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
            for (var type = 0; type < 3; ++type) {
                for (var i = 0; i < _size; ++i) {
                    var cells = GetCells((RegionType)type, i);
                    var totalSet = cells.Aggregate(0, (current, cell) => current | _possibilitySetCell[cell.Column, cell.Row]);
                    if (totalSet != _possibilitySetModel) {
                        return false;
                    }
                }
            }
            return true;
        }

        public Cell[] GetCells(RegionType type, int i) {
            return _cellsRegion[(int)type, i];
        }

        public void SetLastChangedIterRegion(RegionType type, int i, int value) {
            _lastChangedIterRegion[(int)type, i] = value;
        }

        public int GetLastChangedIterRegion(RegionType type, int i) {
            return _lastChangedIterRegion[(int)type, i];
        }

        public IEnumerable<int> GetUnsolvedValues(RegionType type, int i) {
            var solved = _solved[(int)type, i];
            for (var j = 0; j < _size; ++j) {
                if ((solved & (1 << j)) == 0) {
                    yield return j;
                }
            }
        }

        public bool Contains(RegionType type, int i, Cell cell) {
            switch (type) {
                case RegionType.Column:
                    return cell.Column == i;
                case RegionType.Row:
                    return cell.Row == i;
                case RegionType.Square:
                    var sqrCol = i % _sizeSqrt;
                    var sqrRow = i / _sizeSqrt;
                    var startCol = sqrCol * _sizeSqrt;
                    var startRow = sqrRow * _sizeSqrt;
                    return (cell.Column >= startCol && cell.Column < startCol + _sizeSqrt &&
                            cell.Row >= startRow && cell.Row < startRow + _sizeSqrt);
            }
            throw new InvalidOperationException();
        }

        public void SetValueSolved(RegionType type, int i, int value) {
            _solved[(int)type, i] |= (1 << value);
        }

        public event Action<int, int> ModelChanged;


        /// <summary>
        ///   We update our cache variables and bubble up the event.
        /// </summary>
        void HandleCellChanged(int col, int row, RegionType? foundInType, int foundInI) {
            EliminatedCount += 1;
            LastChangedCell = new Cell(col, row);
            var value = _value[col, row];
            foreach (var region in GetIntersectingRegions(col, row)) {
                SetLastChangedIterRegion(region.Type, region.I, ChangeCount);
                if (value != -1) {
                    SetValueSolved(region.Type, region.I, value);
                }
            }
            ++ChangeCount;
            if (value != -1) {
                ++SolvedCount;
                if (foundInType.HasValue) {
                    DuplicateElimination.EliminateDuplicates(this, col, row, foundInType.Value, foundInI);
                } else {
                    DuplicateElimination.EliminateDuplicates(this, col, row);
                }
            }
            if (ModelChanged != null) {
                ModelChanged(col, row);
            }
        }


        public event Action<int, int, RegionType?, int> CellChanged;


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
                CellChanged(col, row, null, 0);
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
                CellChanged(col, row, null, 0);
            }
        }

        /// <summary>
        ///   Returns the Row, Column, and Square that contain that cell.
        /// </summary>
        public IEnumerable<Region> GetIntersectingRegions(int col, int row) {
            if (_intersectingRegions[col, row] == null) {
                _intersectingRegions[col, row] = new Region[3];
                _intersectingRegions[col, row][0] = new Region(RegionType.Column, col);
                _intersectingRegions[col, row][1] = new Region(RegionType.Row, row);
                int sqI = (row / _sizeSqrt) * _sizeSqrt + col / _sizeSqrt;
                _intersectingRegions[col, row][2] = new Region(RegionType.Square, sqI);
            }
            return _intersectingRegions[col, row];
        }

        public void SetLastChangedIterCell(int col, int row, int value) {
            _lastChangedIterCell[col, row] = value;
        }

        public int GetLastChangedIterCell(int col, int row) {
            return _lastChangedIterCell[col, row];
        }

        public void SetValueOptimized(int col, int row, int val, RegionType foundinType, int foundinI) {
            if (_value[col, row] == val) {
                return;
            }
            _possibilitySetCell[col, row] = 1 << val;
            _remainingPossibilityCount[col, row] = 1;
            _value[col, row] = val;
            if (CellChanged != null) {
                CellChanged(col, row, foundinType, foundinI);
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
                CellChanged(col, row, null, 0);
            }
        }
    }
}