using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Overwatch
{
    internal class Watcher
    {
        private string _logPath;
        public string LogPath
        {
            get { return _logPath; }
            set { _logPath = value; }
        }
        FileSystemWatcher watcher;
        static Thread thread;
        public Watcher(string path)
        {
            watcher = new FileSystemWatcher(path);
        }
        public void Watch()
        { 
            // Fügt die Events hinzu
            watcher.Created += OnFileCreated;

            watcher.Changed += OnFileChanged;

            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
            
        }
        private static void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] " + $"File created: {e.FullPath}");
        }
        private static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            // Thread fileInfoThread = new Thread(new ThreadStart(OnFileCreated));
            Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] " + $"File changed: {e.FullPath}");
            // FileInfo fileInfo = new FileInfo(e.FullPath);
            // long oldSize = fileInfo.Length;

            FileInfo fileInfo1 = new FileInfo(e.FullPath);

        }
    }
}
