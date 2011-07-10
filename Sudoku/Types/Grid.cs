namespace Sudoku.Types {
    public static class Grid {
        public static T Get<T>(this T[,] tGrid, int i) {
            return tGrid[i / tGrid.GetLength(0), i % tGrid.GetLength(0)];
        }
    }
}