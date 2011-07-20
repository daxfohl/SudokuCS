using System.Drawing;
using System.Windows.Forms;
using Sudoku.Model;

namespace Sudoku.View {
    sealed class SudokuGridView : DataGridView {
        SudokuModel _model;
        public SudokuGridView() {
            DoubleBuffered = true;
        }

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
                for (int row = 0; row < len; ++row) {
                    Rows[row].HeaderCell.Value = row.ToString();
                    Columns[row].Width = Rows[0].Height;
                    for (int col = 0; col < len; ++col) {
                        this[row, col].Style.BackColor =
                            ((col / _model.SizeSqrt + row / _model.SizeSqrt) % 2) == 0 ? Color.LightBlue : Color.LightYellow;
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