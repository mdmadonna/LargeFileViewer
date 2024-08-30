namespace LargeFileViewer
{
    partial class FileViewer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.textViewer = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineViewer = new System.Windows.Forms.RichTextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.Location = new System.Drawing.Point(876, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(17, 234);
            this.vScrollBar.TabIndex = 5;
            this.vScrollBar.ValueChanged += new System.EventHandler(this.vScrollBar_ValueChanged);
            // 
            // textViewer
            // 
            this.textViewer.BackColor = System.Drawing.SystemColors.Window;
            this.textViewer.ContextMenuStrip = this.contextMenuStrip1;
            this.textViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textViewer.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textViewer.Location = new System.Drawing.Point(70, 0);
            this.textViewer.Name = "textViewer";
            this.textViewer.Size = new System.Drawing.Size(806, 234);
            this.textViewer.TabIndex = 8;
            this.textViewer.Text = "";
            this.textViewer.SelectionChanged += new System.EventHandler(this.textViewer_SelectionChanged);
            this.textViewer.ClientSizeChanged += new System.EventHandler(this.textViewer_ClientSizeChanged);
            this.textViewer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textViewer_KeyDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.toolStripSeparator1,
            this.clearToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(120, 54);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(116, 6);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // lineViewer
            // 
            this.lineViewer.Dock = System.Windows.Forms.DockStyle.Left;
            this.lineViewer.Location = new System.Drawing.Point(0, 0);
            this.lineViewer.Name = "lineViewer";
            this.lineViewer.Size = new System.Drawing.Size(67, 234);
            this.lineViewer.TabIndex = 9;
            this.lineViewer.Text = "";
            this.lineViewer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textViewer_KeyDown);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(67, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 234);
            this.splitter1.TabIndex = 10;
            this.splitter1.TabStop = false;
            // 
            // FileViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textViewer);
            this.Controls.Add(this.vScrollBar);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.lineViewer);
            this.Name = "FileViewer";
            this.Size = new System.Drawing.Size(893, 234);
            this.FontChanged += new System.EventHandler(this.FileViewer_FontChanged);
            this.Resize += new System.EventHandler(this.FileViewer_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private VScrollBar vScrollBar;
        private RichTextBox textViewer;
        private RichTextBox lineViewer;
        private Splitter splitter1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem clearToolStripMenuItem;
    }
}
