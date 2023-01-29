/**********************************************************************************************
 * 
 *  Display an About Box for the Large File Viewer.
 *  
 *  Copyright 2023 © AJM Software L.L.C.
 **********************************************************************************************/

using System.Diagnostics;
using System.Reflection;


namespace LargeFileViewer
{
    public partial class Stats : Form
    {
        public Stats()
        {
            InitializeComponent();
        }

        private void Stats_Load(object sender, EventArgs e)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            lblVersion.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            lblCompileDate.Text = File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString();

            Process p = Process.GetCurrentProcess();
            lblProcTime.Text = string.Format("{0} ms", p.TotalProcessorTime.TotalMilliseconds.ToString("N4"));
            lblCurVirtualMem.Text = p.VirtualMemorySize64.ToString("N0");
            lblPeakVirtualMem.Text = p.PeakVirtualMemorySize64.ToString("N0");
            lblCurWorkingSet.Text = p.WorkingSet64.ToString("N0");
            lblPeakWorkingSet.Text = p.PeakWorkingSet64.ToString("N0");
            lblPrivateMem.Text = p.PrivateMemorySize64.ToString("N0");
            lblHandles.Text = p.HandleCount.ToString("N0");
            lblThreads.Text = p.Threads.Count.ToString("N0");
        }
        private void Stats_Click(object sender, EventArgs e)
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
