/**********************************************************************************************
 * 
 *  This class monitors the file being viewed and detects any changes made to that while while
 *  it is loaded in the Large File Viewer. If a change is detected, an event is fired to signal
 *  the Main thread to invalidate the ListView and ask the user to reload the file.
 *  
 *  Copyright 2023 © AJM Software L.L.C.
 **********************************************************************************************/

using static LargeFileViewer.Common;
using static LargeFileViewer.FileContainer;

namespace LargeFileViewer
{
    internal class FileMonitor
    {
        internal FileSystemWatcher? watcher;
        public event EventHandler? FileChanged;

        /// <summary>
        /// Start a FileWatcher on the file loaded in the ListView.
        /// </summary>
        public FileMonitor()
        {
            if (FileProperties.FilePath == null) { return; }
            watcher = new FileSystemWatcher(FileProperties.FilePath)
            {
                NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size
            };

            watcher.Changed += OnChanged;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            watcher.Filter = FileProperties.BaseFileName;
            watcher.IncludeSubdirectories = false;
            watcher.EnableRaisingEvents = true;
        }

        public void Close()
        {
            if (watcher == null) return;
            watcher.EnableRaisingEvents = false;
        }

        /// <summary>
        /// Fired when a change is detected in the file being viewed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed) return;
            StopFileLoad();
            MessageBox.Show(string.Format("{0} has changed. Reload it as needed,", FileProperties.FileName), PROGRAMNAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //if (dr == DialogResult.Yes)
            //{
            //    if (FileChanged == null) return;
            //    FileChanged.Invoke(null, EventArgs.Empty);
            //    return;
            //}
        }

        /// <summary>
        /// Fired when the file being viewed is deleted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            StopFileLoad();
            MessageBox.Show(string.Format("{0} has been deleted.", FileProperties.FileName), PROGRAMNAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Fired when the file being viewed is renamed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            StopFileLoad();
            MessageBox.Show(string.Format("{0} has been renamed to {1}.", FileProperties.FileName, e.Name), PROGRAMNAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Fired if the FileWatcher detects an error.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnError(object sender, ErrorEventArgs e)
        {
            StopFileLoad();
            Exception ex = e.GetException();
            MessageBox.Show(string.Format("Error: {0}.{1}Processing terminated.", ex.Message, Environment.NewLine), PROGRAMNAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// This routine will disable the FileWatcher and, if the file load is still in
        /// progress, signal the Main thread to stop the load. 
        /// </summary>
        private void StopFileLoad()
        {
            if (watcher != null) watcher.EnableRaisingEvents = false;
            bFileInvalid = true;
            if ((!bFileLoaded & !bManualStop)) bManualStop = true;
            FileChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
