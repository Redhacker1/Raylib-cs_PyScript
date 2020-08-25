using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RaylibTest
{
    class GameIO
    {
        public void InitPyFS()
        {
            AddPyScripts();
            Get_and_Add_Py_Files_To_List();
            Console.WriteLine("Script FS initialized Python now ready to be used");

        }
        public void AddPyScripts(string path = "")
        {
            if (path == "")
            {
                path = Directory.GetCurrentDirectory() + @"\Scripts";
            }
            // Create a new FileSystem watcher
            FileSystemWatcher Watcher = new FileSystemWatcher();
            // Give the FileSystem watcher its attributes
            Watcher.Path = path;
            Watcher.NotifyFilter = NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.FileName
                                 | NotifyFilters.DirectoryName;
            Watcher.Filter = "*.py";
            Watcher.IncludeSubdirectories = true;
            Watcher.InternalBufferSize = 65536;

            // Add event handlers.
            Watcher.Changed += OnChanged;
            Watcher.Created += OnAdded;
            Watcher.Renamed += OnRenamed;

            // Begin Watching
            Watcher.EnableRaisingEvents = true;

            // Add the watcher to a list of filewatchers so that it along with the rest of them can be modified, referenced, changed or deleted at runtime
            Program.GameClass.File_Watchers.Add(path, Watcher);
        }

        //This is a manual start that will allow the python files already there to populate the list so they can be referenced initially, the AddPyScripts filewatcher and events will not add these files and so they must be added like this manually.
        public void Get_and_Add_Py_Files_To_List()
        {
            List<string> directories = new List<string> { Directory.GetCurrentDirectory() + @"\Scripts" };
            foreach (string path in directories)
            {
                foreach (string file in Directory.GetFiles(path, "*.py", SearchOption.AllDirectories))
                {
                    string code = Read_file(file);
                    Game.Scripts.Add(file, code);
                    Console.WriteLine(file);
                }
            }
        }

        public string Read_file(string path)
        {
            string contents;
            try
            {
                StreamReader thing = new StreamReader(path);
                contents = thing.ReadToEnd();
                thing.Close();
                return contents;
            }
            catch (IOException)
            {
                contents = Read_file(path);
                return contents;
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Set the key of the dictionary referring to the script to reflect the new changes made to the file. Also removes the "Safe Script" tag from the script due to it now able to have syntax errors in the script
            string code = Read_file(e.FullPath);
            Game.Scripts[e.FullPath] = code;
            Game.Safe_Scripts.Remove(e.FullPath);
        }

        private void OnAdded(object source, FileSystemEventArgs e)
        {
            // Add a new key to the dictionary and set the value to be the value to be the contents of the script file.
            string code = Read_file(e.FullPath);
            Game.Scripts.Add(e.FullPath, code);
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            // Take the contents of the old file and add it under the new key then delete the old key.
            Game.Scripts.Add(e.FullPath, Game.Scripts[e.OldFullPath]);
            Game.Scripts.Remove(e.OldFullPath);
        }
    }
}
