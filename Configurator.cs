using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Overwatch
{
    internal class Configurator
    {
        static string path = "settings.txt";
        static char commentChar = '!';

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
            Console.WriteLine("Amount of settings: " + amountOfSettings);
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

    }
}
