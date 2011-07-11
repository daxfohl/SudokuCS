namespace Sudoku.Model {
    /// <summary>
    /// Represents a Row of the model
    /// </summary>
    public class Row : Line {
        public Row(SudokuModel model, int row) : base(model, row) {}
        public Row(SudokuModel model, Line row) : base(model, row) {}

        protected override Cell GetCell(int col) {
            return new Cell(col, Index);
        }

        protected override void LoadIntersectingSquares(Square[] squares) {
            int squareRow = Model.GetSquare(0, Index).StartRow;
            for (int i = 0; i < squares.Length; ++i) {
                squares[i] = Model.Squares[i, squareRow];
            }
        }

        public override bool Contains(Cell cell) {
            return cell.Row == Index;
        }
    }
}