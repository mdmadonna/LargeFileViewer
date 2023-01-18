/**********************************************************************************************
 * 
 *  Main class for the Latge File Viewer.
 *  
 *  Copyright 2023 © AJM Software L.L.C.
 **********************************************************************************************/

using System.Text;

using static LargeFileViewer.common;
using static LargeFileViewer.OptionsManager;
using static System.Environment;

namespace LargeFileViewer
{
    public partial class Main : Form
    {
        internal FileMonitor? fileMonitor;
        public Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExHandler);

            throw new Exception("Test Error");

            InitializeComponent();
            StartUpFont = lvFile.Font;
            OptionsManager.Initialize();
            UpdateMenu();
            toolStripLoadStatus.Text = string.Empty;
            toolStripProgress.Visible = false;
            Options.ListFontChanged += OnFontChanged;

            lvFile.View = View.Details;
            lvFile.VirtualMode = true;
            if (defaultFont != null) lvFile.Font = defaultFont;

            // Hook up handlers for VirtualMode events.
            lvFile.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(LV_GetItem);

            // Placeholders for future use
            //lvFile.CacheVirtualItems += new CacheVirtualItemsEventHandler(LV_CacheItems);
            //lvFile.SearchForVirtualItem += new SearchForVirtualItemEventHandler(LV_FindItem);

        }

        #region eventhandlers
        /// <summary>
        /// Executed if the default Font for the ListView changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnFontChanged(object? sender, EventArgs e)
        {
            if (SelectedFont != null) return;
            Font newFont = defaultFont != null ? defaultFont : DefaultFont;
            if (lvFile.InvokeRequired)
            { lvFile.Invoke((MethodInvoker)delegate { lvFile.Font = newFont; }); }
            else
            { lvFile.Font = newFont; }
        }

        /// <summary>
        /// If the file we're viewing or loading changes, this routine is fired and the 
        /// ListView is invalidated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnFileChanged(object? sender, EventArgs e)
        {
            if (lvFile.InvokeRequired)
            { lvFile.Invoke((MethodInvoker)delegate { lvFile.Invalidate(); }); }
            else
            { lvFile.Invalidate(); }
        }

        /// <summary>
        /// This is fired when searching thru the file for some text.  It is executed
        /// on the first hit and during a 'Find Next' or 'Find Prev'.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnFound(object? sender, Find.FoundEventArgs e)
        {
            if (e.newline == 0) return;
            int linenum = e.newline - 1;
            if (lvFile.InvokeRequired)
            {
                lvFile.Invoke((MethodInvoker)delegate { lvFile.EnsureVisible(linenum); });
                lvFile.Invoke((MethodInvoker)delegate { lvFile.SelectedIndices.Clear(); });
                lvFile.Invoke((MethodInvoker)delegate { lvFile.SelectedIndices.Add(linenum); });
            }
            else
            {
                lvFile.EnsureVisible(linenum);
                lvFile.SelectedIndices.Clear();
                lvFile.SelectedIndices.Add(linenum);
            }
        }

        /// <summary>
        /// Global error handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void ExHandler(object? sender, UnhandledExceptionEventArgs e)
        {
            bManualStop = true;
            Form? find = AppForm(FINDFORM);
            if (find != null) { ((Find)find).bCancelSearch= true; }

            string errorMsg = "A fatal error has occurred." + Environment.NewLine;
            Exception ex = (Exception)e.ExceptionObject;
            errorMsg += ex.Message + Environment.NewLine;
            errorMsg += "The program will now terminate";
            ShowMessage(errorMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);

            WriteErrorLog(string.Format("Error Message: {0}", ex.Message));
            if (!string.IsNullOrEmpty(ex.Source)) WriteErrorLog(string.Format("Source: {0}", ex.Source));
            if (!string.IsNullOrEmpty(ex.StackTrace)) WriteErrorLog(string.Format("Trace: {0}", ex.StackTrace));

            if (ex.InnerException != null)
            {
                WriteErrorLog(string.Format("Inner Error Message: {0}", ex.InnerException.Message));
                if (!string.IsNullOrEmpty(ex.InnerException.Source)) WriteErrorLog(string.Format("Inner Source: {0}", ex.InnerException.Source));
                if (!string.IsNullOrEmpty(ex.InnerException.StackTrace)) WriteErrorLog(string.Format("Inner Trace: {0}", ex.InnerException.StackTrace));
            }
            Application.Exit();
        }

        /// <summary>
        /// Write a line to the error log
        /// </summary>
        /// <param name="errorMsg"></param>
        internal void WriteErrorLog(string errorMsg)
        {
            var commonpath = GetFolderPath(SpecialFolder.CommonApplicationData);
            string dir = Path.Combine(commonpath, "ajmsoft\\LargeFileViewer");
            string logfilename = Path.Combine(dir, LOGFILENAME);
            try
            {
                using (StreamWriter sw = new StreamWriter(logfilename, true))
                {
                    sw.WriteLine(string.Format("{0}|{1}", DateTime.Now, errorMsg));
                }
            }
            catch { }
        }
        #endregion

        /// <summary>
        /// Populate the recently used files in the menu
        /// </summary>
        private void UpdateMenu()
        {
            if (optionsdata == null) return;
            if (optionsdata.MRUFiles == null) return;

            // Refresh Recently Used file list
            // First, delete any existing entries
            int i;
            int j = fileToolStripMenuItem.DropDownItems.Count;
            for (i = 0; i < j; i++)
            {
                int idx = j - (i + 1);
                if (fileToolStripMenuItem.DropDownItems == null) return;
                if (fileToolStripMenuItem.DropDownItems[idx].Tag != null)
                {
                    if (fileToolStripMenuItem.DropDownItems[idx].Tag.ToString() == "RecentFile")
                    {
                        fileToolStripMenuItem.DropDownItems[idx].Click -= recentFileToolStripMenuItem_Click;
                        fileToolStripMenuItem.DropDownItems.RemoveAt(idx);
                    }
                }
            }

            // Second, add the current list
            for (i = 0; i < optionsdata.MRUFiles.Count; i++)
            {
                int idx = optionsdata.MRUFiles.Count - (i + 1);
                ToolStripMenuItem tsRecent = new ToolStripMenuItem(optionsdata.MRUFiles[idx]);
                tsRecent.Click += recentFileToolStripMenuItem_Click;
                tsRecent.Tag = "RecentFile";
                fileToolStripMenuItem.DropDownItems.Add(tsRecent);
            }
        }

        /// <summary>
        /// Establish the initial position of forms opened by the main program.
        /// </summary>
        /// <param name="f"></param>
        private void PositionForm(Form f)
        {
            f.Top = this.Top + 20;
            f.Left = this.Left + 20;
        }

        /// <summary>
        /// Handler when a recently used file is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recentFileToolStripMenuItem_Click(object? sender, EventArgs? e)
        {
            if (sender == null) return;
            string filename = ((ToolStripMenuItem)sender).Text;
            if (!SetupFileProperties(filename))
            {
                ShowMessage(string.Format("Can't open {0}.", filename));
                return;
            }
            ProcessFile();
        }

        #region Menu Handlers
        /// <summary>
        /// Start a new instance of this program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartApp();
        }

        /// <summary>
        /// Open a File and load it into the listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!OpenFile()) return;
            ProcessFile();
        }

        /// <summary>
        /// Display properties of the opened file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FileProperties.FileName))
            {
                ShowMessage("Please open a file to view properties.");
                return;
            }
            Form? f = AppForm(PROPSFORM);
            if (f == null) { f = new Props(); PositionForm(f); }
            f.Show();
            f.Activate();
        }

        /// <summary>
        /// Clear the current contents of the listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeFileStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearFile();
        }

        /// <summary>
        /// Close Program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Copy the selected line
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedindices = lvFile.SelectedIndices;
            if (selectedindices.Count == 0) return;
            var s = lvFile.Items[selectedindices[0]].SubItems[1].Text;
            Clipboard.SetText(s);
        }

        /// <summary>
        /// Open the Find Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form? f = AppForm(FINDFORM);
            if (f == null)
            {
                f = new Find();
                ((Find)f).Found += OnFound;
                PositionForm(f);
            }
            f.Show();
            f.Activate();
        }

        /// <summary>
        /// Find the next match or open the Find Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form? finder = AppForm("Find");
            if (finder == null) return;
            ((Find)finder).MoveFinder(Find.SearchDirection.Forward);
        }

        /// <summary>
        /// Find the previous dialog or open the Find Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findPreviousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form? finder = AppForm("Find");
            if (finder == null) return;
            ((Find)finder).MoveFinder(Find.SearchDirection.Backward);
        }

        /// <summary>
        /// Open the GoTo line dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FileProperties.FileName))
            {
                ShowMessage("Please open a file before using this feature.");
                return;
            }
            GoTo f = new GoTo();
            f.ShowDialog();
            int lineNum = f.LineNumber;
            f.Dispose();
            if (lineNum > 0) lvFile.EnsureVisible(lineNum - 1);
        }

        /// <summary>
        /// Option the Options dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form? f = AppForm(OPTIONSFORM);
            if (f == null) { f = new Options(); PositionForm(f); }
            f.Show();
            f.Activate();
        }

        /// <summary>
        /// Display and format the file in Hex. This is not currently implemented.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is not yet implemented.", PROGRAMNAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// Change the font for the current ListView. The new font remains active until
        /// the program is closed or the Default Font changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!GetFont(lvFile.Font)) return;
            lvFile.Font = SelectedFont;
            lvFile.Invalidate();
        }

        /// <summary>
        /// Show/Hide the Status Bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusBarToolStripMenuItem.Checked = !statusBarToolStripMenuItem.Checked;
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Show help.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewhelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form? f = AppForm(HELPFORM);
            if (f == null) { f = new Help(); PositionForm(f); }
            f.Show();
            f.Activate();
        }

        /// <summary>
        /// Provide feedback/support requests to the author.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void feedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form? f = AppForm(FEEDBACKFORM);
            if (f == null) { f = new Feedback(); PositionForm(f); }
            f.Show();
            f.Activate();
        }

        /// <summary>
        /// Display the About Box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form? f = AppForm(ABOUTFORM);
            if (f == null) { f = new About(); PositionForm(f); }
            f.Show();
            f.Activate();
        }

        // Context Menu click Events
        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            copyToolStripMenuItem_Click(sender, e);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeFileStripMenuItem_Click(sender, e);
        }
        #endregion
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form? finder = AppForm(FINDFORM);
            if (finder != null) finder.Close();
            bManualStop = true;
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Placeholder only - not currently used for a button but will fire
        /// when the escape key is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            bManualStop = true;
        }

        public void ReloadFile()
        {
            string f = FileProperties.FileName;
            ClearFile();
            SetupFileProperties(f);
            ProcessFile();
        }

        /// <summary>
        /// Reset file related variables and stop current activities
        /// as needed.
        /// </summary>
        /// <param name="bClearCurrentFileInfo"></param>
        public void ClearFile(bool bClearCurrentFileInfo = true)
        {
            this.Text = PROGRAMNAME;
            if ((!bFileLoaded & !bManualStop))
            {
                bManualStop = true;
                Thread.Sleep(500);
            }
            Form? finder = AppForm("Find");
            if (finder != null)
            {
                finder.Close();
                Thread.Sleep(500);
            }
            lvFile.VirtualListSize = 0;
            toolStripLoadStatus.Text = string.Empty;
            toolStripProgress.Value = 0;
            if (bClearCurrentFileInfo) ClearCurrentFile();
            if (fileMonitor != null)
            {
                fileMonitor.FileChanged -= OnFileChanged;
                fileMonitor.Close();
                fileMonitor = null;
            }
        }

        /// <summary>
        /// Start a Task to load the requested file.
        /// </summary>
        internal void ProcessFile()
        {
            // Stop any in-flight activities
            ClearFile(false);

            // Initialize appropriate variables and start background Load Task
            this.Text = string.Format("{0} {1}", PROGRAMNAME, FileProperties.FileName);
            OptionsManager.Add(FileProperties.FileName);
            UpdateMenu();
            bManualStop = false;
            bFileLoaded = false;
            bFileInvalid = false;
            idx.Clear();
            toolStripProgress.Visible = true;
            fileMonitor = new FileMonitor();
            fileMonitor.FileChanged += OnFileChanged;
            if (SelectedFont != null) lvFile.Font = SelectedFont;
            else
            if (defaultFont != null) lvFile.Font = defaultFont;

            Thread t = new Thread(FileIndex)
            {
                Name = "FileOpen",
                Priority = ThreadPriority.AboveNormal
            };
            t.Start();

            // Track status of the file load
            Application.DoEvents();
            while (!bFileLoaded & !bManualStop)
            {
                toolStripLoadStatus.Text = string.Format("Reading {0}", _linecount);
                lvFile.VirtualListSize = FileProperties.LineCount;
                toolStripProgress.Value = Convert.ToInt32((FileProperties.BytesRead * 100) / FileProperties.FileLen);
                Application.DoEvents();
            }
            lvFile.VirtualListSize = _linecount;
            toolStripLoadStatus.Text = string.Format("{0}: {1} lines read", bManualStop ? "Stopped" : "Done", _linecount);
            if (bFileLoaded && !bManualStop) { toolStripProgress.Value = 0; toolStripProgress.Visible = false; }
        }

        void LV_GetItem(object? sender, RetrieveVirtualItemEventArgs e)
        {
            if (bFileInvalid)
            {
                e.Item = LV_ErrorItem(e.ItemIndex + 1);
                return;
            }

            if (!idx.TryGetValue(e.ItemIndex + 1, out LineIndex? li))
            {
                throw new Exception(string.Format("Invalid Item Index : {0}", e.ItemIndex.ToString()));
            }
            FileStream fs = File.Open(FileProperties.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            fs.Seek(li.pos, SeekOrigin.Begin);
            byte[] buff = new byte[li.len];
            fs.Read(buff, 0, buff.Length);
            fs.Close();
            string line = Encoding.UTF8.GetString(buff);
            ListViewItem lv = new ListViewItem((e.ItemIndex + 1).ToString());
            lv.SubItems.Add(line);
            e.Item = lv;
        }
        ListViewItem LV_ErrorItem(int eidx)
        {
            ListViewItem lv = new ListViewItem((eidx).ToString());
            lv.SubItems.Add("The file has changed. Please reload it.");
            return lv;
        }

        // Manages the cache.  ListView calls this when it might need a cache
        // refresh. It is a placeholder only and may be used in the future.
        void LV_CacheItems(object? sender, CacheVirtualItemsEventArgs e)
        {
        }

        // This event handler is used for List search functionality. It is called
        // for a search request when in Virtual mode. It is not very flexible so
        // it is not currently in use.
        void LV_FindItem(object? sender, SearchForVirtualItemEventArgs e)
        {

        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                if (!bFileLoaded & !bManualStop)
                {
                    object sender = new();
                    EventArgs e = new();
                    btnCancel_Click(sender, e);
                }
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }
    }
    public partial class newListView : ListView
    {
        public newListView()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}