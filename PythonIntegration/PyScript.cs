using Python.Runtime;
using RaylibTest.MainAssembly;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RaylibTest.Python
{
    class PyScript
    {
        readonly GameIO IOlib = new GameIO();

        public string ModuleName;
        public bool SafeScript = false;
        public string Contents;
        public string Filename;
        public string ShortPath;
        public string FullPath;
        public PyObject Script;

        public void Setter(string Path) 
        {
            FullPath = Path;
            string[] File_Name_and_Path = SplitPath(Path);
            ShortPath = File_Name_and_Path[0];
            Filename = File_Name_and_Path[1];
            IOlib.Read_file(FullPath);
            Contents = IOlib.Read_file(FullPath);
            try
            {
                Script = Py.Import(FSpath_to_PyPath(Path));
                //Script = PythonEngine.ImportModule(FSpath_to_PyPath(Path));
            }
            catch
            {
                // RedSkittleFox: Get name of the module that is being loaded in case we charsed:
#if DEBUG
                Console.WriteLine("Loading module: " + FSpath_to_PyPath(Path));
#endif
                Console.WriteLine("No module Found!");
            }

        }

        private string[] SplitPath(string File_Path)
        {
            File_Path = File_Path.Replace("\\", "/");
            string[] Directories_and_file = File_Path.Split('/');
            string Subdirectory = string.Empty;

            for (int index = 0; index < Directories_and_file.Length; index++)
            {
                string Directory = Directories_and_file[index];
                if (Subdirectory == string.Empty)
                {
                    Subdirectory = Directory;
                }
                else
                {
                    Subdirectory = Subdirectory + "/" + Directory;
                }
            }
            string file_Name = Directories_and_file.Last();
            Subdirectory = Subdirectory.Replace(file_Name, "");

            return new string[] { Subdirectory, file_Name };
        }

        private string FSpath_to_PyPath(string FSPath)
        {
            string Good_path = string.Empty;
            string parent_folder = string.Empty;
            foreach(string valid_path in G_vars.Python_Script_Directories)
            {
                if (FSPath.Contains(valid_path))
                {
                    Good_path = valid_path;
                    parent_folder = valid_path.Replace("\\", "/");
                    string[] parent_folder_intermediate = parent_folder.Split("/");
                    parent_folder = parent_folder_intermediate[parent_folder_intermediate.Length - 1];
                    break;
                }
            }
            if (Good_path == string.Empty)
            {
                return null;
            }
            else
            {
                string pypath = FSPath.Replace(Good_path, "");
                pypath = pypath.Replace("\\", "/");
                pypath = pypath.Replace("/",".");
                pypath = pypath.Replace(".py", "");
                pypath = parent_folder + pypath;
                ModuleName = pypath;
                return pypath;
            }
        }

        /// <summary>
        /// This runs the selected python Script
        /// </summary>
        public void RunScript()
        {
            // Go down this path if the script has not been proven to be without syntax errors
            if (false == SafeScript)
            {
                try
                {
                    PythonEngine.Exec(Contents);
                    SafeScript = true;
                    return;
                }
                catch (PythonException Ex)
                {
                    //TODO: Log Script Error with time interval as well as print to a console
                    Console.WriteLine(Ex.Message);
                    Console.WriteLine("Error!");
                    return;
                }
            }
            // If it does not then run this
            else
            {
                PythonEngine.Exec(Contents);
            }
        }

        public dynamic PythonFunction(string FuncName, dynamic[] args)
        {
            dynamic item;
            if (args == null)
            {
                item = Script.InvokeMethod(FuncName);
                return item;
            }
            else
            {
                PyObject[] pyArgs = new PyObject[args.Length];
                for (int x = 0; x < args.Length; x++)
                {
                    var pyarg = PyObject.FromManagedObject(args[x]);
                    pyArgs[x] = pyarg;
                }

                // var Return_Value = Script.Invoke(pyArgs);
                var Return_Value = Script.InvokeMethod(FuncName, pyArgs);
                return Return_Value;
            }

        }

        public dynamic ToCSharp(PyObject variable)
        {
            string Pytype = variable.GetPythonType().ToString();
            if (Pytype == "<class 'System.Int32'>" || Pytype == "<class 'int'>")
            {
                if (variable.As<long>() >= int.MaxValue)
                {
                    
                    var CSvar = variable.As<long>();
                    return CSvar;
                }
                else
                {
                    var CSvar = variable.As<int>();
                    return CSvar;
                }

            }

            else if (Pytype == "<class 'System.Int64'>")
            {
                var CSvar = variable.As<long>();
                return CSvar;
            }

            else if (Pytype == "<class 'System.Int16'>" || Pytype == "<class 'short'>")
            {
                var CSvar = variable.As<short>();
                return CSvar;
            }

            else if (Pytype == "<class 'System.String'>" || Pytype == "<class 'str'>")
            {
                var CSvar = variable.As<string>();
                return CSvar;
            }
            else if (Pytype == "<class 'System.Boolean'>" || Pytype == "<class 'bool'>")
            {
                var CSvar = variable.As<bool>();
                return CSvar;
            }
            else if (Pytype == "<class 'NoneType'>")
            {
                return null;
            }
            else if (Pytype == "<class 'System.Object[]'>" || Pytype.Contains("System.Collections.Generic.0") || Pytype == "<class 'list'>")
            {
                List<dynamic> list_var = new List<dynamic>();
                foreach (PyObject item in variable)
                {
                    dynamic thing = ToCSharp(item);
                    list_var.Add(thing);
                }
                return list_var;
            }

            return null;
        }


        public void ReloadModule()
        {
            var pylock = PythonEngine.AcquireLock();
            PythonEngine.ReloadModule(Script);
            PythonEngine.ReleaseLock(pylock);
        }
    }
}
