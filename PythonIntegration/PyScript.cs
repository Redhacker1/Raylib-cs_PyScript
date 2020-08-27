using Python.Runtime;
using RaylibTest.MainAssembly;
using System;
using System.Linq;

namespace RaylibTest.Python
{
    class PyScript
    {
        readonly GameIO IOlib = new GameIO();

        public string Contents;
        public string Filename;
        public string shortPath;
        public string FullPath;
        public PyObject Script;

        public void Setter(string Path) 
        {
            FullPath = Path;
            string[] File_Name_and_Path = SplitPath(Path);
            shortPath = File_Name_and_Path[0];
            Filename = File_Name_and_Path[1];
            IOlib.Read_file(FullPath);
            Contents = IOlib.Read_file(FullPath);
            //Console.WriteLine(FSpath_to_PyPath(Path));

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
            FSPath = FSPath.Replace('\\', '/');
            string[] Path_Split = FSPath.Split('/');
            string pyPath = Path_Split[^1];
            if (pyPath != "/")
            {
                pyPath = pyPath.Trim().Replace("/", "");
            }

            else
            {
                pyPath = Path_Split[^2];
                pyPath = pyPath.Trim().Remove('/');
            }


            return pyPath;
        }
    }
}
