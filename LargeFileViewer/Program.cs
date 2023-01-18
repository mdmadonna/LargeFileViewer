/**********************************************************************************************
 * 
 *  Startup for the Large File Viewer.
 *  
 *  Copyright 2023 © AJM Software L.L.C.
 **********************************************************************************************/

namespace LargeFileViewer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Main());
        }
    }
}