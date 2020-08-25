using static Python.Runtime.PythonEngine;
using Python.Runtime;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Runtime;

namespace Py_embedded
{
    public class PythonAbstractions
    {
        private void Create_Windows_EnvVariables(string custom_PATH)
        {
            string pathToPython = Environment.CurrentDirectory + @"\Python37\Windows";
            string path = pathToPython + ";";
            Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pathToPython, EnvironmentVariableTarget.Process);

            var lib = new[]
                {
                pathToPython + @"\Lib",
                pathToPython + @"\DLLs",
                pathToPython + @"\Lib\site-packages",
                Environment.CurrentDirectory + @"\Scripts"
                };

            if (custom_PATH != "")
            {
                lib = new[]
                {
                pathToPython + @"\Lib",
                pathToPython + @"\DLLs",
                pathToPython + @"\Lib\site-packages",
                Environment.CurrentDirectory + @"\Scripts",
                custom_PATH
                };
            }

            string paths = string.Join("; ", lib);
            Environment.SetEnvironmentVariable("PYTHONPATH", paths, EnvironmentVariableTarget.Process);
        }

        private void Create_Linux_EnvVariables(string custom_PATH)
        {
            string pathToPython = @"\Python37\Linux";
            string path = pathToPython + ";" +
            Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pathToPython, EnvironmentVariableTarget.Process);

            var lib = new[]
                {
                @"\Python37\Linux\lib\python3.7",
                @"\Python37\Linux\DLLs",
                @"\Python37\Linux\lib\python3.7\site-packages",
                @"\Scripts"
                };

            if (custom_PATH != "")
            {
                lib = new[]
                {
                @"\Python37\Linux\lib\python3.7",
                @"\Python37\Linux\DLLs",
                @"\Python37\Linux\lib\python3.7\site-packages",
                @"\Scripts",
                custom_PATH
                };
            }

            string paths = string.Join("; ", lib);
            Environment.SetEnvironmentVariable("PYTHONPATH", paths, EnvironmentVariableTarget.Process);
        }

        public void Initpython(string custom_PATH = "")
        {
            if (Environment.OSVersion.ToString().Contains("Unix"))
            {
                Console.WriteLine("IsLinux");
                Create_Linux_EnvVariables(custom_PATH);
            }
            else
            {
                Create_Windows_EnvVariables(custom_PATH);
            }
            Initialize();
            
        }

        string FSpath_to_PyPath(string FSPath)
        {
            FSPath = FSPath.Replace('\\', '/');
            string[] Path_Split = FSPath.Split('/');
            string pyPath = Path_Split[Path_Split.Length - 1];
            if (pyPath != "/")
            {
                pyPath = pyPath.Trim().Replace("/", "");
            }

            else
            {
                pyPath = Path_Split[Path_Split.Length - 2];
                pyPath = pyPath.Trim().Remove('/');
            }


            return pyPath;
        }



        public void TerminatePython()
        {
            try
            {
                Shutdown();
            }
            catch (PythonException Exception)
            {
                Console.WriteLine(Exception.Message);
            }

        }

        public dynamic RunScript(string ScriptLocation = "Scripts", string ScriptName = "Main", string FunctionStart = "")
        {
            PyObject Script;
            ScriptLocation = FSpath_to_PyPath(ScriptLocation);
            var a = IntPtr.Zero;
            if (ScriptLocation != "Scripts")
            {
                Initpython(ScriptLocation);
            }
            else
            {
                Initpython();
                AcquireLock();
            }

            try
            {
                Script = ImportModule(ScriptLocation + "." + ScriptName);
                if (FunctionStart != "")
                {
                    Script.InvokeMethod(FunctionStart);
                }
                ReleaseLock(a);
            }
            catch (PythonException Exception)
            {
                Console.WriteLine(Exception.Message);
                return false;
            }
            TerminatePython();
            return Script;

        }

        public dynamic ToCSharp(PyObject variable)
        {
            Initpython();
            string Pytype = variable.GetPythonType().ToString();
            if (Pytype == "<class 'int'>" || Pytype == "<class 'long'>")
            {
                if (variable.As<long>() >= int.MaxValue)
                {
                    var CSvar = variable.As<long>();
                    TerminatePython();
                    return CSvar;
                }
                else
                {
                    var CSvar = variable.As<int>();
                    TerminatePython();
                    return CSvar;
                }

            }
            else if (Pytype == "<class 'short'>")
            {
                var CSvar = variable.As<short>();
                TerminatePython();
                return CSvar;
            }

            else if (Pytype == "<class 'str'>")
            {
                var CSvar = variable.As<string>();
                TerminatePython();
                return CSvar;
            }
            else if (Pytype == "<class 'bool'>")
            {
                var CSvar = variable.As<bool>();
                TerminatePython();
                return CSvar;
            }
            else if (Pytype == "<class 'NoneType'>")
            {
                TerminatePython();
                return null;
            }
            else if (Pytype == "<class 'list'>")
            {
                List<dynamic> list_var = new List<dynamic>();
                foreach (PyObject item in variable)
                {
                    dynamic thing = ToCSharp(item);
                    list_var.Add(thing);
                }
                TerminatePython();
                return list_var;
            }

            return null;
        }

        public int Python_Console()
        {
            Initpython();

            int i = Runtime.Py_Main( 0, new string[] { });
            TerminatePython();

            return i;

        }



        public string Get_python_type(PyObject variable)
        {
            Initpython();
            var type = variable.GetPythonType();
            string stringtype = type.ToString();
            TerminatePython();
            return stringtype;

        }

        public dynamic RunFunction(string ScriptLocation = "Scripts", string ScriptName = "Main.py", string FuncName = "Main", dynamic[] Args = null)
        {
            Initpython(ScriptLocation);
            ScriptLocation = FSpath_to_PyPath(ScriptLocation);

            Initpython();


            dynamic return_value;
            try
            {
                Console.WriteLine("Function Starting");
                PyObject Script = ImportModule(ScriptLocation + "." + ScriptName);
                if (Args == null)
                {
                    return_value = Script.InvokeMethod(FuncName);
                }
                else
                {

                    return_value = Script.InvokeMethod(FuncName, Args.ToPython());
                }

            }
            catch (PythonException Exception)
            {
                Console.WriteLine(Exception.Message);
                TerminatePython();
                return null;
            }
            TerminatePython();
            return ToCSharp(return_value);
        }
    }
}

