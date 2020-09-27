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
        PyScript Script_Module;
        public override dynamic Run_Task()
        {
            Script_Module = G_vars.Scripts[Arguments[0]];

            if (TaskType == "Run Script")
            {
                Script_Module.RunScript();
                return null;
            }
            else if (TaskType == "Run Function")
            {
                dynamic returnvalue = Script_Module.PythonFunction(Arguments[1], Arguments[2]);
                returnvalue = Script_Module.ToCSharp(returnvalue);
                //Console.WriteLine(returnvalue.ToString());
                return returnvalue;
            }
            else
            {
                return null;
            }
        }
    }
}