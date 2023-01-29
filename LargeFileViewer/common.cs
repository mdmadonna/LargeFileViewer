/**********************************************************************************************
 * 
 *  This class contains miscellaneous common routines and variables used thrughout the Large
 *  File Viewer. 
 *  
 *  Copyright 2023 © AJM Software L.L.C.
 **********************************************************************************************/

using System.Collections.Concurrent;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace LargeFileViewer
{
    
    internal static class common
    {
        #region constants

        internal const string ABOUTFORM = "AboutBox";
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
        internal const string UTILFORM = "Stats";
        #endregion


        internal static Font? SelectedFont { get; set; }     // Selected Font for the ListView
        internal static Font? StartUpFont { get; set; }      // Initial Font for the ListView
        internal static string[] CharMap { get; private set; }

        /// <summary>
        /// Static Constructor
        /// </summary>
        static common()
        {
            // Create a character map used to display characters in the hex version
            // of the listview
            CharMap = new string[256];
            for (int i = 0; i < 256; i++)
            {
                if (i < 128)
                {
                    byte b = Convert.ToByte(i);
                    CharMap[i] = Char.IsLetterOrDigit((char)i) || Char.IsPunctuation((char)i) || Char.IsSeparator((char)i) || Char.IsSymbol((char)i) ? Char.ConvertFromUtf32(i) : ".";
                }
                else { CharMap[i] = "."; }
            }

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
