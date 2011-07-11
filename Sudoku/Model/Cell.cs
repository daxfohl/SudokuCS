namespace Sudoku.Model {
    /// <summary>
    ///   Represents a single cell in the model
    /// </summary>
    public struct Cell {
        public readonly int Column;
        public readonly int Row;

        public Cell(int column, int row) {
            Column = column;
            Row = row;
        }
    }
}