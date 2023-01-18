/**********************************************************************************************
 * 
 *  Display properties of the file loaded in theListView.
 *  
 *  Copyright 2023 © AJM Software L.L.C.
 **********************************************************************************************/

namespace LargeFileViewer
{
    public partial class Props : Form
    {
        public Props()
        {
            InitializeComponent();
        }

        private void Props_Load(object sender, EventArgs e)
        {
            lblFileName.Text = FileProperties.BaseFileName;
            txtFullFileName.Text = FileProperties.FileName;
            txtDirectory.Text = FileProperties.FilePath;
            lblExtension.Text = Path.GetExtension(FileProperties.FileName);
            lblLength.Text = string.Format("{0} bytes", FileProperties.FileLen.ToString("N0"));
            lblLines.Text = FileProperties.LineCount.ToString();
            lblCreateDate.Text = FileProperties.CreateDate.ToString();
            lblModifyDate.Text = FileProperties.ModifiedDate.ToString();

            cbReadOnly.Checked = FileProperties.ReadOnly;
            cbArchive.Checked = FileProperties.Archive;
            cbSystem.Checked = FileProperties.System;
            cbHidden.Checked = FileProperties.Hidden;
            cbCompressed.Checked = FileProperties.Compressed;
            cbEncrypted.Checked = FileProperties.Encrypted;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
