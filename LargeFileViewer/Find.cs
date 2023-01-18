/**********************************************************************************************
 * 
 *  This Form is used to find string literals contained in the file being viewed.
 *  
 *  Copyright 2023 © AJM Software L.L.C.
 **********************************************************************************************/

using System.Collections;
using System.Text;

using static LargeFileViewer.common;

namespace LargeFileViewer
{
    public partial class Find : Form
    {
        public bool bSearching { get; set; }
        public bool bCancelSearch { get; set; }
        public bool bFormClosing { get; set; }

        /// <summary>
        /// Array of line numbers containg the string being found. flist is the underlying data 
        /// store and FoundList is a synchronized wrapper used to access flist in a thread
        /// safe manner. All accesses to this array should be made through FoundList. flist
        /// should never be accessed directly.
        /// 
        /// Currently, the search feature is not multi-threaded so a synchronized is not really
        /// needed.  A multi-threaded search is an option under consideration so the synchronized 
        /// wrapper will continue to be used.
        /// </summary>
        ArrayList flist;
        public ArrayList FoundList;

        /// <summary>
        /// This event is fired when the first match is found. It signals the Main thread to 
        /// make sure the line with the found text is visible and highlighted. It is also
        /// fired for Find Next or Find Previous.
        /// </summary>
        public event EventHandler<FoundEventArgs>? Found;
        public class FoundEventArgs : EventArgs
        {
            public int newline;
        }

        // The currently displayed and highlighted line containing the found text
        public int FoundPos { get; set; }

        // The text we're looking for
        string searchText = string.Empty;
        bool bMatchCase;
        int searchcount = 0;

        /// <summary>
        /// Constructor to initialize variables used in the search.
        /// </summary>
        public Find()
        {
            InitializeComponent();
            bFormClosing = false;
            flist = new();
            FoundList = ArrayList.Synchronized(flist);
            FoundPos = 0;
        }

        private void Find_Load(object sender, EventArgs e)
        {
            toolStripStatus.Text = string.Empty;
            toolStripResults.Text = string.Empty;
            toolStripPosition.Text = string.Empty;
            toolStripProgress.Visible = false;
            lblMsg.Text = string.Empty;
        }
        private void Find_FormClosing(object sender, FormClosingEventArgs e)
        {
            bCancelSearch = true;
            bSearching = false;
            bFormClosing = true;
        }

        /// <summary>
        /// If a search is underway or complete, move to the next matched line, otherwise 
        /// start searching. If the search criteria has changed, cancel the current search
        /// a start a new search.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text == string.Empty)
            {
                lblMsg.Text = "Pleae enter a Search string.";
                txtSearch.Focus();
                return;
            }

            if (cbMatchCase.Checked != bMatchCase || searchText != (bMatchCase ? txtSearch.Text : txtSearch.Text.ToLower()))
            {
                StartFind();
                return;
            }
            if (FoundPos >= FoundList.Count)
            {
                lblMsg.Text = string.Format("No more items found. {0}", bSearching ? "Still Looking..." : "Reached end of file.");
                return;
            }
            FoundPos++;
            FireFound(FoundPos);
        }

        /// <summary>
        /// Move the ListView to the previous match.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (FoundPos <= 1)
            {
                lblMsg.Text = "No more items found. Reached start of file,";
                return;
            }
            FoundPos--;
            FireFound(FoundPos);
        }

        /// <summary>
        /// Cancel any ongoing search and execute a short wait.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // The Cancel button should only enabled while a search is underway so the
            // if stmt is here only for clarity.
            if (bSearching)
            {
                bCancelSearch = true;
                Thread.Sleep(500);          // Future: Change this wait for the Task to complete.
            }
        }

        public enum SearchDirection
        {
            Forward,
            Backward
        }

        /// <summary>
        /// Simulate a Forward or Previous click to move to the next
        /// Found item.
        /// </summary>
        /// <param name="sd"></param>
        public void MoveFinder(SearchDirection sd)
        {
            object sender = new();
            EventArgs e = new();
            if (sd == SearchDirection.Forward)
            { btnFind_Click(sender, e); }
            else
            { btnPrevious_Click(sender, e); }
        }

        /// <summary>
        /// Fire this event to move the selected item in the ListView
        /// to the next (or previous) match.
        /// </summary>
        /// <param name="pos"></param>
        private void FireFound(int pos)
        {
            if (Found == null) return;
            int fCount = FoundList == null ? 0 : FoundList.Count;

            // Use this next stmt only if I implement multithreaded searching
            // otherwise the list is already sorted.
            //if (FoundList != null) if (!bSearching) FoundList.Sort();

            lblMsg.Text = string.Empty;
            var v = (FoundList != null) ? FoundList[pos - 1] : 0;
            if (FoundPos != 0) toolStripPosition.Text = string.Format("{0} of {1}", FoundPos, fCount.ToString());
            FoundEventArgs fargs = new();
            fargs.newline = (v != null) ? (int)v : 0;
            Found.Invoke(null, fargs);
        }

        /// <summary>
        /// Start a background task to search the file for matches then monitor 
        /// it until completion
        /// </summary>
        private void StartFind()
        {
            bMatchCase = cbMatchCase.Checked;
            bSearching = true;
            bCancelSearch = false;
            searchText = bMatchCase ? txtSearch.Text : txtSearch.Text.ToLower();
            toolStripProgress.Visible = true;
            toolStripPosition.Text = string.Empty;
            toolStripResults.Text = string.Empty;
            btnCancel.Enabled = true;
            FoundPos = 0;
            FoundList.Clear();
            searchcount = 0;
            int matchCount = 0;

            Thread t = new Thread(SearchTask)
            {
                Name = "FileSearch",
                Priority = ThreadPriority.Normal
            };
            t.Start();
            Application.DoEvents();
            while (bSearching & !bCancelSearch)
            {
                toolStripStatus.Text = string.Format("{0} of {1}", searchcount, idx.Count);
                matchCount = (FoundList != null) ? FoundList.Count : 0;
                if (matchCount != 0) toolStripResults.Text = string.Format("{0} matches", matchCount.ToString());
                toolStripProgress.Value = Convert.ToInt32((searchcount * 100) / FileProperties.LineCount);
                if (FoundPos != 0) toolStripPosition.Text = string.Format("{0} of {1}", FoundPos, matchCount.ToString());
                if (FoundPos == 0 && matchCount > 0)
                {
                    if (Found != null)
                    {
                        FoundPos = 1;
                        FireFound(FoundPos);
                    }
                }
                Application.DoEvents();
            }
            if (bFormClosing) return;
            btnCancel.Enabled = false;
            toolStripProgress.Value = 0;
            toolStripProgress.Visible = false;
            toolStripStatus.Text = "Search Complete";
            matchCount = (FoundList != null) ? FoundList.Count : 0;
            toolStripResults.Text = string.Format("{0} matches", matchCount.ToString());
            if (matchCount > 0)
            {
                if (FoundPos == 0) FoundPos = 1;
                FireFound(FoundPos);
            }
        }

        /// <summary>
        /// This is the background task that will read the file loaded in the viewer and search 
        /// for the text we want to find.
        /// </summary>
        private void SearchTask()
        {
            long StartPos = 0;
            long EndPos = FileProperties.LineCount;
            int startidx = 1;
            FoundList.Clear();

            FileStream fs = File.Open(FileProperties.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            fs.Seek(StartPos, SeekOrigin.Begin);
            int i = startidx;
            do
            {
                LineIndex li = idx[i];
                byte[] buff = new byte[li.len + 2];
                fs.Read(buff, 0, buff.Length);
                string line = Encoding.UTF8.GetString(buff);
                if (!bMatchCase) line = line.ToLower();
                if (line.IndexOf(searchText) >= 0) FoundList.Add(i);
                if (i >= EndPos || bCancelSearch) break;
                i++;
                searchcount++;
                Application.DoEvents();
            } while (true);
            bSearching = false;
            fs.Close();
        }

        /// <summary>
        /// Enable F3 and Alt-F3 to execute Find Next ans Find Previous.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            KeyEventArgs e = new KeyEventArgs(keyData);
            if (e.KeyCode == Keys.F3)
            {
                object sender = new();
                if (e.Shift)
                { btnPrevious_Click(sender, e); }
                else
                { btnFind_Click(sender, e); }
                return true; // handled
            }
            return base.ProcessCmdKey(ref msg, keyData);

        }

    }
}
