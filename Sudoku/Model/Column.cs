namespace Sudoku.Model {
    /// <summary>
    /// Represents a Column of the model
    /// </summary>
    public class Column : Line {
        public Column(SudokuModel model, int col) : base(model, col) {}
        public Column(SudokuModel model, Line col) : base(model, col) {}

        protected override Cell GetCell(int row) {
            return new Cell(Index, row);
        }

        protected override void LoadIntersectingSquares(Square[] squares) {
            int squareCol = Model.GetSquare(Index, 0).StartCol;
            for (int i = 0; i < squares.Length; ++i) {
                squares[i] = Model.Squares[squareCol, i];
            }
        }

        public override bool Contains(Cell cell) {
            return cell.Column == Index;
        }
    }
}