using System.Drawing;
using System.Windows.Forms;
using Sudoku.Model;

namespace Sudoku.View {
    class SudokuGridView : DataGridView {
        SudokuModel _model;

        /// <summary>
        ///   Adds new columns and rows to correspond to the model
        /// </summary>
        public SudokuModel Model {
            set {
                _model = value;
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
                        var square = value.GetSquare(i, j);
                        this[i, j].Style.BackColor =
                            ((square.SquareCol + square.SquareRow) % 2) == 0 ? Color.LightBlue : Color.LightYellow;
                    }
                }
            }
        }

        /// <summary>
        ///   Updates the cell in the view to correspond to the model
        /// </summary>
        public void UpdateCell(int col, int row) {
            if (_model.IsSolved(col, row)) {
                this[col, row].Style.ForeColor = Color.Black;
                this[col, row].Value = (char)(_model.GetValue(col, row) + 'A');
            } else {
                this[col, row].Value = _model.GetRemainingPossibilityCount(col, row);
            }
        }
    }
}