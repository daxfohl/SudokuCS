using System.Drawing;
using System.Windows.Forms;
using Sudoku.Model;

namespace Sudoku.View {
    class SudokuGridView : DataGridView {
        /// <summary>
        ///   Adds new columns and rows to correspond to the model
        /// </summary>
        public SudokuModel Model {
            set {
                int len = value.Size;
                Rows.Clear();
                Columns.Clear();
                for (int i = 0; i < len; ++i) {
                    Columns.Add(i.ToString(), i.ToString());
                }
                Rows.Add(len);
                for (int i = 0; i < len; ++i) {
                    Rows[i].HeaderCell.Value = i.ToString();
                    Columns[i].Width = Rows[0].Height;
                    for (int j = 0; j < len; ++j) {
                        var square = value.Cells[i, j].Square;
                        this[i, j].Style.BackColor =
                            ((square.SquareCol + square.SquareRow) % 2) == 0 ? Color.LightBlue : Color.LightYellow;
                    }
                }
            }
        }

        /// <summary>
        ///   Updates the cell in the view to correspond to the model
        /// </summary>
        /// <param name = "cell"></param>
        public void UpdateCell(Cell cell) {
            int col = cell.ColumnIndex;
            int row = cell.RowIndex;
            if (cell.IsSolved) {
                this[col, row].Style.ForeColor = Color.Black;
                this[col, row].Value = (char)(cell.Value + 'A');
            } else {
                this[col, row].Value = cell.RemainingPossibilityCount;
            }
        }
    }
}