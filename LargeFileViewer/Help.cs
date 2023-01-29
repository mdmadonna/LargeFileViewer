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
        public Help()
        {
            InitializeComponent();
        }

        private void Help_Load(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            string helpURI = "https://raw.githubusercontent.com/mdmadonna/LargeFileViewer/master/Help.rtf";
            try
            {
                string responseBody = client.GetStringAsync(helpURI).Result;
                txtHelp.Text = responseBody;
                txtHelp.TabStop = false;
            }
            catch (HttpRequestException ex)
            {
                txtHelp.Text = ex.Message;
            }

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
