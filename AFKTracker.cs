using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Overwatch
{
    internal class AFKTracker
    {
        // Wie lange der User keine Taste gedrückt hat, bis es als AFK gewertet wird (in Millisekunden)
        static readonly uint AFK_TIME_BEGINNING = 6000;

        // List for when the User started to go AFK
        static List<DateTime> afkStartTimes = new List<DateTime>();

        // List of the time spans how long the user was afk each time
        static List<TimeSpan> durations = new List<TimeSpan>();

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
            uint listIndex = 0;
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            bool isAfk = false;
            while (true)
            {
                GetLastInputInfo(ref lastInputInfo);
                uint lastInputTime = lastInputInfo.dwTime;

                // Überprüfe, ob seit mehr als einer Minute keine Taste gedrückt wurde
                if (Environment.TickCount - lastInputTime > AFK_TIME_BEGINNING && !isAfk)
                {
                    isAfk = true;
                    afkStartTimes.Add(DateTime.Now);
                    Console.WriteLine($"AFK detected at {afkStartTimes[(int)listIndex]} ID: {listIndex}");
                }
                // Tritt ein, wenn der User schon AFK ist aber in den letzten 100ms es eine Aktion gegeben hat, er also wieder aktiv ist
                if (Environment.TickCount - lastInputTime < 100 && isAfk)
                {
                    // Adds the AFK Duration to the list
                    durations.Add((DateTime.Now - afkStartTimes[(int)listIndex]));

                    // The user is now active
                    isAfk = false;

                    Console.WriteLine($"AFK with ID {listIndex} stopped at {afkStartTimes[(int)listIndex]}" +
                        $"\nDuration: {durations[(int)listIndex]}");
                    listIndex++;
                }

                // Füge hier eine geeignete Pause oder Schleife ein
            }
        }
    }
}
