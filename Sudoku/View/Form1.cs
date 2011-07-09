using System;
using System.IO;
using System.Windows.Forms;
using Sudoku.Model;
using Sudoku.Solving;
using Sudoku.Types;

namespace Sudoku.View {
    public partial class Form1 : Form {
        SudokuModel _model;
        Solver _solver;
        string _name;

        public Form1() {
            InitializeComponent();
        }

        #region Loading a new model

        void OpenFile(string name) {
            _name = name;
            // Open and read the file
            try {
                TextReader reader = new FileInfo(name).OpenText();
                string text = reader.ReadToEnd();
                reader.Dispose();
                var lines = text.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

                // Create a new model and solver, and add event listeners
                if (_model != null) {
                    _model.Changed -= HandleModelChanged;
                    _solver.ScanFinished -= HandleSolverScanFinished;
                    _solver.Finished -= HandleSolverFinished;
                }
                _model = new SudokuModel(lines.Length);
                _solver = new Solver(_model);
                _model.Changed += HandleModelChanged;
                _solver.ScanFinished += HandleSolverScanFinished;
                _solver.Finished += HandleSolverFinished;

                // Update the screen
                _grid.Model = _model;
                _txtProgress.Clear();

                // Load data into the model
                for (int row = 0; row < lines.Length; ++row) {
                    var cells = lines[row].Split(',');
                    for (int col = 0; col < lines.Length; ++col) if (!string.IsNullOrEmpty(cells[col])) _model.Cells[col, row].Value = cells[col][0] - 'A';
                }
            } catch (FileNotFoundException) {}
        }

        #endregion

        #region Model and Solver event handlers

        void HandleModelChanged(object sender, EventArgs<Cell> e) {
            if (_chkUpdate.Checked) {
                UpdateCell(e.Value);
                UpdateLabels();
            }
        }

        void UpdateCell(Cell cell) {
            _grid.UpdateCell(cell);
        }

        void UpdateLabels() {
            int total = _model.SizeCubed - _model.SizeSquared;
            _lblSolved.Text = string.Format("Solved : {0}/{1} ({2:P2})", _model.SolvedCount, _model.SizeSquared, (double)_model.SolvedCount / _model.SizeSquared);
            _lblRemain.Text = string.Format("Eliminated : {0}/{1} ({2:P2})", _model.EliminatedCount, total, (double)_model.EliminatedCount / total);
        }

        void HandleSolverFinished(object sender, EventArgs e) {
            AppendProgressLine("Finished in " + _solver.RunTime.TotalMilliseconds + " ms.");
        }

        void HandleSolverScanFinished(object sender, EventArgs<Strategy> e) {
            if (_chkUpdate.Checked) {
                var strategy = e.Value;
                AppendProgressLine(string.Format("{0}:\t {1}, {2}\t({3}, {4})",
                                                 strategy.GetType().Name,
                                                 strategy.EliminatedLastRun,
                                                 Math.Round(strategy.TimeLastRun.TotalMilliseconds),
                                                 strategy.TotalEliminated,
                                                 strategy.TotalTime.TotalMilliseconds));
            }
        }

        void AppendProgressLine(string s) {
            _txtProgress.AppendText(s + "\r\n");
        }

        #endregion

        #region GUI event handlers

        void Form1_Load(object sender, EventArgs e) {
            OpenFile(@"..\..\..\sudoku.csv");
        }

        void mnuOpen_Click(object sender, EventArgs e) {
            var dlg = new OpenFileDialog {InitialDirectory = "C:\\", Filter = @"CSV|*.csv"};
            dlg.ShowDialog(this);
            OpenFile(dlg.FileName);
            dlg.Dispose();
        }

        void mnuSolve_Click(object sender, EventArgs e) {
            _solver.Solve();
        }

        void tmrScreenUpdate_Tick(object sender, EventArgs e) {
            foreach (var cell in _model.Cells) UpdateCell(cell);
            UpdateLabels();
        }


         void _btnSolve_Click(object sender, EventArgs e) {
            _solver.Solve();
        }

         private void _btnReload_Click(object sender, EventArgs e) {
             OpenFile(_name);
         }
        #endregion
    }
}