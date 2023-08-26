using IronPython.Compiler.Ast;
using IronPython.Modules;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static IronPython.Modules.PythonCsvModule;
using System.Runtime.InteropServices;
using static IronPython.Modules.PythonDateTime;
using System.Data.SqlTypes;

namespace Overwatch
{
    internal class Configurator
    {
        static string path = "C:\\Users\\Daniel\\source\\repos\\Overwatch\\Overwatch\\bin\\Debug\\Settings.cfg";
        static string newSettingsPath = "C:\\Users\\Daniel\\source\\repos\\Overwatch\\Overwatch\\bin\\Debug\\TempSettingsSave.cfg";
        static char commentChar = '!';

        static string runRegistryKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        static string appName = Assembly.GetExecutingAssembly().GetName().Name;
        static string startupBatchScript = "StartUpbatch.bat";

        // For letting Overwatch blink in the task bar

        const uint FLASHW_STOP = 0; // Stop flashing
        const uint FLASHW_CAPTION = 1;
        const uint FLASHW_TRAY = 2; // Flash the taskbar button
        const uint FLASHW_ALL = 3; // Flash window caption and taskbar button
        const uint FLASHW_TIMER = 4; // Flash continuously, until the FLASHW_STOP flag is set
        const uint FLASHW_TIMERNOFG = 12; // Flash continuously until the window comes to the foreground.

        [DllImport("User32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            public uint cbSize; // Size of the structure in Bytes
            public IntPtr hwnd; // Handle to the window to be flashed, window can be opened or minimized

            public uint dwFlags; // Flash Status
            public uint uCount; // Amount of times to flash the window
            public uint dwTimeout; // Rate at which the window is being flashed in milliseconds. If 0, the function uses the default cursor blink rate
        }

        public static void FlashWindow(uint count = uint.MaxValue)
        {
            FLASHWINFO fInfo = new FLASHWINFO();
            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = Process.GetCurrentProcess().MainWindowHandle;
            fInfo.dwFlags = FLASHW_TRAY| FLASHW_TIMERNOFG;
            fInfo.uCount = count;
            fInfo.dwTimeout = 0;


            FlashWindowEx(ref fInfo);
        }

        public static void StopFlashingWindow()
        {
            FLASHWINFO fInfo = new FLASHWINFO();
            fInfo.hwnd = Process.GetCurrentProcess().MainWindowHandle;
            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.dwFlags = FLASHW_STOP;
            fInfo.uCount = uint.MaxValue;
            fInfo.dwTimeout = 0;
            FlashWindowEx(ref fInfo);
        }

        public enum Datatypes
        {
            INT,
            UINT,
            DOUBLE,
            BOOL,
            STRING,
            LONG,
            ULONG
        }
        public static void EnableStartOnBoot()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        }
        public static string GetSettings()
        {
            string settings = "";

            string line;

            int amountOfSettings = 0;
            
            // This sets the amount of Settings for each not commented line which inherit '='
            using (StreamReader reader = new StreamReader(path)) 
            { 
                while((line = reader.ReadLine()) != null)
                {
                    if (line.Length > 0 && line.TrimStart()[0] != commentChar)
                    {
                        amountOfSettings++;
                    }
                }
            }

            // Read out the options' names
            string[] options = new string[amountOfSettings];
            char limiter = '=';
            int settingsIndex = 0;
            using(StreamReader reader = new StreamReader(path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if(line.Length > 0 && line.TrimStart()[0] != commentChar)
                    {
                        string[] words = line.Split(' ');
                        line = "";
                        foreach (string word in words) { line += word; }
                        int index = line.IndexOf(limiter);
                        if (index > 0)
                        {
                            options[settingsIndex] = line.Substring(0, index);
                            settingsIndex++;
                        }
                    }
                    
                }
            }

            // Read out the values for the options
            string[] values = new string[options.Length];
            settingsIndex = 0;

            using (StreamReader reader = new StreamReader(path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length > 0 && line.TrimStart()[0] != commentChar)
                    {
                        string[] words = line.Split(' ');
                        line = "";
                        foreach (string word in words) { line += word; }
                        int index = line.IndexOf(limiter);
                        if (index > 0)
                        {
                            values[settingsIndex] = line.Substring(index + 1);
                            settingsIndex++;
                        }

                    }
                }
            }

            settings += $"Total settings: {amountOfSettings}\n";
            for(int i = 0; i < settingsIndex; i++)
            {
                settings += $"{options[i]}:{values[i]} | {GetDatatype(values[i])}\n";
            }
            return settings;
        }

        public static string[,] Settings()
        {
            string[,] finalValues;

            int amountOfSettings = 0;

            string line;

            using (StreamReader reader = new StreamReader(path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length > 0 && line.TrimStart()[0] != commentChar)
                    {
                        amountOfSettings++;
                    }
                }
            }

            // Read out the options' names
            string[] options = new string[amountOfSettings];
            char limiter = '=';
            int settingsIndex = 0;
            using (StreamReader reader = new StreamReader(path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length > 0 && line.TrimStart()[0] != commentChar)
                    {
                        string[] words = line.Split(' ');
                        line = "";
                        foreach (string word in words) { line += word; }
                        int index = line.IndexOf(limiter);
                        if (index >= 0)
                        {
                            options[settingsIndex] = line.Substring(0, index);
                            settingsIndex++;
                        }
                    }

                }
            }

            // Read out the values for the options
            string[] values = OptionValues();
            if (amountOfSettings > 1)
            {
                finalValues = new string[amountOfSettings, amountOfSettings];
                for (int i = 0; i < amountOfSettings; i++)
                {
                    finalValues[0, i] = options[i];
                }
                for (int i = 0; i < amountOfSettings; i++)
                {
                    finalValues[1, i] = values[i];
                }
                return finalValues;
            }
            else if (amountOfSettings == 1)
            {
                return new string[,] { { $"{options[0]}" }, { $"{values[0]}" } };
            }
            else throw new Exception();

            // Values are set like this:
            // for (int i = 0; i < amountOfSettings; i++)
            // {                        Value name            actual value
            //     Console.WriteLine($"{finalValues[0, i]}:{finalValues[1, i]}");
            // }
        }

        static int AmountOfSettings()
        {
            string line = "";
            int amountOfSettings = 0;
            using (StreamReader reader = new StreamReader(path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length > 0 && line.TrimStart()[0] != commentChar)
                    {
                        amountOfSettings++;
                    }
                }
            }
            return amountOfSettings;
        }

        static string[] OptionNames()
        {
            int amountOfSettings = AmountOfSettings();
            string line = "";
            string[] options = new string[amountOfSettings];
            char limiter = '=';
            int settingsIndex = 0;
            using (StreamReader reader = new StreamReader(path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length > 0 && line.TrimStart()[0] != commentChar)
                    {
                        string[] words = line.Split(' ');
                        line = "";
                        foreach (string word in words) { line += word; }
                        int index = line.IndexOf(limiter);
                        if (index >= 0)
                        {
                            options[settingsIndex] = line.Substring(0, index);
                            settingsIndex++;
                        }
                    }

                }
            }
            return options;
        }

        public static string[] OptionValues()
        {
            int amountOfSettings = AmountOfSettings();
            string line = "";
            string[] values = new string[OptionNames().Length];
            int settingsIndex = 0;
            char limiter = '=';

            using (StreamReader reader = new StreamReader(path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length > 0 && line.TrimStart()[0] != commentChar)
                    {
                        string[] words = line.Split(' ');
                        line = "";
                        foreach (string word in words) { line += word; }
                        int index = line.IndexOf(limiter);
                        if (index >= 0)
                        {
                            values[settingsIndex] = line.Substring(index + 1);
                            settingsIndex++;
                        }

                    }
                }
            }

            return values;
        }

        // Bool in settings.txt
        public static string GetVariable(string varName)
        {
            string[,] settings = Settings();
            int amountOfSettings = AmountOfSettings();
            int index = 0;
            bool varIsDeclared = false;

            // First search for the name
            for(int i = 0; i < amountOfSettings; i++)
            {
                if (settings[0, i].ToLower() == varName.ToLower())
                {
                    varIsDeclared = true;
                    index = i;
                    break;
                }
            }
            if (!varIsDeclared) throw new Exception($"\"{varName}\" couldn't be found");

            // Return the value
            return settings[1, index];
        }

        public static int GetInt(string varName)
        {
            varName = varName.Replace(" ", "");
            return Convert.ToInt32(GetVariable(varName));
        }

        public static string GetString(string varName)
        {
            varName = varName.Replace(" ", "");
            return GetVariable(varName);
        }

        public static double GetDouble(string varName)
        {
            varName = varName.Replace(" ", "");
            return Convert.ToDouble(GetVariable(varName));
        }

        public static bool GetBool(string varName)
        {
            varName = varName.Replace(" ", "");
            return Convert.ToBoolean(GetVariable(varName));
        }

        public static long GetLong(string varName)
        {
            varName = varName.Replace(" ", "");
            return Convert.ToInt64(GetVariable(varName));
        }
        public static uint GetUInt(string varName)
        {
            varName = varName.Replace(" ", "");
            return Convert.ToUInt32(GetVariable(varName));
        }
        public static ulong GetULong(string varName)
        {
            varName = varName.Replace(" ", "");
            return Convert.ToUInt64(GetVariable(varName));
        }

        public static void ChangeProperty(string varName, string newValue)
        {
            using (StreamReader sr = new StreamReader(path))
            using (StreamWriter sw = new StreamWriter(newSettingsPath))
            {
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    string[] words = line.Split('=');

                    // If there's a variable in the line
                    bool inheritsVar = false;
                    foreach(char c in line) { if (c == '=') inheritsVar = true; }
                    if (line.Length == 0 || line.TrimStart()[0] == commentChar) inheritsVar = false; 

                    string varLine;

                    if (inheritsVar)
                    {
                        varLine = line.Substring(0, line.IndexOf('='));

                        string[] varNameWords = varName.Split(' ');
                        varName = "";
                        foreach(string varNameWord in varNameWords) { varName += varNameWord; }
                        string[] varLineWords = varLine.Split(' ');
                        varLine = "";
                        foreach(string varLineWord in varLineWords) { varLine += varLineWord; }

                        // Change the value of the property
                        if (varLine.ToLower() == varName.ToLower() ) 
                        { 
                            Console.WriteLine($"Found a match in {line} --> {varLine}");
                            sw.WriteLine($"{varLine} = {newValue}");
                        }
                        else
                        {
                            sw.WriteLine(line);
                        }
                    }
                    else
                    {
                        sw.WriteLine(line);
                    }
                    
                }
            }
        }
        public static void ApplyChanges()
        {
            using (StreamReader sr = new StreamReader(newSettingsPath))
            using (StreamWriter sw = new StreamWriter(path))
            {
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    sw.WriteLine(line);
                }
            }
        }
        
        public static Datatypes GetDatatype(string input)
        {
            if (int.TryParse(input, out int n))         return Datatypes.INT;
            if (double.TryParse(input, out double d))   return Datatypes.DOUBLE;
            if (bool.TryParse(input, out bool b))       return Datatypes.BOOL;
            if (long.TryParse(input, out long l))       return Datatypes.LONG;
            if (uint.TryParse(input, out uint un))      return Datatypes.UINT;
            if (ulong.TryParse(input, out ulong ul))    return Datatypes.ULONG;
            return Datatypes.STRING;

        }
        
        // Registry key options for auto-startup

        public static void PrintRegistryKeys()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            string[] values = registryKey.GetValueNames();
            foreach (string value in values)
            {
                Console.WriteLine(value);
            }
            Console.WriteLine(registryKey.ToString());

        }
        public static void AddRegistryKey()
        {
            string appName = Assembly.GetExecutingAssembly().GetName().Name;
            if (!RunsOnStartup())
            {
                // Add the entry to the Windows-registration
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@runRegistryKey, true);
                registryKey.SetValue(appName, GetStartUpScript());
                registryKey.Close();
            }
            // else throw new Exception($"{appName} is already registered");
        }
        public static void RemoveRegistryKey()
        {
            string appName = Assembly.GetExecutingAssembly().GetName().Name;
            if (RunsOnStartup())
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@runRegistryKey, true);
                registryKey.DeleteValue(appName, true);
                registryKey.Close();
            }
            // else throw new Exception($"{appName} was not registered");
        }

        // Check if Overwatch is registered in the RunRegistryKeys
        public static bool RunsOnStartup()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@runRegistryKey, true);

            string[] registryKeys = registryKey.GetValueNames();
            foreach (string value in registryKeys)
            {
                if (value == appName) return true;
            }
            return false;
        }
        
        public static string GetStartUpScript()
        {
            string result = "";
            string line = "";
            using (StreamReader reader = new StreamReader(startupBatchScript))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    result += $"{line}";
                }
            }
            return result;
        }

        public static void InitStartup()
        {
            ApplyChanges();
            if (GetBool("RunOnStartup"))
            {
                AddRegistryKey();
            }
            else if(!GetBool("RunOnStartup"))
            {
                // If the value has been changed recently from true to false, the program will ask for being restarted
                if (RunsOnStartup())
                {
                    RemoveRegistryKey();

                    // Show the window for the message
                    Watcher.ShowWindow(Watcher.GetConsoleWindow(), Watcher.SW_SHOW);

                    // Program should blink here
                    string input;

                    void Ask()
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Title = "Property change";
                        Console.WriteLine("Info:\nA property has been changed. Do you want to restart the application now? (Y/N)");
                        Console.ForegroundColor = ConsoleColor.White;
                        input = Console.ReadLine();
                        if (input.ToUpper() == "Y" || input.ToUpper() == "N")
                        {
                            
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Not the case");
                            Console.WriteLine(input.ToUpper());
                            Ask();
                        }
                    }
                    Ask();

                    if(input.ToUpper() == "Y")
                    {
                        string fileName = Process.GetCurrentProcess().MainModule.FileName;
                        Process.Start(fileName);
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.Clear();
                    }
                }
            }

            PrintOutInfos();
        }

        public static bool PrintInfosAtBeginning() => GetBool("DisplayInfos");
        public static void PrintOutInfos()
        {
            IntPtr winHandle = Watcher.GetConsoleWindow();
            if (PrintInfosAtBeginning())
            {
                // Show the window
                Watcher.ShowWindow(winHandle, Watcher.SW_SHOW);

                string line = "========================================";
                string thanks = "Thank you for using Overwatch :)\nPress any key to continue...";
                string properties = "";
                string[,] settings = Settings();
                for(int i = 0; i < AmountOfSettings(); i++)
                {
                    if (settings[0,i].ToLower() != "currentversion")
                    {
                        properties += $"{settings[0, i]} = {settings[1, i]}\n";
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Overwatch version {GetString("currentversion")}\n{line}\n{properties}{line}\n{thanks}");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Watcher.ShowWindow(winHandle, Watcher.SW_HIDE);
            }
        }

    }
}
