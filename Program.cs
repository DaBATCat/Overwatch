using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using IronPython;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using Microsoft.Win32;
using System.Reflection;

namespace Overwatch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            new Watcher(Configurator.GetString("Tracked Path")).Watch();
            // var thread = new Thread(Execute);
            //thread.Start();
            //Console.WriteLine("Main Thread {0} exiting...", 
            //    Thread.CurrentThread.ManagedThreadId);
            //DoSomethingWeee();
            // AFKTracker.Track();
            // ScriptEngine engine = Python.CreateEngine();
            // ScriptScope scope = engine.CreateScope();
            // engine.ExecuteFile("main.py", scope);
            // int a = engine.Execute("test()", scope);
            // Console.WriteLine("Der Wert ist: " + a);
            // Environment.SetEnvironmentVariable("Runtime.PythonDLL", "C:\\Users\\Daniel\\source\\repos\\Overwatch\\packages\\pythonnet.3.0.1\\lib\\netstandard2.0");

            // Configurator.AddRegistryKey();
            // Console.WriteLine(Configurator.RunsOnStartup());
            // Configurator.PrintRegistryKeys();

            // Console.WriteLine("Settings:");
            // Console.WriteLine(Configurator.GetSettings());
            Console.ReadLine(); 
        }

        public void NormalExecution()
        {
            var thread1 = new Thread(Run1);
        }
        private static void Execute()
        {
            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Thread {0}: {1}, Priority {2}",
                Thread.CurrentThread.ManagedThreadId,
                Thread.CurrentThread.ThreadState,
                Thread.CurrentThread.Priority);
            while(stopwatch.ElapsedMilliseconds <= 5000)
            {
                Console.WriteLine("Thread {0}: Elapsed {1:N2} seconds", 
                    Thread.CurrentThread.ManagedThreadId,
                    stopwatch.ElapsedMilliseconds / 1000.0);
                Thread.Sleep(500);
            }
            stopwatch.Stop();
        }
        private static void DoSomethingWeee()
        {
            for(int i = 0; i < 50; i++)
            {
                Thread.Sleep(200);
                Console.WriteLine(i);
            }
        }

        static void Run1()
        {
            new Watcher(@"E:\").Watch();
        }

        
    }
}
