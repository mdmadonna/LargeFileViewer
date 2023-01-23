/**********************************************************************************************
 * 
 *  This class contains miscellaneous common routines and variables used thrughout the Large
 *  File Viewer. 
 *  
 *  Copyright 2023 © AJM Software L.L.C.
 **********************************************************************************************/

using System.Collections.Concurrent;
using System.Diagnostics;

namespace LargeFileViewer
{
    /// <summary>
    /// Internal structure for the file being viewed. This is an index of each line
    /// and its position within the file.
    /// </summary>
    internal class LineIndex
    {
        internal long pos;
        internal int len;
    }

    /// <summary>
    /// Track type of file endings
    /// </summary>
    enum FileEndingType
    {
        windows,
        unix,
        unknown
    }

    /// <summary>
    /// Information related to the file being viewed
    /// </summary>
    internal static class FileProperties
    {
        public static string FileName => common._filename;
        public static long FileLen => common._filelen;
        public static FileEndingType FileEndings => common._fileEndingType;
        public static DateTime CreateDate => common._createdate;
        public static DateTime ModifiedDate => common._modifieddate;
        public static bool ReadOnly => common._readonly;
        public static bool Archive => common._archive;
        public static bool System => common._system;
        public static bool Compressed => common._compressed;
        public static bool Encrypted => common._encrypted;
        public static bool Hidden => common._hidden;
        public static int LineCount => common._linecount;
        public static long BytesRead => common._bytesread;
        public static string BaseFileName => Path.GetFileName(common._filename);
        public static string? FilePath => Path.GetDirectoryName(common._filename);
    }

    internal static class common
    {
        #region constants

        internal const string ABOUTFORM = "About";
        internal const string FEEDBACKFORM = "Feedback";
        internal const string FINDFORM = "Find";
        internal const string GOTOFORM = "GoTo";
        internal const string HELPFORM = "Help";
        internal const string LFVMUTEX = "LARGEFILEVIEWERMUTEX";
        internal const string LOGFILENAME = "error.log";
        internal const string OPTIONSFORM = "Options";
        internal const string OPTIONSFILENAME = "options.config";
        internal const string PROGRAMNAME = "Large File Viewer";
        internal const string PROPSFORM = "Props";
        #endregion

        internal static string _filename = string.Empty;
        internal static long _filelen;
        internal static FileEndingType _fileEndingType;
        internal static DateTime _createdate;
        internal static DateTime _modifieddate;
        internal static DateTime _accesseddate;
        internal static bool _readonly;
        internal static bool _archive;
        internal static bool _system;
        internal static bool _compressed;
        internal static bool _encrypted;
        internal static bool _hidden;
        internal static int _linecount;
        internal static long _bytesread;

        // Index of all lines within the file.
        internal static ConcurrentDictionary<long, LineIndex> idx = new();
        internal static bool bManualStop;       // Used to stop loading a file
        internal static bool bFileLoaded;       // Set to true when the file is completely loaded.
        internal static bool bFileInvalid;      // Set to true if the file is changed while loaded in the viewer

        internal static Font? SelectedFont;     // Selected Font for the ListView
        internal static Font? StartUpFont;      // Initial Font for the ListView

        /// <summary>
        /// Open a file a populate properties.
        /// </summary>
        /// <returns></returns>
        internal static bool OpenFile()
        {
            string filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;

                if (openFileDialog.ShowDialog() == DialogResult.OK) filePath = openFileDialog.FileName;
            }
            if (string.IsNullOrEmpty(filePath)) return false;
            return SetupFileProperties(filePath);
        }

        /// <summary>
        /// Populate the properties of the file being viewed.
        /// </summary>
        /// <param name="fName"></param>
        /// <returns></returns>
        internal static bool SetupFileProperties(string fName)
        {
            FileInfo fi = new FileInfo(fName);
            if (!fi.Exists) return false;
            _filename = fName;
            _filelen = fi.Length;
            _createdate = fi.CreationTime;
            _modifieddate = fi.LastWriteTime;
            _accesseddate = fi.LastAccessTime;
            _readonly = fi.IsReadOnly;
            _archive = (fi.Attributes & FileAttributes.Archive) == FileAttributes.Archive;
            _system = (fi.Attributes & FileAttributes.System) == FileAttributes.System;
            _compressed = (fi.Attributes & FileAttributes.Compressed) == FileAttributes.Compressed;
            _encrypted = (fi.Attributes & FileAttributes.Encrypted) == FileAttributes.Encrypted;
            _hidden = (fi.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
            _linecount = 0;
            _bytesread = 0;
            if (!GetFileEnding(fName, out _fileEndingType)) return false;
            return true;
        }

        /// <summary>
        /// Read the file being viewed and record the position of each line in the file.
        /// This is run as a background task.
        /// </summary>
        internal static bool GetFileEnding(string fName, out FileEndingType fsType)
        {
            fsType = FileEndingType.unknown;
            string? line = string.Empty;
            try
            {
                StreamReader sr = new StreamReader(_filename);
                line = sr.ReadLine();
                sr.Close();
                FileStream fs = File.Open(FileProperties.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                fs.Seek(string.IsNullOrEmpty(line) ? 0 : line.Length, SeekOrigin.Begin);
                byte[] buff = new byte[2];
                int bytecount = fs.Read(buff, 0, buff.Length);
                fs.Close();
                // if 0 or 1 byte is returned, we have an empty file or a file that is
                // 1 byte in length so it doesn't matter what the file endings are.
                if (bytecount != 2)
                {
                    fsType = FileEndingType.windows;
                }
                else
                {
                    // If we find an LFCR mark it as windows otherwise assume it's unix, mac or 
                    // old mac, all of which have 1 byte line endings.
                    if (buff[0] == 0x0D && buff[1] == 0x0A) { fsType = FileEndingType.windows; }
                    else { fsType = FileEndingType.unix; }
                }
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Cleaer current file properties. This is used when the file being viewed
        /// is closed.
        /// </summary>
        internal static void ClearCurrentFile()
        {
            _filename = string.Empty;
            _filelen = 0;
            _linecount = 0;
            _bytesread = 0;
            idx.Clear();
        }

        /// <summary>
        /// Read the file being viewed and record the position of each line in the file.
        /// This is run as a background task.
        /// </summary>
        internal static void FileIndex()
        {
            _linecount = 0;
            long curPos = 0;
            string? line = string.Empty;
            int eCount = _fileEndingType == FileEndingType.windows ? 2 : 1;

            StreamReader sr = new StreamReader(_filename);
            while ((line = sr.ReadLine()) != null && !bManualStop)
            {
                _linecount++;
                LineIndex li = new()
                {
                    pos = curPos,
                    len = line.Length
                };
                idx.TryAdd(_linecount, li);
                curPos += line.Length + eCount;
                _bytesread = curPos;
                if (bManualStop) break;
            }
            sr.Close();
            bFileLoaded = true;
        }

        /// <summary>
        /// Display the Select Font dialog. This is used to select a new Font
        /// for the ListView displaying the currently loaded file.
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        internal static bool GetFont(Font? font)
        {
            FontDialog fontDlg = new FontDialog();
            fontDlg.ShowColor = true;
            fontDlg.ShowApply = true;
            fontDlg.ShowEffects = true;
            fontDlg.ShowHelp = true;
            fontDlg.FontMustExist = true;
            if (font != null) fontDlg.Font = font;
            if (fontDlg.ShowDialog() == DialogResult.Cancel) return false;
            SelectedFont = fontDlg.Font;
            if (SelectedFont == font) return false;
            return true;
        }

        /// <summary>
        /// Start a new instance of the Large File Viewer
        /// </summary>
        internal static void StartApp()
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = Environment.ProcessPath;
            start.WindowStyle = ProcessWindowStyle.Normal;
            start.UseShellExecute = false;
            Process.Start(start);
        }

        /// <summary>
        /// Find a specific Form within the application.
        /// </summary>
        /// <param name="formName"></param>
        /// <returns>Form or null in the Form is not open/loaded.</returns>
        internal static Form? AppForm(string formName)
        {
            FormCollection forms = Application.OpenForms;
            foreach (Form frm in forms)
            {
                if (frm.Name == formName) return frm;
            }
            return null;
        }

        /// <summary>
        /// Generic routine to display a message.
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="messageBoxButtons">Option buttons to display with the message.</param>
        /// <param name="messageBoxIcon">Optional Icon to display with the message.</param>
        /// <returns></returns>
        internal static DialogResult ShowMessage(string message, MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK, MessageBoxIcon messageBoxIcon = MessageBoxIcon.Information)
        {
            DialogResult result = MessageBox.Show(message, PROGRAMNAME, messageBoxButtons, messageBoxIcon);
            return result;
        }
    }
}
