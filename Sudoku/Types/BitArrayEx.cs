using System.Collections.Generic;

namespace Sudoku.Types {
    /// <summary>
    ///   Works like System.Collections.BitArray, but is invariate (for safety) and a struct,
    ///   so should work more intuitively.  Also the .Equals() is overridden to be 
    ///   by-value instead of by-reference.
    /// </summary>
    public static class BitArrayEx {
        public static int HiBitCount(this int ii) {
            ii = (ii >> 1 & 0x55555555) + (ii & 0x55555555);
            ii = (ii >> 2 & 0x33333333) + (ii & 0x33333333);
            ii = (ii >> 4 & 0x0f0f0f0f) + (ii & 0x0f0f0f0f);
            ii = (ii >> 8 & 0x00ff00ff) + (ii & 0x00ff00ff);
            return (ii >> 16) + (ii & 0x0000ffff);
        }

        public static int FirstHiBitPosition(this int ii) {
            var i = (((ii & 0xaaaaaaaa) == 0) ? 0 : 1);
            i += (((ii & 0xcccccccc) == 0) ? 0 : 2);
            i += (((ii & 0xf0f0f0f0) == 0) ? 0 : 4);
            i += (((ii & 0xff00ff00) == 0) ? 0 : 8);
            return i + (((ii & 0xffff0000) == 0) ? 0 : 16);
        }

        /// <summary>
        ///   An iterator that gives you the positions of the high bits.
        /// </summary>
        public static IEnumerable<int> HighBitPositions(this int ii) {
            for (var curr = 0; curr < 32; ++curr) {
                if (((1 << curr) & ii) != 0) {
                    yield return curr;
                }
            }
        }
    }
}