using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Overwatch
{
    internal class AFKTracker
    {
        // Wie lange der User keine Taste gedrückt hat, bis es als AFK gewertet wird (in Millisekunden)
        static readonly uint AFK_TIME_BEGINNING = Configurator.GetUInt("AFK Time Beginning " );

        // List for when the User started to go AFK
        public static List<DateTime> afkStartTimes = new List<DateTime>();

        // List of the time spans how long the user was afk each time
        public static List<TimeSpan> durations = new List<TimeSpan>();

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        // Main entry point for starting Tracking (best from extra thread because of endless loop until the program stops)
        public static void Track()
        { 
            bool consoleOutput = true;

            uint listIndex = 0;
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);

            bool isAfk = false;
            while (true)
            {
                Thread.Sleep(100);
                GetLastInputInfo(ref lastInputInfo);
                uint lastInputTime = lastInputInfo.dwTime;

                // Überprüfe, ob seit mehr als einer Minute keine Taste gedrückt wurde
                if (Environment.TickCount - lastInputTime > AFK_TIME_BEGINNING && !isAfk)
                {
                    isAfk = true;

                    // Subtract the AFK time with the already passed time
                    afkStartTimes.Add(DateTime.Now.AddMilliseconds(-AFK_TIME_BEGINNING));

                    if (consoleOutput)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine($"AFK detected at {afkStartTimes[(int)listIndex]} ID: {listIndex}");
                    }
                }

                // Tritt ein, wenn der User schon AFK ist aber in den letzten 100ms es eine Aktion gegeben hat, er also wieder aktiv ist
                if (Environment.TickCount - lastInputTime < 100 && isAfk)
                {
                    // Adds the AFK Duration to the list
                    durations.Add(DateTime.Now - afkStartTimes[(int)listIndex]);

                    // The user is now active
                    isAfk = false;

                    if (consoleOutput)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine($"AFK with ID {listIndex} stopped at {DateTime.Now}" +
                                                $"\nDuration: {durations[(int)listIndex]}");
                    }

                    listIndex++;
                }

                // Füge hier eine geeignete Pause oder Schleife ein
            }
        }
    }
}
