/**********************************************************************************************
 * 
 *  This Form is move to a specific line in the file.
 *  
 *  Copyright 2023 © AJM Software L.L.C.
 **********************************************************************************************/

using static LargeFileViewer.common;

namespace LargeFileViewer
{
    public partial class GoTo : Form
    {
        public int LineNumber { get; set; }
        public GoTo()
        {
            InitializeComponent();
        }

        private void GoTo_Load(object sender, EventArgs e)
        {
            lblRange.Text = string.Format("Range: {0} - {1}", FileProperties.LineCount == 0 ? 0 : 1, FileProperties.LineCount);
        }

        private void btnGoTo_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtLineNumber.Text, out int lineNumber))
            { ShowMessage("Please enter a valid line number."); return; }
            if (lineNumber <= 0 || lineNumber > FileProperties.LineCount) { ShowMessage("Please enter a valid line number."); ; return; }
            LineNumber = lineNumber;
            this.Close();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

    }
}
