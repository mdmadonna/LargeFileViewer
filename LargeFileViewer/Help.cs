/**********************************************************************************************
 * 
 *  Help text is maintained on GitHub.  This Form simply downloads it and displays it in
 *  a TextBox.
 *  
 *  Copyright 2023 © AJM Software L.L.C.
 **********************************************************************************************/

namespace LargeFileViewer
{
    public partial class Help : Form
    {
        private bool _activated = false;
        public Help()
        {
            InitializeComponent();
        }

        private void Help_Load(object sender, EventArgs e)
        {
            
        }

        private void Help_Activated(object sender, EventArgs e)
        {
            if (_activated) return;

            _activated = true;
            txtHelp.Text = "Please stand by...";
            Application.DoEvents();
            HttpClient client = new HttpClient();
            string helpURI = "https://raw.githubusercontent.com/mdmadonna/LargeFileViewer/master/Help.rtf";
            try
            {
                string responseBody = client.GetStringAsync(helpURI).Result;
                txtHelp.Clear();
                txtHelp.Rtf = responseBody;
                txtHelp.TabStop = false;
            }
            catch (Exception ex) 
            {
                txtHelp.Text = string.Format("Help is currently unavailable.{0}{0}{1}", Environment.NewLine, ex.Message);
            }
            client.Dispose();
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
