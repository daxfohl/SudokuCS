using System.Collections;
using System.Collections.Generic;

namespace Sudoku.Types {
    /// <summary>
    /// A lightweight List type, without some of the complexity of
    /// Systems.Collections.Generic.List.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LightweightList<T> : ICountable<T> {
        readonly IList<T> _tList;

        public LightweightList(IList<T> tList) {
            _tList = tList;
        }

        public int Count {
            get { return _tList.Count; }
        }

        public T this[int i] {
            get { return _tList[i]; }
        }

        public IEnumerator<T> GetEnumerator() {
            return _tList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}