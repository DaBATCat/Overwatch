using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Overwatch
{
    internal class Configurator
    {
        public enum Datatypes
        {
            INT,
            DOUBLE,
            BOOL,
            STRING
        }
        public static void EnableStartOnBoot()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        }
        public static string GetSettings()
        {
            char commentChar = '!';

            string settings = "";

            string line;

            int amountOfSettings = 0;
            int a = 0;
            
            // This sets the amount of Settings for each not commented line which inherit '='
            using (StreamReader reader = new StreamReader("settings.txt")) 
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
            using(StreamReader reader = new StreamReader("settings.txt"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if(line.Length > 0 && line.TrimStart()[0] != commentChar)
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
            string[] values = new string[options.Length];
            settingsIndex = 0;

            using (StreamReader reader = new StreamReader("settings.txt"))
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

            settings += $"Total settings: {amountOfSettings}\n";
            for(int i = 0; i < settingsIndex; i++)
            {
                settings += $"{options[i]}:{values[i]} | {GetDatatype(values[i])}\n";
            }
            return settings;
        }

        
        public static Datatypes GetDatatype(string input)
        {
            if (int.TryParse(input, out int n)) return Datatypes.INT;
            if (double.TryParse(input, out double d)) return Datatypes.DOUBLE;
            if (bool.TryParse(input, out bool b)) return Datatypes.BOOL;
            return Datatypes.STRING;

        }
    }
}
