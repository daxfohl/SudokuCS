using System;
using System.IO;
using System.Windows.Forms;
using Sudoku.Model;
using Sudoku.Solving;

namespace Sudoku.View {
    public partial class Form1 : Form {
        SudokuModel _model;
        string _name;
        Solver _solver;

        public Form1() {
            InitializeComponent();
        }

        #region Loading a new model

        void OpenFile(string name) {
            _name = name;
            // Open and read the file
            try {
                TextReader reader = new FileInfo(name).OpenText();
                var text = reader.ReadToEnd();
                reader.Dispose();
                var lines = text.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

                // Create a new model and solver, and add event listeners
                if (_model != null) {
                    _model.ModelChanged -= HandleModelModelChanged;
                    _solver.ScanFinished -= HandleSolverScanFinished;
                    _solver.Finished -= HandleSolverFinished;
                }
                _model = new SudokuModel(lines.Length);
                _solver = new Solver(_model);
                _model.ModelChanged += HandleModelModelChanged;
                _solver.ScanFinished += HandleSolverScanFinished;
                _solver.Finished += HandleSolverFinished;

                // Update the screen
                _grid.Model = _model;
                _txtProgress.Clear();

                // Load data into the model
                for (var row = 0; row < lines.Length; ++row) {
                    var cells = lines[row].Split(',');
                    for (var col = 0; col < lines.Length; ++col) {
                        if (!string.IsNullOrEmpty(cells[col])) {
                            _model.SetValue(col, row, cells[col][0] - 'A');
                        }
                    }
                }
            } catch (FileNotFoundException) {}
        }

        #endregion

        #region Model and Solver event handlers

        void HandleModelModelChanged(int col, int row) {
            if (_chkUpdate.Checked) {
                UpdateCell(col, row);
                UpdateLabels();
            }
        }

        void UpdateCell(int col, int row) {
            _grid.UpdateCell(col, row);
        }

        void UpdateLabels() {
            var total = _model.SizeCubed - _model.SizeSquared;
            _lblSolved.Text = string.Format("Solved : {0}/{1} ({2:P2})", _model.SolvedCount, _model.SizeSquared, (double)_model.SolvedCount / _model.SizeSquared);
            _lblRemain.Text = string.Format("Eliminated : {0}/{1} ({2:P2})", _model.EliminatedCount, total, (double)_model.EliminatedCount / total);
        }

        void HandleSolverFinished() {
            AppendProgressLine("Finished in " + _solver.RunTime.TotalMilliseconds + " ms.");
        }

        void HandleSolverScanFinished(Strategy strategy) {
            if (_chkUpdate.Checked) {
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

        void Form1_OnLoad(object sender, EventArgs e) {
            OpenFile(@"..\..\..\sudoku.csv");
        }

        void mnuOpen_OnClick(object sender, EventArgs e) {
            var dlg = new OpenFileDialog {InitialDirectory = "C:\\", Filter = @"CSV|*.csv"};
            dlg.ShowDialog(this);
            OpenFile(dlg.FileName);
            dlg.Dispose();
        }

        void mnuSolve_OnClick(object sender, EventArgs e) {
            _solver.Solve();
        }

        void tmrScreenUpdate_OnTick(object sender, EventArgs e) {
            for (var row = 0; row < _model.Size; ++row) {
                for (var col = 0; col < _model.Size; ++col) {
                    UpdateCell(col, row);
                }
            }
            UpdateLabels();
        }


        void _btnSolve_OnClick(object sender, EventArgs e) {
            _solver.Solve();
        }

        private void _btnReload_OnClick(object sender, EventArgs e) {
            OpenFile(_name);
        }

        #endregion
    }
}