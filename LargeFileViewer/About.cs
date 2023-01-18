/**********************************************************************************************
 * 
 *  Display an About Box for the Large File Viewer.
 *  
 *  Copyright 2023 © AJM Software L.L.C.
 **********************************************************************************************/

using System.Reflection;


namespace LargeFileViewer
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            lblVersion.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            lblCompileDate.Text = File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString();
            lblCopy.Text = "\u00a9 2023 AJM Software, L.L.C.";

            HttpClient client = new HttpClient();
            string licenseURI = "https://raw.githubusercontent.com/mdmadonna/LargeFileViewer/main/LICENSE";
            try
            {
                string responseBody = client.GetStringAsync(licenseURI).Result;
                responseBody = responseBody.Replace("\n\n", "{linemarker}{linemarker}");
                responseBody = responseBody.Replace("\n", string.Empty);
                responseBody = responseBody.Replace("{linemarker}", Environment.NewLine);
                txtLicense.Text = responseBody;
            }
            catch 
            {
                txtLicense.Text = "Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the \"Software\"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:\r\n\r\nThe above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.\r\n\r\nTHE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.";
            }
        }

        private void About_Click(object sender, EventArgs e)
        {
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
