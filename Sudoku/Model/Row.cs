namespace Sudoku.Model {
    /// <summary>
    /// Represents a Row of the model
    /// </summary>
    public class Row : Line {
        public Row(SudokuModel model, int row) : base(model, row) {}
        public Row(SudokuModel model, Line row) : base(model, row) {}

        protected override Cell GetCell(SudokuModel model, int i) {
            return model.Cells[i, Index];
        }

        protected override void LoadIntersectingSquares(Square[] squares) {
            int squareRow = Cells[0].Square.SquareRow;
            for (int i = 0; i < squares.Length; ++i) {
                squares[i] = Model.Squares[i, squareRow];
            }
        }

        public override bool Contains(Cell cell) {
            return cell.Row == this;
        }
    }
}