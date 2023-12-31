﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Win32;
using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace Overwatch
{
    internal class Watcher
    {
        static int totalCreations;
        static int totalRenamings;
        static int totalChanges;
        static int totalDeletions;
        static int totalErrors;

        private static bool _logInDB;
        public static bool LogInDB
        {
            get { return _logInDB; }
            set { _logInDB = value; }
        }

        private static string _logPath;
        public static string LogPath
        {
            get { return _logPath; }
            set { _logPath = value; }
        }

        FileSystemWatcher watcher;
        // static Thread afkThread;
        static long eventCounter;

        // DLLImport for getting the event if the application is closed
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        // DLLImports for hiding the console
        [DllImport("Kernel32")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // 0 for minimizing
        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;
        static DateTime startTime;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        public Watcher(string path)
        {
            watcher = new FileSystemWatcher(path);
            _logPath = path;
            _logInDB = Configurator.GetBool("Log in DB");
            eventCounter = 0;

            totalCreations = 0;
            totalRenamings = 0;
            totalChanges = 0;
            totalDeletions = 0;
            totalErrors = 0;
        }


        private static bool Handler(CtrlType sig)
        {
            WriteSessionInfoToFile(systemEvent:false, null);

            switch (sig)
            {
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                default:
                    return false;
            }
        }


        // Write the tracked time to a file and check if the user closed the application or if the PC was shut down
        private static void WriteSessionInfoToFile(bool systemEvent, SessionEndedEventArgs e)
        {

            // Calculating the duration of the length of the programs use
            TimeSpan duration = DateTime.Now - startTime;


            // Adds the AFK durations to a string
            string afkDurations = "";
            for (int i = 0; i < AFKTracker.afkStartTimes.Count; i++)
            {
                afkDurations += $"AFK #{i} started at {AFKTracker.afkStartTimes[i]}\n" +
                    $"Duration: {AFKTracker.durations[i]}\n";
            }

            // Calculating all the time the user was AFK
            TimeSpan totalAfkTimeSpan = new TimeSpan();
            for (int i = 0; i < AFKTracker.durations.Count; i++)
            {
                totalAfkTimeSpan += AFKTracker.durations[i];
            }

            // The whole time the user was active on the PC
            TimeSpan totalActiveTimeSpan = duration - totalAfkTimeSpan;

            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            keyValues["SessionStartTime"] = startTime.ToString().Replace('.', '-');
            keyValues["SessionEndTime"] = DateTime.Now.ToString().Replace('.', '-');
            keyValues["TotalSessionDuration"] = duration.ToString().Replace('.', '-');
            keyValues["TrackedDirectory"] = _logPath;
            keyValues["TotalActiveTime"] = totalActiveTimeSpan.ToString().Replace(".", "-");
            keyValues["TotalAfkTime"] = totalAfkTimeSpan.ToString().Replace('.', '-');
            keyValues["DefaultAfkStartlimitInMiliseconds"] = Configurator.GetString("AfkTimeBeginning");
            

            int afkStartTimes = AFKTracker.durations.Count;

            if (_logInDB)
            {
                DBHandler.InsertData(keyValues["SessionStartTime"], keyValues["SessionEndTime"], keyValues["TotalSessionDuration"],
                    keyValues["TrackedDirectory"], keyValues["TotalActiveTime"], keyValues["TotalAfkTime"], afkStartTimes, Convert.ToInt32(eventCounter),
                    totalCreations, totalDeletions, totalRenamings, totalErrors, systemEvent, keyValues["DefaultAfkStartlimitInMiliseconds"], totalChanges);
            }
            else
            {
                using(StreamWriter sw = new StreamWriter(Configurator.GetString("Log Path"), true))
                {
                    // Logging the process to a Textfile
                    string msg = $"\n----------\nProcess from\t{startTime}\n" +
                        $"Stopped at:\t{DateTime.Now}\n" +
                        $"Duration: {((int)duration.TotalHours)}h {((int)duration.TotalMinutes)}m " +
                        $"{((int)duration.TotalSeconds)}s\n" +
                        $"Total events: {eventCounter}\n" +
                        $"{afkDurations}" +
                        $"Total time AFK: {(int)totalAfkTimeSpan.TotalHours}h {totalAfkTimeSpan.Minutes}m " +
                        $"{totalAfkTimeSpan.Seconds}s\n" +
                        $"Total active time: {(int)totalActiveTimeSpan.TotalHours}h {totalActiveTimeSpan.Minutes}m " +
                        $"{totalActiveTimeSpan.Seconds}s\n" +
                        $"Tracked directory: {_logPath}\n" +
                        $"Closed application by SystemEvent? {systemEvent}";
                    sw.WriteLine(msg);
                    if (systemEvent) sw.WriteLine(e.ToString());

                }
            }
        }

        public void Watch()
        {
            Configurator.InitStartup();

            Thread afkThread = new Thread(AFKTracker.Track)
            {
                Name = "afkThread"
            };
            
            afkThread.Start();
            Console.Title = $"Logging in {_logPath}...";

            // Adds the events to the FileSystemWatcher
            watcher.Created += OnFileCreated;
            
            watcher.Changed += OnFileChanged;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;
            watcher.Deleted += OnDeleted;

            SystemEvents.SessionEnded += App_SessionEnding;

            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            // Adds an event for when the console is closed
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            // Saves the timestamp when the program starts
            startTime = DateTime.Now;
        }

        // Wenn eine Datei oder etwas in einem Pfad umbenannt wird
        private static void OnRenamed(object sender, RenamedEventArgs e) 
        {
            CountLog(LogString($"File renamed: {e.Name}  from  {e.OldName}"), ConsoleColor.Yellow);
            totalRenamings++;
        }
        // Wenn eine Datei erstellt wird
        private static void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            CountLog(LogString($"File created: {e.FullPath}"), ConsoleColor.Green);
            totalCreations++;
        } 
        // Wenn eine Datei geändert wird
        // -> DB!
        private static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            CountLog(LogString($"File changed: {e.FullPath}"), ConsoleColor.Cyan);
            totalChanges++;
        }
        // Bei einem Error
        private static void OnError(object sender, ErrorEventArgs e) 
        {
            PrintException(e.GetException());
            totalErrors++;
            if (Configurator.GetBool("Show Popups"))
            {
                Toaster.DefaultToastNotification($"Error #{totalErrors}", e.GetException().Message);
            }
        }
        
        // Bei einer Löschung
        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            CountLog(LogString($"File deleted: {e.FullPath}"), ConsoleColor.Gray);
            totalDeletions++;
        }
        
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
        public static string LogString(string msg) => $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {msg}";

        // For printing the error message
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

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            // Thread.Sleep(1000);
            // StreamWriter sw = new StreamWriter("Test.txt");
            // sw.WriteLine($"Der Shid wurde am {DateTime.Now} geschlossen. AuuA");
            // sw.Close();
        }

        // Occurs if the PC shuts down
        private void App_SessionEnding(object sender, SessionEndedEventArgs e)
        {
            WriteSessionInfoToFile(systemEvent: true, e);
        }




    }
}
