/**********************************************************************************************
 * 
 *  Start the default mail program with the Large File Viewer support address.
 *  
 *  Copyright 2023 © AJM Software L.L.C.
 **********************************************************************************************/

using System.Diagnostics;

namespace LargeFileViewer
{
    public partial class Feedback : Form
    {
        public Feedback()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process p = new();
            p.StartInfo.FileName = "mailto:support@ajmsoft.com?subject=Large File Viewer Support";
            p.StartInfo.UseShellExecute = true;
            p.Start();
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
