using System.Collections.Generic;
using Sudoku.Types;

namespace Sudoku.Model {
    /// <summary>
    /// Represents a square within the model, with all the expected methods and properties.
    /// </summary>
    public class Square : Region {
        readonly int _sqrCol;
        readonly int _sqrRow;
        readonly int _startCol;
        readonly int _startRow;
        Cell[] _cells;
        Line[] _intersectingLines;

        public Square(SudokuModel model, int sqrCol, int sqrRow)
            : base(model) {
            _sqrCol = sqrCol;
            _sqrRow = sqrRow;
            _startCol = _sqrCol * Model.SizeSqrt;
            _startRow = _sqrRow * Model.SizeSqrt;
        }

        public Square(SudokuModel model, Square square)
            : base(model) {
            _sqrCol = square._sqrCol;
            _sqrRow = square._sqrRow;
            _startCol = square._startCol;
            _startRow = square._startRow;
        }

        #region Properties

        public int SquareCol {
            get { return _sqrCol; }
        }

        public int SquareRow {
            get { return _sqrRow; }
        }

        public int StartCol {
            get { return _startCol; }
        }

        public int StartRow {
            get { return _startRow; }
        }

        public override Cell[] Cells {
            get {
                if (_cells == null) {
                    _cells = new Cell[SideLength * SideLength];
                    for (int c = 0; c < SideLength; ++c) {
                        for (int r = 0; r < SideLength; ++r) {
                            _cells[c * SideLength +  r] = Model.Cells[c + _startCol, r + _startRow];
                        }
                    }
                }
                return _cells;
            }
        }

        public int SideLength {
            get { return Model.SizeSqrt; }
        }

        public IEnumerable<Line> IntersectingLines {
            get {
                if (_intersectingLines == null) {
                    _intersectingLines = new Line[SideLength * 2];
                    for (int i = 0; i < SideLength; ++i) {
                        _intersectingLines[i] = Model.Columns[StartCol + i];
                        _intersectingLines[i + SideLength] = Model.Rows[StartRow + i];
                    }
                }
                return _intersectingLines;
            }
        }

        public override bool Contains(Cell cell) {
            return cell.Square == this;
        }

        #endregion

        #region Grid

        /// <summary>
        /// A Grid of Squares, which also extends ICountable(IRegion)
        /// for compatability with functions that return that datatype.  (This compatability
        /// was required for collections in which Rows and Columns could also be included).
        /// </summary>
        public class Grid : Grid<Square>, IEnumerable<IRegion> {
            public Grid(Square[,] tGrid)
                : base(tGrid) { }

            public Square this[Cell cell] {
                get {
                    int squareCol = cell.ColumnIndex / SideLength;
                    int squareRow = cell.RowIndex / SideLength;
                    return this[squareCol, squareRow];
                }
            }

            IEnumerator<IRegion> IEnumerable<IRegion>.GetEnumerator() {
                foreach (var x in this) {
                    yield return x;
                }
            }
        }

        #endregion
    }
}