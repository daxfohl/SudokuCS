using System;
using System.Collections.Generic;

namespace Sudoku.Types {
    /// <summary>
    /// Works like System.Collections.BitArray, but is invariate (for safety) and a struct,
    /// so should work more intuitively.  Also the .Equals() is overridden to be 
    /// by-value instead of by-reference.
    /// </summary>
    public struct BitArray32 {
        public static BitArray32 Zero = new BitArray32(0);
        readonly Int32 _int;

        public BitArray32(int i) {
            _int = i;
        }

        public bool this[int i] {
            get { return (_int & (1 << i)) != 0; }
        }

        public int HiBitCount {
            get {
                int x = _int;
                x = (x >> 1 & 0x55555555) + (x & 0x55555555);
                x = (x >> 2 & 0x33333333) + (x & 0x33333333);
                x = (x >> 4 & 0x0f0f0f0f) + (x & 0x0f0f0f0f);
                x = (x >> 8 & 0x00ff00ff) + (x & 0x00ff00ff);
                return (x >> 16) + (x & 0x0000ffff);
            }
        }

        public int FirstHiBitPosition {
            get {
                int i = (((_int & 0xaaaaaaaa) == 0) ? 0 : 1);
                i += (((_int & 0xcccccccc) == 0) ? 0 : 2);
                i += (((_int & 0xf0f0f0f0) == 0) ? 0 : 4);
                i += (((_int & 0xff00ff00) == 0) ? 0 : 8);
                return i + (((_int & 0xffff0000) == 0) ? 0 : 16);
            }
        }

        /// <summary>
        /// An iterator that gives you the positions of the high bits.
        /// </summary>
        public IEnumerable<int> HighBitPositions {
            get {
                for (int curr = 0; curr < 32; ++curr) {
                    if (((1 << curr) & _int) != 0) {
                        yield return curr;
                    }
                }
            }
        }

        public static BitArray32 CreateWithNthBitOn(int n) {
            return new BitArray32(1 << n);
        }

        public static BitArray32 CreateWithBottomNBitsOn(int n) {
            return new BitArray32((1 << n) - 1);
        }

        public BitArray32 And(BitArray32 other) {
            return other._int & _int;
        }

        public Int32 ToInt() {
            return _int;
        }

        public BitArray32 Or(BitArray32 other) {
            return other._int | _int;
        }

        public BitArray32 Minus(BitArray32 other) {
            return _int & (~(other._int));
        }

        public bool Contains(BitArray32 other) {
            return (other | this) == this;
        }

        public BitArray32 Not() {
            return ~_int;
        }

        public BitArray32 ReplaceBit(int i, bool b) {
            Int32 mask = 1 << i;
            return b ? (_int | mask) : (_int & (~mask));
        }

        public override bool Equals(object obj) {
            if (obj is BitArray32) {
                return _int == ((BitArray32)obj)._int;
            }
            return false;
        }

        public override int GetHashCode() {
            return _int;
        }

        public override string ToString() {
            return _int.ToString("x8");
        }

        public static implicit operator BitArray32(Int32 i) {
            return new BitArray32(i);
        }

        public static bool operator ==(BitArray32 b1, BitArray32 b2) {
            return b1._int == b2._int;
        }

        public static bool operator !=(BitArray32 b1, BitArray32 b2) {
            return b1._int != b2._int;
        }

        public static BitArray32 operator &(BitArray32 b1, BitArray32 b2) {
            return b1.And(b2);
        }

        public static BitArray32 operator |(BitArray32 b1, BitArray32 b2) {
            return b1.Or(b2);
        }

        public static BitArray32 operator +(BitArray32 b1, BitArray32 b2) {
            return b1.Or(b2);
        }

        public static BitArray32 operator -(BitArray32 b1, BitArray32 b2) {
            return b1.Minus(b2);
        }

        public static BitArray32 operator ~(BitArray32 b) {
            return b.Not();
        }

        public static bool operator >=(BitArray32 b1, BitArray32 b2) {
            return b1.Contains(b2);
        }

        public static bool operator <=(BitArray32 b1, BitArray32 b2) {
            return b2.Contains(b1);
        }

        public static bool operator >(BitArray32 b1, BitArray32 b2) {
            return (b1 >= b2) && (b1 != b2);
        }

        public static bool operator <(BitArray32 b1, BitArray32 b2) {
            return (b1 <= b2) && (b1 != b2);
        }
    }
}