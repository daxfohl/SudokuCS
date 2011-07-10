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
        #region Members

        readonly List<IRegion> _allRegions;
        readonly Cell[,] _cells;
        readonly Column[] _cols;
        readonly int _possibilitySet;
        readonly Row[] _rows;
        readonly int _size;
        readonly int _sizeCubed;
        readonly int _sizeSqrt;
        readonly int _sizeSquared;
        readonly Square[,] _squares;

        #endregion

        #region Constructors

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
            _possibilitySet = (1 << _size) - 1;

            // Initialize the Cell objects, and the cache values
            _cells = new Cell[size, size];
            for (int col = 0; col < size; ++col) {
                for (int row = 0; row < size; ++row) {
                    _cells[col, row] = new Cell(this, col, row);
                    _cells[col, row].Changed += HandleCellChanged;
                }
            }

            // Initialize the Row and Column objects
            _rows = new Row[size];
            _cols = new Column[size];
            for (int i = 0; i < size; ++i) {
                _rows[i] = new Row(this, i);
                _cols[i] = new Column(this, i);
            }

            // Initialize the Square objects
            _squares = new Square[_sizeSqrt, _sizeSqrt];
            for (int sqrCol = 0; sqrCol < _sizeSqrt; ++sqrCol) {
                for (int sqrRow = 0; sqrRow < _sizeSqrt; ++sqrRow) {
                    _squares[sqrCol, sqrRow] = new Square(this, sqrCol, sqrRow);
                }
            }

            // Finally initialize the Regions object
            _allRegions = new List<IRegion>(_cols);
            _allRegions.AddRange(_rows);
            _allRegions.AddRange(_squares.Cast<Square>());
        }

        /// <summary>
        ///   Copy constructor
        /// </summary>
        /// <param name = "model"></param>
        public SudokuModel(SudokuModel model)
            : this(model.Size) {
            // Initialize our variables
            int size = model.Size;
            _size = size;
            _sizeSqrt = Convert.ToInt32(Math.Sqrt(size));
            _sizeSquared = size * size;
            _sizeCubed = _sizeSquared * size;
            _possibilitySet = (1 << _size) - 1;

            // Initialize the Cell objects, and the cache values
            _cells = new Cell[size, size];
            for (int col = 0; col < size; ++col) {
                for (int row = 0; row < size; ++row) {
                    _cells[col, row] = new Cell(this, model.Cells[col, row]);
                    _cells[col, row].Changed += HandleCellChanged;
                }
            }

            // Initialize the Row and Column objects
            _rows = new Row[size];
            _cols = new Column[size];
            for (int i = 0; i < size; ++i) {
                _rows[i] = new Row(this, model.Rows[i]);
                _cols[i] = new Column(this, model.Columns[i]);
            }

            // Initialize the Square objects
            _squares = new Square[_sizeSqrt, _sizeSqrt];
            for (int sqrCol = 0; sqrCol < _sizeSqrt; ++sqrCol) {
                for (int sqrRow = 0; sqrRow < _sizeSqrt; ++sqrRow) {
                    _squares[sqrCol, sqrRow] = new Square(this, model.Squares[sqrCol, sqrRow]);
                }
            }

            // Finally initialize the Regions object
            _allRegions = new List<IRegion>(_cols);
            _allRegions.AddRange(_rows);
            _allRegions.AddRange(_squares.Cast<Square>());

            SolvedCount = model.SolvedCount;
            EliminatedCount = model.EliminatedCount;
            ChangeCount = model.ChangeCount;
            LastChangedCell = _cells[model.LastChangedCell.ColumnIndex, model.LastChangedCell.RowIndex];
        }

        #endregion

        #region Events

        public event Action<Cell> Changed;

        #endregion

        #region Public Properties and Methods

        /// <summary>
        ///   Length of a side of the puzzle (or also the number of numbers available).
        /// </summary>
        public int Size {
            get { return _size; }
        }

        /// <summary>
        ///   Length of a side of a square in the puzzle, 
        ///   or the number of squares on a side of a puzzle.
        /// </summary>
        public int SizeSqrt {
            get { return _sizeSqrt; }
        }

        /// <summary>
        ///   Number of cells in the puzzle.
        /// </summary>
        public int SizeSquared {
            get { return _sizeSquared; }
        }

        public int SizeCubed {
            get { return _sizeCubed; }
        }

        public Row[] Rows {
            get { return _rows; }
        }

        public Column[] Columns {
            get { return _cols; }
        }

        public Square GetSquare(Cell cell) {
            int squareCol = cell.ColumnIndex / _sizeSqrt;
            int squareRow = cell.RowIndex / _sizeSqrt;
            return _squares[squareCol, squareRow];
        }

        public Square[,] Squares {
            get { return _squares; }
        }

        public IEnumerable<IRegion> AllRegions {
            get { return _allRegions; }
        }

        public Cell[,] Cells {
            get { return _cells; }
        }

        public int SolvedCount { get; private set; }

        public int EliminatedCount { get; private set; }

        public int PossibilitySet {
            get { return _possibilitySet; }
        }

        public Cell LastChangedCell { get; private set; }

        public int ChangeCount { get; private set; }

        /// <summary>
        ///   Check for any cell that has no remaining possibilities, or any region that has no
        ///   cells that has a certain number.
        /// </summary>
        /// <returns>true if valid</returns>
        public bool IsConsistent {
            get {
                return !_cells.Cast<Cell>().Any(cell => cell.PossibilitySet == 0) &&
                       _allRegions.Select(region => region.Cells.Aggregate(0, (current, cell) => current | cell.PossibilitySet))
                           .All(foundbits => foundbits == _possibilitySet);
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        ///   We update our cache variables and bubble up the event.
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        void HandleCellChanged(object sender, CellChangedEventArgs e) {
            EliminatedCount += e.NumEliminated;
            LastChangedCell = e.Cell;
            int value = LastChangedCell.Value;
            foreach (var region in LastChangedCell.IntersectingRegions) {
                region.LastChangedIteration = ChangeCount;
                if (value != -1) {
                    region.SetValueSolved(value);
                }
            }
            ++ChangeCount;
            if (value != -1) {
                ++SolvedCount;
                DuplicateElimination.EliminateDuplicates(LastChangedCell, e.Foundin);
            }
            if (Changed != null) {
                Changed(e.Cell);
            }
        }

        #endregion
    }
}