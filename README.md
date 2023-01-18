# LargeFileViewer

The Large File Viewer will allow you to view very large files using a minimum amount of memory. The program is designed specifically to run on Windows and view standard Windows text files but could be easily adapted to accommodate Unix/MacOS text files as well. It was originally designed to view very large log files, database dumps, etc. We use it to view text files up to 50-60GB.

Key points regarding the program:

1. The file being viewed is not stored in memory.
2. The program will start displaying the file immediately.
3. The entire file must be read before you can view all of it but you can navigate to any record or line that has been processed(read).
4. Performance is dependent primarily on disk throughput. On a modest machine it takes 30-40 seconds to process a 10GB file on a standard HDD. SSDs are faster.
5. The program does maintain a list of addresses for each line so a small amount of memory is used per line.
6. Searching requires that the entire file be read to find the data you're looking for.
7. The program does not currently have update capabilities.
