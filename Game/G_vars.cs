using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;
using RaylibTest.Python;

namespace RaylibTest.MainAssembly
{
    class G_vars
    {
        //Global Variables

        //
        static readonly string Executable_Path = Environment.CurrentDirectory;

        // List of valid paths for python scripts
        public static string[] Python_Script_Directories = {Executable_Path + @"\Scripts"};

        //Contains a Dictionary of Script paths and their contents for caching (will work out hot reloading later)
        public static Dictionary<string, PyScript> Scripts = new Dictionary<string, PyScript>();

        // Scripts run once and known to be safe to run again without error handling
        public static List<PyScript> Safe_Scripts = new List<PyScript>();

        // Dictionary of FileWatchers
        public static Dictionary<string, FileSystemWatcher> File_Watchers = new Dictionary<string, FileSystemWatcher>();

        // FPS limit
        public static int FPS_Limit = 0;

        //Resolution
        public static Vector2 Resolution = new Vector2(1024, 768);
    }
}