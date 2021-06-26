using System;
using System.Collections.Generic;
using System.IO;
using Python.Runtime;
using RaylibTest.MainAssembly;

namespace RaylibTest.Python
{
    class GamePython
    {
        public readonly GameIO IOlib = new GameIO();

        void Create_Linux_EnvVariables(string custom_PATH)
        {
            // Sets the path to python install
            string pathToPython = @"\Python37\Linux";
            string path = pathToPython + ";";
            Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pathToPython, EnvironmentVariableTarget.Process);

            string[] lib = new[]
            {
                @"\Python37\Linux\bin",
                @"\Python37\Linux\DLLs",
                @"\Python37\Linux\lib\python3.7\site-packages",
                @"\Scripts"
            };

            if (custom_PATH != "")
                lib = new[]
                {
                    @"\Python37\Linux\lib\python3.7",
                    @"\Python37\Linux\DLLs",
                    @"\Python37\Linux\lib\python3.7\site-packages",
                    @"\Scripts",
                    custom_PATH
                };

            string paths = string.Join("; ", lib);
            Environment.SetEnvironmentVariable("PYTHONPATH", paths, EnvironmentVariableTarget.Process);
        }

        void Create_Windows_EnvVariables(string custom_PATH)
        {
            Console.WriteLine(Path.Join(Environment.CurrentDirectory, "\\Python37\\Windows"));
            string pathToPython = Path.Join(Environment.CurrentDirectory, "\\Python37\\Windows");


            PythonEngine.PythonHome = pathToPython;

            Environment.SetEnvironmentVariable("PATH", pathToPython, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pathToPython, EnvironmentVariableTarget.Process);

            Console.WriteLine(Path.Join(Environment.CurrentDirectory, "\\Scripts\\"));


            List<string> paths = new List<string>
            {
                $"{pathToPython}\\Lib\\site-packages",
                $"{pathToPython}\\Lib",
                Path.Join(Environment.CurrentDirectory, "\\Scripts")
            };

            string allpaths = string.Join(';', paths);
            Environment.SetEnvironmentVariable("PYTHONPATH", allpaths, EnvironmentVariableTarget.Process);
        }

        string FSpath_to_PyPath(string FSPath)
        {
            string Good_path = string.Empty;
            string parent_folder = string.Empty;
            foreach (string valid_path in G_vars.Python_Script_Directories)
                if (FSPath.Contains(valid_path))
                {
                    Good_path = valid_path;
                    parent_folder = valid_path.Replace("\\", "/");
                    string[] parent_folder_intermediate = parent_folder.Split("/");
                    parent_folder = parent_folder_intermediate[^1];
                    break;
                }

            if (Good_path == string.Empty)
            {
                return null;
            }

            string pypath = FSPath.Replace(Good_path, "");
            pypath = pypath.Replace("\\", "/");
            pypath = pypath.Replace("/", ".");
            pypath = pypath.Replace(".py", "");
            pypath = parent_folder + pypath;
            return pypath;
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

            PythonEngine.Initialize();
        }

        public void TerminatePython()
        {
            try
            {
                PythonEngine.Shutdown();
            }
            catch (PythonException Exception)
            {
                Console.WriteLine(Exception.Message);
            }
        }

        public int Python_Console()
        {
            int i = Runtime.Py_Main(0, new string[] { });
            return i;
        }

        //This is a manually allow the python files already there to populate the list so they can be referenced initially, the AddPyScripts filewatcher and events will not add these files and so they must be added like this manually.
        public void Add_Py_Scripts()
        {
            foreach (string path in G_vars.Python_Script_Directories)
            foreach (string file in Directory.GetFiles(path, "*.py", SearchOption.AllDirectories))
            {
                // DELETEME: ~RedSkittleFox
#if DEBUG
                Console.WriteLine("Currently processing file: " + file);
#endif

                PyScript Script_c = new PyScript();
                Script_c.Setter(file);
                G_vars.Scripts[Script_c.ModuleName] = Script_c;
            }
        }

        public void Track_Py_Scripts()
        {
            foreach (string path in G_vars.Python_Script_Directories)
            {
                // Create a new FileSystem watcher
                FileSystemWatcher Watcher = new FileSystemWatcher
                {
                    // Give the FileSystem watcher its attributes
                    Path = path,
                    NotifyFilter = NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName
                                   | NotifyFilters.DirectoryName,
                    Filter = "*.py",
                    IncludeSubdirectories = true,
                    InternalBufferSize = 65536
                };

                // Add event handlers.
                Watcher.Changed += OnChanged;
                Watcher.Created += OnAdded;
                Watcher.Renamed += OnRenamed;

                // Begin Watching
                Watcher.EnableRaisingEvents = true;

                // Add the watcher to a list of filewatchers so that it along with the rest of them can be modified, referenced, changed or deleted at runtime
                G_vars.File_Watchers.Add(path, Watcher);
            }
        }

        public void InitPyFS()
        {
            Add_Py_Scripts();
            Track_Py_Scripts();
            Console.WriteLine("Script FS initialized Python now ready to be used");
        }

        void OnChanged(object source, FileSystemEventArgs e)
        {
            // Set the key of the dictionary referring to the script to reflect the new changes made to the file. Also removes the "Safe Script" tag from the script due to it now able to have syntax errors in the script
            string ModuleName = FSpath_to_PyPath(e.FullPath);
            G_vars.Scripts[ModuleName].SafeScript = false;
            G_vars.Scripts[ModuleName].Contents = IOlib.Read_file(e.FullPath);
            G_vars.Scripts[ModuleName].ReloadModule();
            Console.WriteLine("Changes to {0} have been reflected", ModuleName);
        }

        void OnAdded(object source, FileSystemEventArgs e)
        {
            // Add a new key to the dictionary and set the value to be the value to be the contents of the script file.
            PyScript Script_c = new PyScript();
            Script_c.Setter(e.FullPath);
            G_vars.Scripts[Script_c.ModuleName] = Script_c;
            Console.WriteLine("Script {0} has been added", Script_c.ModuleName);
        }

        void OnRenamed(object source, RenamedEventArgs e)
        {
            // Reimports the file and removes the old one
            string ModuleName = FSpath_to_PyPath(e.OldFullPath);
            G_vars.Scripts.Remove(ModuleName);

            PyScript Script_c = new PyScript();
            Script_c.Setter(e.FullPath);
            G_vars.Scripts[Script_c.ModuleName] = Script_c;
            Console.WriteLine("Script {0} has been Reimported", Script_c.ModuleName);
        }
    }
}