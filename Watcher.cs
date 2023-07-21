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
        static long eventCounter;
        public Watcher(string path)
        {
            watcher = new FileSystemWatcher(path);
            _logPath = path;
            eventCounter = 0;

        }
        public void Watch()
        {
            Console.Title = $"Logging in {_logPath}...";
            // Adds the events to the FileSystemWatcher
            watcher.Created += OnFileCreated;
            
            watcher.Changed += OnFileChanged;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
            
        }
        private static void OnRenamed(object sender, RenamedEventArgs e) 
        {
            CountLog(LogString($"File renamed: {e.Name}  from  {e.OldName}"), ConsoleColor.Yellow);
        }
        private static void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            CountLog(LogString($"File created: {e.FullPath}"), ConsoleColor.Green);
        } 
        private static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            CountLog(LogString($"File changed: {e.FullPath}"), ConsoleColor.Cyan);
        }
        private static void OnError(object sender, ErrorEventArgs e) => PrintException(e.GetException()); 
        // Default log with number entry
        private static void CountLog(string content, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = ConsoleColor.White;
            eventCounter++;
            Console.Write($"#{eventCounter}\t ");
            Console.ForegroundColor = consoleColor;
            Console.Write(content + "\n");
        }

        // Returns the default DateTime and Log-Template String
        private static string LogString(string msg) => $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {msg}";

        private static void PrintException(Exception e)
        {
            if(e != null)
            {
                CountLog(LogString($"Error: {e.Message}"), ConsoleColor.Red);
                Console.ForegroundColor = ConsoleColor.Red;
                if(e.StackTrace != null) Console.WriteLine($"Stacktrace: {e.StackTrace}");
                Console.WriteLine();
                PrintException(e.InnerException);
            }
        }
    }
}
