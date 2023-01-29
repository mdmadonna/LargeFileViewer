/**********************************************************************************************
 * 
 *  This class is a User Control for displaying file content for the Large File Viewer. It is
 *  based off of a RichTextBox.
 *  
 *  Copyright 2023 © AJM Software L.L.C.
 **********************************************************************************************/

using System.Text;
using System.Windows.Forms;

namespace LargeFileViewer
{
    /// <summary>
    /// Track current Dsiplay Mode (Hex vs Text)
    /// </summary>
    public enum DisplayMode
    {
        Text,
        Hex
    }

    public partial class FileViewer : UserControl
    {
        #region Events

        /// <summary>
        /// Fired when 'Clear' is selected from the Context Menu.
        /// </summary>
        public event EventHandler<EventArgs>? Cleared;

        /// <summary>
        /// Fired when the File Viewer needs a line of test to display.
        /// </summary>
        public event EventHandler<RetrieveItemEventArgs>? RetrieveItem;
        public class RetrieveItemEventArgs : EventArgs
        {
            public int line { get; internal set; }
            public string value { get; set; } = string.Empty;
            public DisplayMode displayMode { get; set; }
        }

        /// <summary>
        /// Fired when the cursor position in the File Viewer changes.
        /// </summary>
        public event EventHandler<SelectedPositionChangeEventArgs>? SelectedPositionChanged;
        public class SelectedPositionChangeEventArgs : EventArgs
        {
            public int Line { get; internal set; } = 0;
            public int Position { get; set; } = 0;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Current display mode - either hex or text(character)
        /// </summary>
        public DisplayMode displayMode { get { return _displaymode; } set { SetupDisplayMode(value); } }

        /// <summary>
        /// Number of text rows(lines) in the file being displayed.
        /// </summary>
        public int RowCount { get { return _rowcount; } set { UpdateRowCount(value); } }

        /// <summary>
        /// Number of rows(lines) available in hex mode. This is the equivalent of
        /// file size / HexLineLen. Add 1 if file size is not divisible by HexLineLen.
        /// </summary>
        public int HexRowCount { get { return _hexrowcount; } }

        /// <summary>
        /// Size, in bytes, of the file being viewed.
        /// </summary>
        public long FileSize { get { return _filesize; }  set { _filesize = value;  _hexrowcount = (int)(value / HexLineLen) + (value % HexLineLen == 0 ? 0 : 1); } }

        /// <summary>
        /// Current cursor position in the viewer.
        /// </summary>
        public int SelectedPosition { get { return textViewer.SelectionStart; }  set { SetPosition(value); }  }

        /// <summary>
        /// Get Selected Text from the File Viewer.
        /// </summary>
        public string SelectedText { get { return textViewer.Text.Substring(textViewer.SelectionStart, textViewer.SelectionLength); } }
        
        /// <summary>
        /// Current number of visible rows
        /// </summary>
        public int LineCount { get { return _linecount; } }

        /// <summary>
        /// Map of characters displayed in the text area of the hex display.
        /// </summary>
        public string[] CharMap { get; set; }

        // Number of Hex Characers to display
        public static int HexLineLen { get; private set; }
        // Length of line displayed in hex
        public static int TotHexLineLen { get; private set; }
        #endregion

        // internal variables
        private int _rowcount = 0;
        private int _hexrowcount;
        private int _linecount;
        // total number of rows that can be displayed. This is equal to _rowcount or _hexrowcount
        // depending on display mode.
        private int _maxrows;
        private long _filesize;
        private DisplayMode _displaymode;

        // Kluge to determine number of lines that can be currently displayed in the file
        // viewer
        private RichTextBox dummyRTB = new();

        /// <summary>
        /// Constructor.
        /// </summary>
        public FileViewer()
        {
            InitializeComponent();
            textViewer.MaxLength = int.MaxValue;
            textViewer.ReadOnly = true;
            textViewer.WordWrap= false;
            textViewer.ScrollBars = RichTextBoxScrollBars.Horizontal;
            textViewer.MouseWheel += text_MouseWheel;
            
            lineViewer.MaxLength = int.MaxValue;
            lineViewer.ReadOnly = true;
            lineViewer.WordWrap = false;
            lineViewer.TabStop = false;
            lineViewer.ScrollBars = RichTextBoxScrollBars.None;
            lineViewer.MouseWheel += text_MouseWheel;

            vScrollBar.Visible = false;

            CharMap = new string[] { };
            
            // Set up parms for Hex Display. Everything keys off of HexLineLen. If thie 
            // becomes an updateable option, make sure it is divisible by 16.
            HexLineLen = 64;
            // ToxHexLineLen is the total # of hex characters displayed on a line before the text area is shown
            TotHexLineLen = HexLineLen * 2;     // 2 characters displayed for each byte in the file
            TotHexLineLen += 14;                // Add the file offset length (10) = 4 spaces that follow
            TotHexLineLen += HexLineLen / 4;    // We put a space after each group of 4 bytes so add 1 for each
            TotHexLineLen += HexLineLen / 16;   // We put a space after each group of 16 bytes so add 1 for each
            TotHexLineLen += 6;

            displayMode = DisplayMode.Text;
            textViewer.Text = string.Empty;
            BuildDummyRTB();
        }

        #region Internal Routines
        /// <summary>
        /// Determine the number of rows that can be currently displayed
        /// </summary>
        private int GetClientAreaRows()
        {
            //***********************************************************
            //
            // Could not find an accurate wat of computing the total number 
            // of lines visible in the File Viewer. No combination of logic
            // using font size or height would come up with a correct answer
            // so this kluge was developed.  Hopefully I can find a better
            // way in the future.
            //
            //***********************************************************

            dummyRTB.Font = textViewer.Font;
            dummyRTB.Height = textViewer.Height; 
            dummyRTB.Width = textViewer.Width;
            dummyRTB.ScrollBars = (textViewer.Height != textViewer.ClientSize.Height + 4) ? RichTextBoxScrollBars.ForcedHorizontal : RichTextBoxScrollBars.None;

            int lastLineIndex = dummyRTB.GetCharIndexFromPosition(new Point(1, dummyRTB.Height - 1));
            int lastLine = dummyRTB.GetLineFromCharIndex(lastLineIndex);
            return lastLine - 1;
        }

        /// <summary>
        /// This is a kluge that appears to be the only way to correctly determine how many lines
        /// can be currently displayed in the RTB File Viewer.  I need to create an RTB that 
        /// mirrors the characteristics of the File Viewer and populate it with many lines of text.
        /// I can then fimd the line that appears at the bottom of the RTB. 
        /// </summary>
        private void BuildDummyRTB()
        {
            dummyRTB.WordWrap= false;

            StringBuilder sb = new();
            sb.Append("A").AppendLine();
            int i = 0;
            while (true)
            {
                i++;
                sb.Append("A").AppendLine();
                if (i > 200) break;
            }
            dummyRTB.Text = sb.ToString();
        }

        /// <summary>
        /// This value is updated as the file is read. It represents the number of lines in the 
        /// file and limits what can be displayed in the File Viewer.
        /// </summary>
        /// <param name="rowcount"></param>
        private void UpdateRowCount(int rowcount)
        {
            _rowcount = rowcount;
            if (displayMode == DisplayMode.Text)
            {
                vScrollBar.Maximum = _rowcount;
                _maxrows = rowcount;
                if (textViewer.Lines.Count() < _linecount) RedrawView(vScrollBar.Value);
            }
        }

        /// <summary>
        /// Small amount of setup work needed to switch from textdisplay to hex
        /// display and vice-versa
        /// </summary>
        /// <param name="mode"></param>
        private void SetupDisplayMode(DisplayMode mode)
        {
            _maxrows = mode == DisplayMode.Text ? _rowcount : _hexrowcount;
            vScrollBar.Maximum = _maxrows;
            _displaymode = mode;
        }

        /// <summary>
        /// This routine will redraw the File Viewer.
        /// </summary>
        /// <param name="linenum">The Line number of the row that appears at the top
        /// of the File Viewer.</param>
        private void RedrawView(int linenum)
        {
            // If a Retrieve Event consumer has not yet been defined, just
            // return.
            if (RetrieveItem == null) return;

            // If the file has not yet been read (text display mode only)
            // just return.
            if (_maxrows == 0) { return; }

            // If the number of lines in the files exceed the number of lines
            // that can displayed in the File Viewer, display the Vertical
            // ScrollBar and define the number of rows to move when PgDn or PgUp
            // is entered.
            vScrollBar.Visible = _maxrows > LineCount;
            vScrollBar.LargeChange = Math.Max(LineCount - 1, 1);

            // Retrieve the lines to be displayed and place them into a StringBuilder
            // followed by a NewLine sequence.  After we retrieve enough lines to
            // fill the visible area of the FileViewer, populate the Viewer with the
            // data we retrieved.
            RetrieveItemEventArgs e = new RetrieveItemEventArgs();
            e.displayMode = _displaymode;
            StringBuilder sb = new();
            StringBuilder lsb = new();
            for (int i = 0; i < LineCount; i++)
            {
                if (i + linenum >= _maxrows) break;
                e.line = linenum + i;
                RetrieveItem.Invoke(this, e);
                sb.Append(e.value).AppendLine();
                lsb.Append(e.line + 1).AppendLine();
            }
            textViewer.Text = sb.ToString();
            lineViewer.Text = lsb.ToString();
        }

        /// <summary>
        /// Move the cursor to a specific location in the File Viewer.
        /// </summary>
        /// <param name="position">Location of the cursor.</param>
        private void SetPosition(int position)
        {
            if (position < 0) return;
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Clear the File Viewer.
        /// </summary>
        public void Clear()
        {
            RowCount= 0;
            textViewer.Clear();
            lineViewer.Clear();
            _filesize= 0;
            RedrawView(0);
        }

        /// <summary>
        /// Determine if the line number id currently visible in the File Viewer.
        /// </summary>
        /// <param name="LineNumber"></param>
        public bool IsVisible(int linenum)
        {
            return (linenum >= vScrollBar.Value && linenum < vScrollBar.Value + LineCount);
        }

        /// <summary>
        /// Move the text/hex at the LineNumber to the top of the File Viewer.
        /// </summary>
        /// <param name="LineNumber">Line Number to place at the top of the File Viewer.</param>
        public void EnsureVisible(int linenum)
        {
            vScrollBar.Value = linenum;
        }

        /// <summary>
        /// Repaint the data currently cisible in the File Viewer.
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            RedrawView(vScrollBar.Value);
        }

        #endregion

        #region User Interface
        /// <summary>
        /// Fired when the value of the vertical scrollbar changes. The line at the
        /// current value of the scrollbar will be moved to the top of the File
        /// Viewer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vScrollBar_ValueChanged(object sender, EventArgs e)
        {
            RedrawView(vScrollBar.Value);
        }

        /// <summary>
        /// Copy Selected Text to the clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SelectedText)) Clipboard.SetText(SelectedText);
        }

        /// <summary>
        /// Clear the FileViewer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear();
            if (Cleared != null) Cleared.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Override key input at the Form level to ensure the File Viewer
        /// responds correctly to PgUp, PgDn, Ctrl-Home and Ctrl-End.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_rowcount > 0)
            {
                KeyEventArgs e = new KeyEventArgs(keyData);

                switch (e.KeyCode)
                {
                    case Keys.PageDown:
                        vScrollBar.Value = Math.Min(vScrollBar.Value + vScrollBar.LargeChange, vScrollBar.Maximum-1);
                        return true;
                    case Keys.PageUp:
                        vScrollBar.Value = Math.Max(vScrollBar.Value - vScrollBar.LargeChange, vScrollBar.Minimum);
                        return true;
                    case Keys.End:
                        if (e.Control)
                        {
                            vScrollBar.Value = vScrollBar.Maximum - 1;
                            return true;
                        }
                        break;
                    case Keys.Home:
                        if (e.Control)
                        {
                            vScrollBar.Value = vScrollBar.Minimum;
                            return true;
                        }
                        break;
                    default:
                        break;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Font changes may affect the size of the visible client area of the File
        /// Viewer. Force the clent area to be repainted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileViewer_FontChanged(object sender, EventArgs e)
        {
            textViewer.Font = this.Font;
            FileViewer_Resize(this, EventArgs.Empty);
        }

        /// <summary>
        /// Resizing the form may affect the size of the visible client area of the File
        /// Viewer. Force the clent area to be repainted if the number of visible lines
        /// has changed..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileViewer_Resize(object sender, EventArgs e)
        {
            int lines = GetClientAreaRows();
            if (lines != _linecount)
            {
                _linecount = lines;
                RedrawView(vScrollBar.Value);
            }
        }

        /// <summary>
        /// Any changes to the size of the visible client area of the File
        /// Viewer will force the clent area to be repainted. (i.e. appearance
        /// of scroll bars)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textViewer_ClientSizeChanged(object sender, EventArgs e)
        {
            FileViewer_Resize(sender, e);
        }

        /// <summary>
        /// Any action on the mouse wheel woill cause the File Viewer to move up or 
        /// down as needed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void text_MouseWheel(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            int linecount = (e.Delta * SystemInformation.MouseWheelScrollLines / 120) * -1;
            switch (linecount)
            {
                case < 0:
                    vScrollBar.Value = Math.Max(vScrollBar.Value + linecount, vScrollBar.Minimum);
                    break;
                default:
                    vScrollBar.Value = Math.Min(vScrollBar.Value + linecount, vScrollBar.Maximum - 1);
                    break;
            }
        }

        /// <summary>
        /// If the cursor position changes, fire an event with the new coordinates
        /// of the cursor position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textViewer_SelectionChanged(object sender, EventArgs e)
        {
            if (SelectedPositionChanged == null) return;

            RichTextBox rtb = (RichTextBox)sender;
            int index = rtb.SelectionStart;
            int line = rtb.GetLineFromCharIndex(index);
            int idx = rtb.GetFirstCharIndexFromLine(line);
            SelectedPositionChangeEventArgs se = new();
            se.Line = line + vScrollBar.Value; 
            se.Position= index - idx;
            SelectedPositionChanged.Invoke(sender, se);
        }

        /// <summary>
        /// Trap the Up and Down arrows to provide scrolling functionality.  The Down 
        /// Arrow does appear to work correctly as it leaves the bottom lin blank.
        /// Attempts to fix this (i.e. remove rtb.Select) results in the line being
        /// displayed correctly but the cursor moves to the top of the File Viewer. 
        /// Up Arrow works fine.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textViewer_KeyDown(object sender, KeyEventArgs e)
        {
            RichTextBox rtb = (RichTextBox)sender;
            int index = rtb.SelectionStart;
            int line = rtb.GetLineFromCharIndex(index);

            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (line >= _linecount - 1)
                    { 
                        if (vScrollBar.Value <= (_maxrows - _linecount))
                        {
                            vScrollBar.Value++;
                            int idx = rtb.GetFirstCharIndexFromLine(LineCount - 1);
                            rtb.Select(idx, 0);
                        }
                        e.Handled = true;
                    }

                    break;
                case Keys.Up:
                    if (line == 0 && vScrollBar.Value > 0)
                    {
                        vScrollBar.Value--;
                        e.Handled = true;
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion


    }
}
