using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Overwatch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Console.ForegroundColor = ConsoleColor.White;
            // new Watcher(@"C:\").Watch();
            //var thread = new Thread(Execute);
            //thread.Start();
            //Console.WriteLine("Main Thread {0} exiting...", 
            //    Thread.CurrentThread.ManagedThreadId);
            //DoSomethingWeee();
            AFKTracker.Track();
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
