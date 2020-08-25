using Python.Runtime;
using RaylibTest.Python;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RaylibTest.Python
{

    class PyScript
    {
        /// <summary>
        /// Filename excluding path and file extention
        /// </summary>
        public static string filename = "foo";
        /// <summary>
        /// Path Excluding filename and extention 
        /// </summary>
        public static string path = "Bar";

        public string Contents = "";

        static readonly GamePython Python = new GamePython();

        /// <summary>
        /// This is only set to be able to be set so that the Hotreloader can hotreload this, it should never have to be manually reset by the user.
        /// </summary>
        public PyObject Script_python { get; set; } = Python.Import(path, filename);


        // Full Filename including path and file extention
        public string filename_full { get; } = path + "\\" + filename + ".py";


    }
}
