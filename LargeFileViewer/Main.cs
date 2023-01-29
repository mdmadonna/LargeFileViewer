/**********************************************************************************************
 * 
 *  Main class for the Large File Viewer.
 *  
 *  Copyright 2023 © AJM Software L.L.C.
 **********************************************************************************************/

using System.Text;

using static LargeFileViewer.common;
using static LargeFileViewer.FileContainer;
using static LargeFileViewer.FileViewer;
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
            InitializeComponent();
            OptionsManager.Initialize();
            UpdateMenu();
            statusStrip.BackColor = Color.Empty;
            toolStripLoadStatus.Text = string.Empty;
            toolStripProgress.Visible = false;
            toolStripProgress.BackColor = Color.Empty;
            toolStripFileSize.Alignment = ToolStripItemAlignment.Right;
            toolStripPos.Alignment = ToolStripItemAlignment.Right;
            toolStripLine.Alignment = ToolStripItemAlignment.Right;
            toolStripLine.TextAlign = ContentAlignment.MiddleLeft;
            InitStatusStrip();

            Options.ListFontChanged += OnFontChanged;
            
            InitFileView();
        }
        private void InitFileView()
        {
            fileViewer.RetrieveItem += FV_GetItem;
            fileViewer.SelectedPositionChanged += OnPositionChanged;
            StartUpFont = fileViewer.Font;
            if (defaultFont != null) fileViewer.Font = defaultFont;
        }

        #region eventhandlers

        /// <summary>
        /// Fired if the FileViewer is cleared
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnCleared(object? sender, EventArgs e)
        {
            Clear();
        }

        /// <summary>
        /// Executed if the default Font for the FileViewer changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnFontChanged(object? sender, EventArgs e)
        {
            if (SelectedFont != null) return;
            Font newFont = defaultFont != null ? defaultFont : DefaultFont;
            if (fileViewer.InvokeRequired)
            { fileViewer.Invoke((MethodInvoker)delegate { fileViewer.Font = newFont; }); }
            else
            { fileViewer.Font = newFont; }
        }

        /// <summary>
        /// If the file we're viewing or loading changes, this routine is fired and the 
        /// FileViewer is invalidated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnFileChanged(object? sender, EventArgs e)
        {
            if (fileViewer.InvokeRequired)
            { fileViewer.Invoke((MethodInvoker)delegate { fileViewer.Invalidate(); }); }
            else
            { fileViewer.Invalidate(); }
        }

        /// <summary>
        /// This is fired when the cursor moves to a new line in the FileViewer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnPositionChanged(object? sender, SelectedPositionChangeEventArgs e)
        {
            toolStripLine.Text = string.Format("Line: {0}", (e.Line + 1).ToString("N0"));
            toolStripPos.Text = string.Format("Pos: {0}", (e.Position + 1).ToString());
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
            if (fileViewer.InvokeRequired)
            {
                fileViewer.Invoke((MethodInvoker)delegate { fileViewer.EnsureVisible(linenum); });
            }
            else
            {
                fileViewer.EnsureVisible(linenum);
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
            if (find != null) { ((Find)find).Cancel(); }

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
            if (!LoadFileInfo(filename))
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
            if (!Open()) return;
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
            if (!string.IsNullOrEmpty(fileViewer.SelectedText)) Clipboard.SetText(fileViewer.SelectedText);
        }

        /// <summary>
        /// Open the Find Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileViewer.displayMode == DisplayMode.Hex)
            {
                ShowMessage("'Find' is not available while in Hex Mode.");
                return;
            }
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
            f.MaxLineNumber = fileViewer.displayMode == DisplayMode.Text ? fileViewer.RowCount : fileViewer.HexRowCount;
            f.ShowDialog();
            int lineNum = f.LineNumber;
            f.Dispose();
            if (lineNum > 0) fileViewer.EnsureVisible(lineNum - 1);
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
            hexToolStripMenuItem.Checked = !hexToolStripMenuItem.Checked;
            ChangeDisplayMode();
        }

        /// <summary>
        /// Change the font for the current ListView. The new font remains active until
        /// the program is closed or the Default Font changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!GetFont(fileViewer.Font)) return;
            fileViewer.Font = SelectedFont;
            fileViewer.Invalidate();
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
            if (f == null) { f = new AboutBox(); PositionForm(f); }
            f.Show();
            f.Activate();
        }

        private void utilizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form? f = AppForm(UTILFORM);
            if (f == null) { f = new Stats(); PositionForm(f); }
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

        #region UI
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

        /// <summary>
        /// Trap the Esc key and halt any ongoing file loads,
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
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

        #endregion

        #region Internal Routines
        private void ChangeDisplayMode()
        {
            int currentLine = fileViewer.FirstVisibleLine;
            fileViewer.displayMode = hexToolStripMenuItem.Checked ? DisplayMode.Hex : DisplayMode.Text;
            if (string.IsNullOrEmpty(FileProperties.FileName)) return;
            if (fileViewer.displayMode == DisplayMode.Hex)
            { TextToHex(currentLine); }
            else
            { HexToText(currentLine); }

        }
        private void HexToText(int curline)
        {
            // Reset the Viewer Font
            if (SelectedFont != null) { fileViewer.Font = SelectedFont; }
            else if (Options.DefaultFont != null) { fileViewer.Font = DefaultFont; }
            else fileViewer.Font = StartUpFont;

            // Force refresh of the Viewer
            if (FileProperties.LineCount == 0)
            { 
                ProcessFile(); 
            }
            else
            {
                long offset = GetHexOffset(curline-1, HexLineLen);
                curline = TextLineAtOffset(offset);
                toolStripLoadStatus.Text = string.Format("{0} lines available", fileViewer.RowCount);
                fileViewer.Refresh();
                fileViewer.EnsureVisible(curline);
            }
        }

        private void TextToHex(int curline)
        {
            // Close Find Form as needed until it has been modified to accommodate
            // Hex search.
            Form? f = AppForm(FINDFORM);
            if (f != null) ((Find)f).Close();

            long offset = GetOffset(curline);
            curline = HexLineAtOffset(offset, HexLineLen);
            toolStripLoadStatus.Text = string.Format("{0} lines available", fileViewer.HexRowCount);

            // Hex View requires a monospaced Font
            fileViewer.Font = new Font(FontFamily.GenericMonospace, fileViewer.Font.Size);
            fileViewer.Refresh();
            fileViewer.EnsureVisible(curline);
        }

        public void ReloadFile()
        {
            string f = FileProperties.FileName;
            ClearFile();
            LoadFileInfo(f);
            ProcessFile();
        }

        /// <summary>
        /// Reset file related variables and stop current activities
        /// as needed.
        /// </summary>
        /// <param name="bClearCurrentFileInfo"></param>
        public void ClearFile(bool bClearCurrentFileInfo = true)
        {
            // Reset the Main Window Caption
            this.Text = PROGRAMNAME;

            // If a file load is in progress, stop it.
            if ((!bFileLoaded & !bManualStop))
            {
                bManualStop = true;
                Thread.Sleep(500);
            }

            // Close an open Find Form as needed.
            Form? finder = AppForm("Find");
            if (finder != null)
            {
                finder.Close();
                Thread.Sleep(500);
            }

            // Clear date out of the File Viewer
            fileViewer.Clear();

            // Re-initiialize the Status Bar
            InitStatusStrip();

            // Clear out all file related data and stop monitoring
            // the file for changes/
            if (bClearCurrentFileInfo) Clear();
            if (fileMonitor != null)
            {
                fileMonitor.FileChanged -= OnFileChanged;
                fileMonitor.Close();
                fileMonitor = null;
            }
        }

        /// <summary>
        /// Clear values out of the StatusBar
        /// </summary>
        private void InitStatusStrip()
        {
            toolStripLoadStatus.Text = string.Empty;
            toolStripLine.Text = string.Empty;
            toolStripPos.Text = string.Empty;
            toolStripFileSize.Text = string.Empty;
            toolStripProgress.Value = 0;
            toolStripProgress.Visible = false;
        }

        /// <summary>
        /// Start a Task to load the requested file.
        /// </summary>
        internal void ProcessFile()
        {
            // Stop any in-flight activities
            ClearFile(false);
            fileViewer.FileSize = FileProperties.FileLen;

            // Initialize appropriate variables and start background Load Task
            this.Text = string.Format("{0} {1}", PROGRAMNAME, FileProperties.FileName);
            OptionsManager.Add(FileProperties.FileName);
            UpdateMenu();
            bManualStop = false;
            bFileLoaded = false;
            bFileInvalid = false;
            idx.Clear();
            toolStripProgress.Visible = true;
            toolStripFileSize.Text = string.Format("File Size: {0}", FileProperties.FileLen.ToString("N0"));
            fileMonitor = new FileMonitor();
            fileMonitor.FileChanged += OnFileChanged;

            if (fileViewer.displayMode == DisplayMode.Hex)
            {
                fileViewer.Font = new Font(FontFamily.GenericMonospace, fileViewer.Font.Size);
                toolStripProgress.Visible = false;
                toolStripLoadStatus.Text = string.Format("{0} lines available", fileViewer.HexRowCount);
                return;
            }

            if (SelectedFont != null) fileViewer.Font = SelectedFont;
            else
            if (defaultFont != null) fileViewer.Font = defaultFont;
            Application.DoEvents();

            Thread t = new Thread(IndexFile)
            {
                Name = "FileOpen",
                Priority = ThreadPriority.AboveNormal
            };
            t.Start();

            // Track status of the file load
            while (!bFileLoaded & !bManualStop)
            {
                toolStripLoadStatus.Text = string.Format("Reading {0}", _linecount);
                fileViewer.RowCount = _linecount;
                toolStripProgress.Value = ((FileProperties.FileLen / 100) == 0) ? 100 : Math.Min(Convert.ToInt32(FileProperties.BytesRead / (FileProperties.FileLen / 100)), 100);
                Application.DoEvents();
            }
            fileViewer.RowCount = _linecount;
            toolStripLoadStatus.Text = string.Format("{0}: {1} lines read", bManualStop ? "Stopped" : "Done", _linecount);
            if (bFileLoaded && !bManualStop) { toolStripProgress.Value = 0; toolStripProgress.Visible = false; }
            fileViewer.Refresh();
            Application.DoEvents();
            GC.Collect();
        }

        void FV_GetItem(object? sender, RetrieveItemEventArgs e)
        {
            if (bFileInvalid)
            {
                e.value = "Error - File has been Modified.";
                return;
            }
            if (e.displayMode == DisplayMode.Hex) { FV_GetHexItem(e); return; }

            LineIndex li = new();
            if (!idx.TryGetValue(e.line + 1, out li))
            {
                throw new Exception(string.Format("Invalid Item Index : {0}", e.line.ToString()));
            }

            FileStream fs;
            try
            {
                fs = File.Open(FileProperties.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                fs.Seek(li.pos, SeekOrigin.Begin);
                byte[] buff = new byte[li.len];
                fs.Read(buff, 0, buff.Length);
                fs.Close();
                e.value = Encoding.UTF8.GetString(buff);
            }
            catch(Exception ex)
            {
                ShowMessage(string.Format("Error Reading file.{0}{1}{0}{2}", Environment.NewLine, ex.Message, "Terminating Program."));
                Application.Exit();
            }
        }

        //  Translate the line to hex
        void FV_GetHexItem(RetrieveItemEventArgs e)
        {
            long position = e.line * (long)HexLineLen;
            FileStream fs = File.Open(FileProperties.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            fs.Seek(position, SeekOrigin.Begin);
            byte[] buff = new byte[HexLineLen];
            int bytesread = fs.Read(buff, 0, buff.Length);
            fs.Close();
            StringBuilder sb = new();
            sb.Append("0x").Append(Convert.ToString(long.Parse(position.ToString()), 16).PadLeft(8, '0'));
            sb.Append("   ");
            string HexString = Convert.ToHexString(buff, 0, bytesread);
            int i;
            for (i = 1; i <= HexString.Length; i++)
            {
                sb.Append(HexString[i - 1]);
                if (i % 8 == 0)
                {
                    sb.Append(" ");
                    if (i % 32 == 0) sb.Append(" ");
                }
            }
            sb.Append(new string(' ', (int)TotHexLineLen - sb.Length));

            for (int k = 0; k < bytesread; k++)
            {
                sb.Append(CharMap[buff[k]]);
            }
            e.value = sb.ToString();
        }
        #endregion



    }
}
