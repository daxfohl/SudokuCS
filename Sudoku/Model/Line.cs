using System.Collections.Generic;

namespace Sudoku.Model {
    /// <summary>
    ///   The base class for Row and Column, with the expected properties and some abstract 
    ///   methods that allow good code reuse.
    /// </summary>
    public abstract class Line : Region {
        readonly int _index;
        Cell[] _cells;
        Square[] _intersectingSquares;

        protected Line(SudokuModel model, int index)
            : base(model) {
            _index = index;
        }

        protected Line(SudokuModel model, Line line)
            : base(model) {
            _index = line._index;
        }

        #region Properties

        public int Index { get { return _index; } }

        public int Length { get { return Model.Size; } }

        public override Cell[] Cells {
            get {
                if (_cells == null) {
                    _cells = new Cell[Length];
                    for (var i = 0; i < Length; ++i) {
                        _cells[i] = GetCell(i);
                    }
                }
                return _cells;
            }
        }

        public IEnumerable<Square> IntersectingSquares {
            get {
                if (_intersectingSquares == null) {
                    _intersectingSquares = new Square[Model.SizeSqrt];
                    LoadIntersectingSquares(_intersectingSquares);
                }
                return _intersectingSquares;
            }
        }

        #endregion

        #region Abstract methods

        protected abstract Cell GetCell(int row);
        protected abstract void LoadIntersectingSquares(Square[] squares);

        #endregion
    }
}