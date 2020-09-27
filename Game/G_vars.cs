using RaylibTest.Python;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RaylibTest.MainAssembly
{
    class G_vars
    {
        //Global Variables

        //
        static string Executable_Path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        // List of valid paths for python scripts
        public static string[] Python_Script_Directories = {Executable_Path + @"\Scripts" };
        //Contains a Dictionary of Script paths and their contents for caching (will work out hot reloading later)
        static public Dictionary<string, PyScript> Scripts = new Dictionary<string, PyScript>();
        // Scripts run once and known to be safe to run again without error handling
        static public List<PyScript> Safe_Scripts = new List<PyScript>();
        // Dictionary of FileWatchers
        static public Dictionary<string, FileSystemWatcher> File_Watchers = new Dictionary<string, FileSystemWatcher>();
        // FPS limit
        static public int FPS_Limit = 0;
        //Resolution
        static public Vector2 Resolution = new Vector2(1024, 768);
    }
}
