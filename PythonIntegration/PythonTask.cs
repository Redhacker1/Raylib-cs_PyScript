using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Python.Runtime;

namespace RaylibTest
{
    class PythonTask : Task_base
    {
        GamePython python_api = new GamePython();
        PyObject a = PythonEngine.ImportModule("Scripts" + "." + "Main");
        public override dynamic Run_Task()
        {
            if (TaskType == "Run Script (Static)")
            {
                python_api.Python_Script_Run_Static(Arguments[0]);
                return null;
            }
            else if (TaskType == "Run Script (Dynamic)")
            {
                python_api.Python_Script_Run_Dynamic(Arguments[0]);
                return null;
            }
            else if (TaskType == "Run Script (Experimental)")
            {
                python_api.Python_Script_unified(Arguments[0]);
                return null;
            }
            else if (TaskType == "Run Function")
            {
                python_api.PythonFunction(Arguments[0],a);
                return null;
            }
            else
            {
                return null;
            }
        }
    }
}