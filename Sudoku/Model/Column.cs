namespace Sudoku.Model {
    /// <summary>
    /// Represents a Column of the model
    /// </summary>
    public class Column : Line {
        public Column(SudokuModel model, int col) : base(model, col) {}
        public Column(SudokuModel model, Line col) : base(model, col) {}

        protected override Cell GetCell(SudokuModel model, int i) {
            return model.Cells[Index, i];
        }

        protected override void LoadIntersectingSquares(Square[] squares) {
            int squareCol = Cells[0].Square.SquareCol;
            for (int i = 0; i < squares.Length; ++i) {
                squares[i] = Model.Squares[squareCol, i];
            }
        }

        public override bool Contains(Cell cell) {
            return cell.Column == this;
        }
    }
}