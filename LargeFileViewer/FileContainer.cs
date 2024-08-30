
using System.Collections.Concurrent;

using static LargeFileViewer.Common;


namespace LargeFileViewer
{
    /// <summary>
    /// Internal structure for the file being viewed. This is an index of each line
    /// and its position within the file.
    /// </summary>
    internal struct LineIndex
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
        public static string FileName => FileContainer._filename;
        public static long FileLen => FileContainer._filelen;
        public static FileEndingType FileEndings => FileContainer._fileEndingType;
        public static DateTime CreateDate => FileContainer._createdate;
        public static DateTime ModifiedDate => FileContainer._modifieddate;
        public static bool ReadOnly => FileContainer._readonly;
        public static bool Archive => FileContainer._archive;
        public static bool System => FileContainer._system;
        public static bool Compressed => FileContainer._compressed;
        public static bool Encrypted => FileContainer._encrypted;
        public static bool Hidden => FileContainer._hidden;
        public static int LineCount => FileContainer._linecount;
        public static long BytesRead => FileContainer._bytesread;
        public static string BaseFileName => Path.GetFileName(FileContainer._filename);
        public static string? FilePath => Path.GetDirectoryName(FileContainer._filename);
    }


    internal static class FileContainer
    {

        internal static string _filename = string.Empty;
        internal static long _filelen;
        internal static FileEndingType _fileEndingType;
        internal static int _preambleLength;
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
        internal static Dictionary<int, LineIndex> idx = [];
        internal static bool bManualStop;       // Used to stop loading a file
        internal static bool bFileLoaded;       // Set to true when the file is completely loaded.
        internal static bool bFileInvalid;      // Set to true if the file is changed while loaded in the viewer


        /// <summary>
        /// Clear current file properties. This is used when the file being viewed
        /// is closed.
        /// </summary>
        internal static void Clear()
        {
            _filename = string.Empty;
            _filelen = 0;
            _linecount = 0;
            _bytesread = 0;
            idx.Clear();
            GC.Collect();
        }

        public static int HexLineAtOffset(long offset, int linelen)
        {
            return (int)(offset / linelen);
        }

        public static int TextLineAtOffset(long offset)
        {
            return BinSearch(idx, offset) -1;
        }

        /// <summary>
        /// Binary Search of Dictionary
        /// </summary>
        /// <param name="a"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private static int BinSearch(Dictionary<int, LineIndex> idx, long offset)
        {
            int first = 1;
            int last = idx.Count;
            int mid = 0;
            while (first <= last)
            {
                mid = first + (last - first) / 2;
                if (offset == idx[mid].pos) return mid;
                if (offset > idx[mid].pos)
                    first = mid + 1;
                else
                    last = mid - 1;
            }
            return mid;
        }


        /// <summary>
        /// Returns the offset within the file of the line at LineNumber.
        /// </summary>
        /// <param name="LineNumber"></param>
        public static long GetHexOffset(int LineNumber, int linelen)
        {
            return (long)LineNumber * (long)linelen;
        }

        /// <summary>
        /// Returns the offset within the file of the line at LineNumber.
        /// </summary>
        /// <param name="LineNumber"></param>
        public static long GetOffset(int LineNumber)
        {
            if (LineNumber > idx.Count || LineNumber < 0) throw new ArgumentOutOfRangeException();
            return idx[LineNumber].pos;
        }

        /// <summary>
        /// Open a file and populate properties.
        /// </summary>
        /// <returns></returns>
        internal static bool Open()
        {
            string filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new())
            {
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;

                if (openFileDialog.ShowDialog() == DialogResult.OK) filePath = openFileDialog.FileName;
            }
            if (string.IsNullOrEmpty(filePath)) return false;
            return LoadFileInfo(filePath);
        }

        /// <summary>
        /// Populate the properties of the file being viewed.
        /// </summary>
        /// <param name="fName"></param>
        /// <returns></returns>
        public static bool LoadFileInfo(string fName)
        {
            FileInfo fi = new(fName);
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
            if (!GetFileEnding(fName, out _fileEndingType, out _preambleLength)) return false;
            return true;
        }

        /// <summary>
        /// Determine if the file is a Windows or Unix compatible text file
        /// and detect if a BOM is present.
        /// </summary>
        private static bool GetFileEnding(string fName, out FileEndingType fsType, out int preambleLen)
        {
            fsType = FileEndingType.unknown;
            string? line;
            preambleLen = 0;
            try
            {
                StreamReader sr = new(fName);
                line = sr.ReadLine();
                var preamble = sr.CurrentEncoding.GetPreamble();
                sr.Close();
                FileStream fs = File.Open(FileProperties.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] buff;
                if (preamble.Length > 0)
                {
                    buff = new byte[preamble.Length];
                    int pLen = fs.Read(buff, 0, buff.Length);
                    if (pLen == preamble.Length)
                    {
                        if (buff == preamble) preambleLen = preamble.Length;
                        if (buff.SequenceEqual(preamble)) preambleLen = preamble.Length;
                    }
                }
                fs.Seek(string.IsNullOrEmpty(line) ? 0 : line.Length + preambleLen, SeekOrigin.Begin);
                buff = new byte[2];
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
        /// Read the file being viewed and record the position of each line in the file.
        /// This is run as a background task.
        /// </summary>
        internal static void IndexFile1()
        {
            int linenum = 0;                
            _linecount = 0;                     // Do not remove
            long curPos = _preambleLength;
            string? line = string.Empty;
            int eCount = _fileEndingType == FileEndingType.windows ? 2 : 1;
            try
            {
                using (StreamReader sr = new(_filename))
                {
                    while ((line = sr.ReadLine()) != null && !bManualStop)
                    {
                        linenum++;
                        LineIndex li = new()
                        {
                            pos = curPos,
                            len = line.Length
                        };
                        idx.TryAdd(linenum, li);
                        curPos += line.Length + eCount;
                        _bytesread = curPos;
                        _linecount = linenum;           // Make sure this happens after TryAdd completes
                        if (bManualStop) break;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(string.Format("Unable to read the entire file.{0}Error: {1}", Environment.NewLine, ex.Message));
            }
            bFileLoaded = true;
        }

        /// <summary>
        /// Read the file being viewed and record the position of each line in the file.
        /// This is run as a background task.
        /// </summary>
        internal static void IndexFile()
        {
            int linenum = 0;
            _linecount = 0;                     // Do not remove
            _bytesread = 0;
            int buflines;
            int eCount = _fileEndingType == FileEndingType.windows ? 2 : 1;
            try
            {
                FileStream fs = File.Open(FileProperties.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] buff = new byte[1024 * 256];
                int pLen = fs.Read(buff, 0, buff.Length);
                int index = _preambleLength;
                while (pLen > 0)
                {
                    List<int> lines = LineCount(ref buff, pLen, _fileEndingType);
                    buflines = lines.Count;
                    for (int i = 0; i < buflines; i++)
                    {
                        LineIndex li = new()
                        {
                            pos = index + _bytesread,
                            len = lines.ElementAt(i) - index
                        };
                        linenum++;
                        index = lines.ElementAt(i) + eCount;
                        idx.TryAdd(linenum, li);
                        _linecount = linenum;           // Make sure this happens after TryAdd completes
                    }
                    _bytesread += pLen;
                    index = (pLen - lines.ElementAt(lines.Count - 1) - eCount) * -1;
                    pLen = fs.Read(buff, 0, buff.Length);
                }
                fs.Close();
            }
            catch (Exception ex)
            {
                ShowMessage(string.Format("Unable to read the entire file.{0}Error: {1}", Environment.NewLine, ex.Message));
            }
            bFileLoaded = true;
        }

        /// <summary>
        /// Run through the file buffer looking for newline characters.  In this case, 
        /// we are looking for x'0D' only which will accommodate both unix and windows.
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static List<int> LineCount(ref byte[] buff, int buflen, FileEndingType ft)
        {
            List<int> lines = [];
            int tgt = ft == FileEndingType.windows ? 13 : 10;
            for (int i = 0; i < buflen; i++) 
            { 
                if (buff[i] == tgt) lines.Add(i);
            }
            return lines;
        }
    }
}

