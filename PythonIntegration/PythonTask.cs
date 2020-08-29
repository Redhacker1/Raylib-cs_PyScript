using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RaylibTest.Queue;
using Python.Runtime;
using RaylibTest.MainAssembly;

namespace RaylibTest.Python
{
    class PythonTask : Task_base
    {
        PyScript a;
        public override dynamic Run_Task()
        {
            a = G_vars.Scripts[Arguments[0]];

            if (TaskType == "Run Script")
            {
                a.RunScript();
                return null;
            }
            else if (TaskType == "Run Function")
            {
                a.PythonFunction(Arguments[1]);
                return null;
            }
            else
            {
                return null;
            }
        }
    }
}