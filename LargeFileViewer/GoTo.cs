/**********************************************************************************************
 * 
 *  This Form is move to a specific line in the file.
 *  
 *  Copyright 2023 © AJM Software L.L.C.
 **********************************************************************************************/

using static LargeFileViewer.Common;

namespace LargeFileViewer
{
    public partial class GoTo : Form
    {
        /// <summary>
        /// The number of lines in the file.
        /// </summary>
        public int MaxLineNumber { get; set; }
        /// <summary>
        /// The line number to move to the top of the File Viewer.
        /// </summary>
        public int LineNumber { get; set; }
        public GoTo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoTo_Load(object sender, EventArgs e)
        {
            lblRange.Text = string.Format("Range: 1 - {0}", MaxLineNumber);
            txtLineNumber.Text = LineNumber > 0 && LineNumber <= MaxLineNumber ? LineNumber.ToString() : "1";
        }

        /// <summary>
        /// Validate the Line Number entered and close the dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoTo_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtLineNumber.Text, out int lineNumber))
            { ShowMessage("Please enter a valid line number."); return; }
            if (lineNumber <= 0 || lineNumber > MaxLineNumber) { ShowMessage("Please enter a valid line number."); ; return; }
            LineNumber = lineNumber;
            this.Close();
        }

        /// <summary>
        /// Trap the Escape key to close the dialog..
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
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
