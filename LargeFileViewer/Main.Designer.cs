namespace LargeFileViewer
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.centerPanel = new System.Windows.Forms.Panel();
            this.fileViewer = new LargeFileViewer.FileViewer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripLoadStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripFileSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripLine = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeFileStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripFileMenu = new System.Windows.Forms.ToolStripSeparator();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findNextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findPreviousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewhelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.feedbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.utilizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // centerPanel
            // 
            this.centerPanel.Controls.Add(this.fileViewer);
            this.centerPanel.Controls.Add(this.statusStrip);
            this.centerPanel.Controls.Add(this.menuStrip1);
            this.centerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.centerPanel.Location = new System.Drawing.Point(0, 0);
            this.centerPanel.Name = "centerPanel";
            this.centerPanel.Size = new System.Drawing.Size(800, 450);
            this.centerPanel.TabIndex = 4;
            // 
            // fileViewer
            // 
            this.fileViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileViewer.CharMap = new string[0];
            this.fileViewer.displayMode = LargeFileViewer.DisplayMode.Text;
            this.fileViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileViewer.FileSize = ((long)(0));
            this.fileViewer.Location = new System.Drawing.Point(0, 24);
            this.fileViewer.Name = "fileViewer";
            this.fileViewer.RowCount = 0;
            this.fileViewer.SelectedPosition = 0;
            this.fileViewer.Size = new System.Drawing.Size(800, 402);
            this.fileViewer.TabIndex = 3;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLoadStatus,
            this.toolStripProgress,
            this.toolStripFileSize,
            this.toolStripPos,
            this.toolStripLine});
            this.statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip.Location = new System.Drawing.Point(0, 426);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(800, 24);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripLoadStatus
            // 
            this.toolStripLoadStatus.AutoSize = false;
            this.toolStripLoadStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripLoadStatus.Name = "toolStripLoadStatus";
            this.toolStripLoadStatus.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.toolStripLoadStatus.Size = new System.Drawing.Size(160, 19);
            // 
            // toolStripProgress
            // 
            this.toolStripProgress.Margin = new System.Windows.Forms.Padding(3);
            this.toolStripProgress.Name = "toolStripProgress";
            this.toolStripProgress.Size = new System.Drawing.Size(100, 18);
            this.toolStripProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // toolStripFileSize
            // 
            this.toolStripFileSize.AutoSize = false;
            this.toolStripFileSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripFileSize.Margin = new System.Windows.Forms.Padding(3, 3, 0, 2);
            this.toolStripFileSize.Name = "toolStripFileSize";
            this.toolStripFileSize.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.toolStripFileSize.Size = new System.Drawing.Size(180, 19);
            this.toolStripFileSize.Text = "File Size:";
            this.toolStripFileSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripPos
            // 
            this.toolStripPos.AutoSize = false;
            this.toolStripPos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripPos.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
            this.toolStripPos.Name = "toolStripPos";
            this.toolStripPos.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.toolStripPos.Size = new System.Drawing.Size(60, 19);
            this.toolStripPos.Text = "Pos:";
            this.toolStripPos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripLine
            // 
            this.toolStripLine.AutoSize = false;
            this.toolStripLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripLine.Margin = new System.Windows.Forms.Padding(3, 3, 2, 2);
            this.toolStripLine.Name = "toolStripLine";
            this.toolStripLine.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.toolStripLine.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripLine.Size = new System.Drawing.Size(140, 19);
            this.toolStripLine.Text = "Line:";
            this.toolStripLine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.formatToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newWindowToolStripMenuItem,
            this.openToolStripMenuItem,
            this.PropertiesToolStripMenuItem,
            this.closeFileStripMenuItem,
            this.exitToolStripMenuItem,
            this.toolStripFileMenu});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newWindowToolStripMenuItem
            // 
            this.newWindowToolStripMenuItem.Name = "newWindowToolStripMenuItem";
            this.newWindowToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newWindowToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.newWindowToolStripMenuItem.Text = "New Window";
            this.newWindowToolStripMenuItem.Click += new System.EventHandler(this.newWindowToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // PropertiesToolStripMenuItem
            // 
            this.PropertiesToolStripMenuItem.Name = "PropertiesToolStripMenuItem";
            this.PropertiesToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.PropertiesToolStripMenuItem.Text = "Properties";
            this.PropertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // closeFileStripMenuItem
            // 
            this.closeFileStripMenuItem.Name = "closeFileStripMenuItem";
            this.closeFileStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.closeFileStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.closeFileStripMenuItem.Text = "Close File";
            this.closeFileStripMenuItem.Click += new System.EventHandler(this.closeFileStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStripFileMenu
            // 
            this.toolStripFileMenu.Name = "toolStripFileMenu";
            this.toolStripFileMenu.Size = new System.Drawing.Size(185, 6);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.findToolStripMenuItem,
            this.findNextToolStripMenuItem,
            this.findPreviousToolStripMenuItem,
            this.goToToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.findToolStripMenuItem.Text = "Find";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // findNextToolStripMenuItem
            // 
            this.findNextToolStripMenuItem.Name = "findNextToolStripMenuItem";
            this.findNextToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.findNextToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.findNextToolStripMenuItem.Text = "Find Next";
            this.findNextToolStripMenuItem.Click += new System.EventHandler(this.findNextToolStripMenuItem_Click);
            // 
            // findPreviousToolStripMenuItem
            // 
            this.findPreviousToolStripMenuItem.Name = "findPreviousToolStripMenuItem";
            this.findPreviousToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F3)));
            this.findPreviousToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.findPreviousToolStripMenuItem.Text = "Find Previous";
            this.findPreviousToolStripMenuItem.Click += new System.EventHandler(this.findPreviousToolStripMenuItem_Click);
            // 
            // goToToolStripMenuItem
            // 
            this.goToToolStripMenuItem.Name = "goToToolStripMenuItem";
            this.goToToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.goToToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.goToToolStripMenuItem.Text = "GoTo";
            this.goToToolStripMenuItem.Click += new System.EventHandler(this.goToToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // formatToolStripMenuItem
            // 
            this.formatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hexToolStripMenuItem,
            this.fontToolStripMenuItem});
            this.formatToolStripMenuItem.Name = "formatToolStripMenuItem";
            this.formatToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.formatToolStripMenuItem.Text = "Format";
            // 
            // hexToolStripMenuItem
            // 
            this.hexToolStripMenuItem.Name = "hexToolStripMenuItem";
            this.hexToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.hexToolStripMenuItem.Text = "Hex";
            this.hexToolStripMenuItem.Click += new System.EventHandler(this.hexToolStripMenuItem_Click);
            // 
            // fontToolStripMenuItem
            // 
            this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            this.fontToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.fontToolStripMenuItem.Text = "Font";
            this.fontToolStripMenuItem.Click += new System.EventHandler(this.fontToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // statusBarToolStripMenuItem
            // 
            this.statusBarToolStripMenuItem.Checked = true;
            this.statusBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.statusBarToolStripMenuItem.Name = "statusBarToolStripMenuItem";
            this.statusBarToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.statusBarToolStripMenuItem.Text = "Status Bar";
            this.statusBarToolStripMenuItem.Click += new System.EventHandler(this.statusBarToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewhelpToolStripMenuItem,
            this.feedbackToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.utilizationToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // viewhelpToolStripMenuItem
            // 
            this.viewhelpToolStripMenuItem.Name = "viewhelpToolStripMenuItem";
            this.viewhelpToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.viewhelpToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.viewhelpToolStripMenuItem.Text = "View Help";
            this.viewhelpToolStripMenuItem.Click += new System.EventHandler(this.viewhelpToolStripMenuItem_Click);
            // 
            // feedbackToolStripMenuItem
            // 
            this.feedbackToolStripMenuItem.Name = "feedbackToolStripMenuItem";
            this.feedbackToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.feedbackToolStripMenuItem.Text = "Feedback";
            this.feedbackToolStripMenuItem.Click += new System.EventHandler(this.feedbackToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // utilizationToolStripMenuItem
            // 
            this.utilizationToolStripMenuItem.Name = "utilizationToolStripMenuItem";
            this.utilizationToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.utilizationToolStripMenuItem.Text = "Utilization";
            this.utilizationToolStripMenuItem.Click += new System.EventHandler(this.utilizationToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem1,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.clearToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(103, 60);
            // 
            // copyToolStripMenuItem1
            // 
            this.copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
            this.copyToolStripMenuItem1.Size = new System.Drawing.Size(102, 22);
            this.copyToolStripMenuItem1.Text = "Copy";
            this.copyToolStripMenuItem1.Click += new System.EventHandler(this.copyToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(99, 6);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(99, 6);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.centerPanel);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "Large File Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.centerPanel.ResumeLayout(false);
            this.centerPanel.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Panel centerPanel;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem formatToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem viewhelpToolStripMenuItem;
        private ToolStripMenuItem newWindowToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripSeparator toolStripFileMenu;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem findToolStripMenuItem;
        private ToolStripMenuItem findNextToolStripMenuItem;
        private ToolStripMenuItem findPreviousToolStripMenuItem;
        private ToolStripMenuItem goToToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem hexToolStripMenuItem;
        private ToolStripMenuItem fontToolStripMenuItem;
        private ToolStripMenuItem statusBarToolStripMenuItem;
        private ToolStripMenuItem feedbackToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripLoadStatus;
        private ToolStripMenuItem closeFileStripMenuItem;
        private ToolStripProgressBar toolStripProgress;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem copyToolStripMenuItem1;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem clearToolStripMenuItem;
        private ToolStripMenuItem PropertiesToolStripMenuItem;
        private FileViewer fileViewer;
        private ToolStripMenuItem utilizationToolStripMenuItem;
        private ToolStripStatusLabel toolStripFileSize;
        private ToolStripStatusLabel toolStripPos;
        private ToolStripStatusLabel toolStripLine;
    }
}