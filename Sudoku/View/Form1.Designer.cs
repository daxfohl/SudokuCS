namespace Sudoku.View {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.actionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSolve = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._grid = new Sudoku.View.SudokuGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this._btnSolve = new System.Windows.Forms.Button();
            this._btnReload = new System.Windows.Forms.Button();
            this._txtProgress = new System.Windows.Forms.TextBox();
            this._lblSolved = new System.Windows.Forms.Label();
            this._lblRemain = new System.Windows.Forms.Label();
            this.tmrScreenUpdate = new System.Windows.Forms.Timer(this.components);
            this._chkUpdate = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.actionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1039, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpen});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // mnuOpen
            // 
            this.mnuOpen.Name = "mnuOpen";
            this.mnuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mnuOpen.Size = new System.Drawing.Size(146, 22);
            this.mnuOpen.Text = "&Open";
            this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
            // 
            // actionToolStripMenuItem
            // 
            this.actionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSolve});
            this.actionToolStripMenuItem.Name = "actionToolStripMenuItem";
            this.actionToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.actionToolStripMenuItem.Text = "&Action";
            // 
            // mnuSolve
            // 
            this.mnuSolve.Name = "mnuSolve";
            this.mnuSolve.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.mnuSolve.Size = new System.Drawing.Size(121, 22);
            this.mnuSolve.Text = "&Solve";
            this.mnuSolve.Click += new System.EventHandler(this.mnuSolve_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1039, 592);
            this.panel1.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._grid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Size = new System.Drawing.Size(1039, 592);
            this.splitContainer1.SplitterDistance = 720;
            this.splitContainer1.TabIndex = 3;
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToResizeColumns = false;
            this._grid.AllowUserToResizeRows = false;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._grid.DefaultCellStyle = dataGridViewCellStyle1;
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._grid.Location = new System.Drawing.Point(0, 0);
            this._grid.Name = "_grid";
            this._grid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._grid.Size = new System.Drawing.Size(720, 592);
            this._grid.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this._chkUpdate);
            this.panel2.Controls.Add(this._btnSolve);
            this.panel2.Controls.Add(this._btnReload);
            this.panel2.Controls.Add(this._txtProgress);
            this.panel2.Controls.Add(this._lblSolved);
            this.panel2.Controls.Add(this._lblRemain);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(315, 592);
            this.panel2.TabIndex = 0;
            // 
            // _btnSolve
            // 
            this._btnSolve.Location = new System.Drawing.Point(146, 14);
            this._btnSolve.Name = "_btnSolve";
            this._btnSolve.Size = new System.Drawing.Size(75, 23);
            this._btnSolve.TabIndex = 4;
            this._btnSolve.Text = "Solve";
            this._btnSolve.UseVisualStyleBackColor = true;
            this._btnSolve.Click += new System.EventHandler(this._btnSolve_Click);
            // 
            // _btnReload
            // 
            this._btnReload.Location = new System.Drawing.Point(48, 14);
            this._btnReload.Name = "_btnReload";
            this._btnReload.Size = new System.Drawing.Size(75, 23);
            this._btnReload.TabIndex = 3;
            this._btnReload.Text = "Reload";
            this._btnReload.UseVisualStyleBackColor = true;
            this._btnReload.Click += new System.EventHandler(this._btnReload_Click);
            // 
            // _txtProgress
            // 
            this._txtProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtProgress.Location = new System.Drawing.Point(42, 109);
            this._txtProgress.Multiline = true;
            this._txtProgress.Name = "_txtProgress";
            this._txtProgress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtProgress.Size = new System.Drawing.Size(244, 453);
            this._txtProgress.TabIndex = 2;
            // 
            // _lblSolved
            // 
            this._lblSolved.AutoSize = true;
            this._lblSolved.Location = new System.Drawing.Point(45, 51);
            this._lblSolved.Name = "_lblSolved";
            this._lblSolved.Size = new System.Drawing.Size(0, 13);
            this._lblSolved.TabIndex = 1;
            // 
            // _lblRemain
            // 
            this._lblRemain.AutoSize = true;
            this._lblRemain.Location = new System.Drawing.Point(45, 77);
            this._lblRemain.Name = "_lblRemain";
            this._lblRemain.Size = new System.Drawing.Size(0, 13);
            this._lblRemain.TabIndex = 0;
            // 
            // tmrScreenUpdate
            // 
            this.tmrScreenUpdate.Tick += new System.EventHandler(this.tmrScreenUpdate_Tick);
            // 
            // _chkUpdate
            // 
            this._chkUpdate.AutoSize = true;
            this._chkUpdate.Checked = true;
            this._chkUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkUpdate.Location = new System.Drawing.Point(227, 18);
            this._chkUpdate.Name = "_chkUpdate";
            this._chkUpdate.Size = new System.Drawing.Size(61, 17);
            this._chkUpdate.TabIndex = 5;
            this._chkUpdate.Text = "Update";
            this._chkUpdate.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1039, 616);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Sudoku";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuOpen;
        private System.Windows.Forms.ToolStripMenuItem actionToolStripMenuItem;
        private SudokuGridView _grid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label _lblSolved;
        private System.Windows.Forms.Label _lblRemain;
        private System.Windows.Forms.ToolStripMenuItem mnuSolve;
        private System.Windows.Forms.TextBox _txtProgress;
        private System.Windows.Forms.Timer tmrScreenUpdate;
        private System.Windows.Forms.Button _btnSolve;
        private System.Windows.Forms.Button _btnReload;
        private System.Windows.Forms.CheckBox _chkUpdate;
    }
}

