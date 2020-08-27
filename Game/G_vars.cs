using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RaylibTest.MainAssembly
{
    class G_vars
    {
        //Global Variables

        // List of valid paths for python scripts
        static public string[] Python_Script_Directories = { @"\Scripts" };
        //Contains a Dictionary of Script paths and their contents for caching (will work out hot reloading later)
        static public Dictionary<string, string> Scripts = new Dictionary<string, string>();
        // Scripts run once and known to be safe to run again without error handling
        static public List<string> Safe_Scripts = new List<string>();
        // Dictionary of FileWatchers
        static public Dictionary<string, System.IO.FileSystemWatcher> File_Watchers = new Dictionary<string, System.IO.FileSystemWatcher>();
        // FPS limit
        static public int FPS_Limit = 0;
        //Resolution
        static public Vector2 Resolution = new Vector2(1024, 768);

        public void HelloThere()
        {
            Console.WriteLine("Hello there!");
        }
    }
}
