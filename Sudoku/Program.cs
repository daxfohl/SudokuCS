using System;
using System.Windows.Forms;
using Sudoku.View;

namespace Sudoku {
    static class Program {
        /// <summary>
        ///   The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}