namespace LargeFileViewer
{
    partial class Options
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.numMaxRecent = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFont = new System.Windows.Forms.TextBox();
            this.btnFindFont = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFontSize = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.txtFontStyle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbDefault = new System.Windows.Forms.CheckBox();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxRecent)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // numMaxRecent
            // 
            this.numMaxRecent.Location = new System.Drawing.Point(26, 20);
            this.numMaxRecent.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numMaxRecent.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxRecent.Name = "numMaxRecent";
            this.numMaxRecent.Size = new System.Drawing.Size(43, 23);
            this.numMaxRecent.TabIndex = 0;
            this.numMaxRecent.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Maximum number of recently opened files";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Font:";
            // 
            // txtFont
            // 
            this.txtFont.Location = new System.Drawing.Point(82, 59);
            this.txtFont.Name = "txtFont";
            this.txtFont.ReadOnly = true;
            this.txtFont.Size = new System.Drawing.Size(200, 23);
            this.txtFont.TabIndex = 3;
            this.txtFont.TabStop = false;
            // 
            // btnFindFont
            // 
            this.btnFindFont.Location = new System.Drawing.Point(288, 59);
            this.btnFindFont.Name = "btnFindFont";
            this.btnFindFont.Size = new System.Drawing.Size(32, 23);
            this.btnFindFont.TabIndex = 4;
            this.btnFindFont.Text = "...";
            this.btnFindFont.UseVisualStyleBackColor = true;
            this.btnFindFont.Click += new System.EventHandler(this.btnFindFont_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Font Size:";
            // 
            // txtFontSize
            // 
            this.txtFontSize.Location = new System.Drawing.Point(82, 89);
            this.txtFontSize.Name = "txtFontSize";
            this.txtFontSize.ReadOnly = true;
            this.txtFontSize.Size = new System.Drawing.Size(39, 23);
            this.txtFontSize.TabIndex = 6;
            this.txtFontSize.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(182, 241);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(281, 241);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "Close";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtFontStyle
            // 
            this.txtFontStyle.Location = new System.Drawing.Point(82, 118);
            this.txtFontStyle.Name = "txtFontStyle";
            this.txtFontStyle.ReadOnly = true;
            this.txtFontStyle.Size = new System.Drawing.Size(71, 23);
            this.txtFontStyle.TabIndex = 9;
            this.txtFontStyle.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Font Style:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbDefault);
            this.groupBox1.Controls.Add(this.txtFont);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnFindFont);
            this.groupBox1.Controls.Add(this.txtFontStyle);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtFontSize);
            this.groupBox1.Location = new System.Drawing.Point(26, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(334, 162);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Default Font";
            // 
            // cbDefault
            // 
            this.cbDefault.AutoSize = true;
            this.cbDefault.Location = new System.Drawing.Point(15, 27);
            this.cbDefault.Name = "cbDefault";
            this.cbDefault.Size = new System.Drawing.Size(127, 19);
            this.cbDefault.TabIndex = 11;
            this.cbDefault.Text = "Use System Default";
            this.cbDefault.UseVisualStyleBackColor = true;
            this.cbDefault.CheckedChanged += new System.EventHandler(this.cbDefault_CheckedChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(36, 245);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(52, 15);
            this.lblStatus.TabIndex = 12;
            this.lblStatus.Text = "lblStatus";
            // 
            // Options
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(372, 276);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numMaxRecent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Options";
            ((System.ComponentModel.ISupportInitialize)(this.numMaxRecent)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NumericUpDown numMaxRecent;
        private Label label1;
        private Label label2;
        private TextBox txtFont;
        private Button btnFindFont;
        private Label label3;
        private TextBox txtFontSize;
        private Button btnSave;
        private Button btnExit;
        private TextBox txtFontStyle;
        private Label label4;
        private GroupBox groupBox1;
        private CheckBox cbDefault;
        private Label lblStatus;
    }
}