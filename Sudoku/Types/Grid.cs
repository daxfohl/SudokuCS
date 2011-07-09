using System.Collections;
using System.Collections.Generic;

namespace Sudoku.Types {
    /// <summary>
    ///   A generic two-dimensional square array
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    public class Grid<T> : IEnumerable<T> where T : class {
        readonly T[,] _tGrid;

        public Grid(T[,] tGrid) {
            _tGrid = tGrid;
        }

        public T this[int col, int row] {
            get { return _tGrid[col, row]; }
        }

        public T this[int i] {
            get { return _tGrid[i / SideLength, i % SideLength]; }
        }

        public int SideLength {
            get { return _tGrid.GetLength(0); }
        }

        public int Count {
            get { return _tGrid.Length; }
        }

        public IEnumerator<T> GetEnumerator() {
            foreach (var t in _tGrid) {
                yield return t;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}