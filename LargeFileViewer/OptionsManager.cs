/**********************************************************************************************
 * 
 *  Class to manage the configuration file containing default options. A future consideration 
 *  will be to add a FileWatcher to monitor changes made by multiple instances of the Large
 *  File Viewer and reload options when changes are detected.
 *  
 *  Copyright 2023 © AJM Software L.L.C.
 **********************************************************************************************/

using System.Text.Json;

using static LargeFileViewer.common;
using static System.Environment;

namespace LargeFileViewer
{
    internal static class OptionsManager
    {
        /// <summary>
        /// Class describing options data contained the the configuration file.  New options
        /// should be added here.
        /// </summary>
        public class OptionsData
        {
            public int MaxFiles { get; set; }
            public string? FontName { get; set; }
            public string? FontSize { get; set; }
            public string? FontStyle { get; set; }
            public List<string>? MRUFiles { get; set; }

            public OptionsData()
            {
                MaxFiles = 5;       //default to 5 files in Most Recently Used list
            }
        }

        // Instance containing current options data.
        public static OptionsData optionsdata = new();
        // Mutex used to obtain a system wide lock on the options file during
        // read/write operations.
        private static Mutex mutex = new Mutex(false, LFVMUTEX);
        static string optfilename = string.Empty;
        public static Font? defaultFont { get; set; }

        /// <summary>
        /// Load the options file or create it id it doesn't exist.
        /// </summary>
        internal static void Initialize()
        {
            // The options file is in the ProgramData directory
            var commonpath = GetFolderPath(SpecialFolder.CommonApplicationData);
            string dir = Path.Combine(commonpath, "ajmsoft\\LargeFileViewer");
            optfilename = Path.Combine(dir, OPTIONSFILENAME);
            if (!File.Exists(optfilename))
            {
                try
                {
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    StreamWriter sw = new StreamWriter(optfilename);
                    sw.Close();
                }
                catch (Exception ex)
                {
                    ShowMessage("Cannot create Options File. Program Options will not be saved." + Environment.NewLine + ex.Message);
                }
                return;
            }
            string optdata = string.Empty;
            if (mutex.WaitOne(1000))
            {
                try
                {
                    optdata = File.ReadAllText(optfilename);
                }
                catch { }
                finally { mutex.ReleaseMutex(); }
            }
            if (string.IsNullOrEmpty(optdata)) return;

            object? od = JsonSerializer.Deserialize<OptionsData>(optdata);
            if (od == null) return;
            optionsdata = (OptionsData)od;

            // Set up the default font. Ignore any errors.
            try
            {
                Single fontSize = 0;
                Single.TryParse(optionsdata.FontSize, out fontSize);
                if (fontSize == 0) fontSize = 9;
                string fontStyle = string.IsNullOrEmpty(optionsdata.FontStyle) ? "Regular" : optionsdata.FontStyle;
                int value = (int)Enum.Parse(typeof(FontStyle), fontStyle);
                string fontName = string.IsNullOrEmpty(optionsdata.FontName) ? string.Empty : optionsdata.FontName;
                if (!string.IsNullOrEmpty(fontName))
                {
                    defaultFont = new Font(fontName, fontSize, (FontStyle)value);
                }
            }
            catch { }
        }

        /// <summary>
        /// Write the options file to disk.
        /// </summary>
        /// <returns></returns>
        internal static bool WriteOptions()
        {
            optionsdata.FontName = defaultFont == null ? null : defaultFont.Name;
            optionsdata.FontSize = defaultFont == null ? null : defaultFont.Size.ToString();
            optionsdata.FontStyle = defaultFont == null ? null : defaultFont.Style.ToString();

            string optdata = JsonSerializer.Serialize<OptionsData>(optionsdata);
            if (mutex.WaitOne(1000))
            {
                try
                {
                    File.WriteAllText(optfilename, optdata);
                    return true;
                }
                catch { }
                finally { mutex.ReleaseMutex(); }
            }
            return false;
        }

        /// <summary>
        /// Add a file to the MRU list. Drop the oldest recently use file if the number
        /// of files in the list exceeds max.
        /// </summary>
        /// <param name="filename"></param>
        internal static void Add(string filename)
        {
            if (optionsdata.MRUFiles == null) optionsdata.MRUFiles = new();
            for (int i = 0; i < optionsdata.MRUFiles.Count; i++)
            {
                if (optionsdata.MRUFiles[i] == filename)
                {
                    optionsdata.MRUFiles.RemoveAt(i);
                    break;
                }
            }
            int j = optionsdata.MRUFiles.Count;
            if (j >= optionsdata.MaxFiles) while (j >= optionsdata.MaxFiles) { optionsdata.MRUFiles.RemoveAt(0); j--; }
            optionsdata.MRUFiles.Add(filename);
            WriteOptions();
        }
    }
}
