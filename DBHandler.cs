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

        public static void Execute()
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            engine.ExecuteFile("main.py", scope);
            
        }
    }
}
