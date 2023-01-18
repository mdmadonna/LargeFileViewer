/**********************************************************************************************
 * 
 *  Display and maintain program options.
 *  
 *  Copyright 2023 © AJM Software L.L.C.
 **********************************************************************************************/

using static LargeFileViewer.common;
using static LargeFileViewer.OptionsManager;

namespace LargeFileViewer
{
    public partial class Options : Form
    {
        Font? curFont;
        // Event used to signal the Main thread that the default Font has changed
        public static event EventHandler? ListFontChanged;

        /// <summary>
        /// Constructor insures that the MRU (Most Recently Used) list is a value
        /// between 1 and 10. Default is 5.
        /// </summary>
        public Options()
        {
            InitializeComponent();
            lblStatus.Text = string.Empty;
            numMaxRecent.Value = optionsdata.MaxFiles < 1 || optionsdata.MaxFiles > 10 ? 5 : optionsdata.MaxFiles;
            curFont = defaultFont;
            InitializeForm();
        }

        private void InitializeForm()
        {
            cbDefault.Checked = (curFont == null);
            txtFont.Text = curFont != null ? curFont.Name : string.Empty;
            txtFontSize.Text = curFont != null ? curFont.Size.ToString() : string.Empty;
            txtFontStyle.Text = curFont != null ? curFont.Style.ToString() : string.Empty;
        }

        /// <summary>
        /// Initiate the Font Dialog to select a new default Font.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindFont_Click(object sender, EventArgs e)
        {
            if (!GetFont(curFont)) return;
            if (SelectedFont == null) { return; }
            curFont = SelectedFont;
            InitializeForm();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (curFont == null && !cbDefault.Checked)
            {
                ShowMessage("Select a font of check 'Use System Default'");
                cbDefault.Focus();
                return;
            }
            optionsdata.MaxFiles = (int)numMaxRecent.Value;
            defaultFont = curFont;
            lblStatus.Text = WriteOptions() ? "Options Saved" : "Options not saved";
            if (ListFontChanged != null) ListFontChanged.Invoke(null, EventArgs.Empty);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDefault.Checked)
            {
                curFont = null;
                InitializeForm();
            }
        }
    }
}
