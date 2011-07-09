using System.Collections.Generic;
using Sudoku.Types;

namespace Sudoku.Model {
    /// <summary>
    /// The base class for Row and Column, with the expected properties and some abstract 
    /// methods that allow good code reuse.
    /// </summary>
    public abstract class Line : Region {
        readonly int _index;
        LightweightList<Cell> _cells;
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

        public int Index {
            get { return _index; }
        }

        public int Length {
            get { return Model.Size; }
        }

        public override ICountable<Cell> Cells {
            get {
                if (_cells == null) {
                    var cells = new Cell[Length];
                    for (int i = 0; i < Length; ++i) {
                        cells[i] = GetCell(Model, i);
                    }
                    _cells = new LightweightList<Cell>(cells);
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

        protected abstract Cell GetCell(SudokuModel model, int i);
        protected abstract void LoadIntersectingSquares(Square[] squares);

        #endregion

        #region LineList class

        /// <summary>
        /// A LightweightList of either Rows or Columns, which also extends ICountable(IRegion)
        /// for compatability with functions that return that datatype.  (This compatability
        /// was required for collections in which Squares could also be included).
        /// </summary>
        /// <typeparam name="T">either Row or Column</typeparam>
        public class LineList<T> : LightweightList<T>, ICountable<IRegion> where T : Line {
            public LineList(IList<T> tList)
                : base(tList) {}

            int ICountable<IRegion>.Count {
                get { return Count; }
            }

            IRegion ICountable<IRegion>.this[int i] {
                get { return this[i]; }
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