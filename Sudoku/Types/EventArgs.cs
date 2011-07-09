using System;

namespace Sudoku.Types {
    public class EventArgs<T> : EventArgs {
        public T Value;

        public EventArgs(T t) {
            Value = t;
        }

        public static implicit operator EventArgs<T>(T t) {
            return new EventArgs<T>(t);
        }
    }
}