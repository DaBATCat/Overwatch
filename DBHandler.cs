using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Threading;

namespace Overwatch
{
    internal class DBHandler
    {
        public static string dbLib = "db_lib.py";
        public static string dbExecuter = "db_executer.py";
        public static string pythonExpression;

        public static void Execute()
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            engine.ExecuteFile("main.py", scope);
        }
        
        public static void InsertData(string sessionStartTime, string sessionEndTime, string totalSessionDuration,
            string trackedDirectory, string totalActiveTime, string totalAfkTime, int totalTimesAfk,
            int totalEvents, int totalCreations, int totalDeletions, int totalRenamings,
            int totalErrors, bool sessionWasClosedBySystemevent, string defaultAfkStartlimitInMiliseconds)
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            // scope.ImportModule(dbLib);
            // //scope.ImportModule("sqlite3");
            // engine.ExecuteFile(dbLib, scope);
            // engine.ExecuteFile(dbExecuter, scope);
            pythonExpression = $"import db_lib\ndb_lib.insert_data(\"(\\\"{sessionStartTime}\\\")\", \"(\\\"{sessionEndTime}\\\")\", \"(\\\"{totalSessionDuration}\\\")\"," +
                $"\"\\\"{trackedDirectory} \\\"\", \"(\\\"{totalActiveTime}\\\")\", \"(\\\"{totalAfkTime}\\\")\", {totalTimesAfk}," +
                $"{totalEvents}, {totalCreations}, {totalDeletions}, {totalRenamings}, " +
                $"{totalErrors}, {sessionWasClosedBySystemevent}, \"{defaultAfkStartlimitInMiliseconds}\")";
            using(StreamWriter sw = new StreamWriter("Expression.py"))
            {
                sw.WriteLine(pythonExpression);
            }
            engine.ExecuteFile("Expression.py");
        }
        public static void InsertTest(int number, string hallo)
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            scope.ImportModule("db_lib");
            scope.ImportModule("sqlite3");
            // pythonExpression = "insert_data(69,"
        }
    }
}
