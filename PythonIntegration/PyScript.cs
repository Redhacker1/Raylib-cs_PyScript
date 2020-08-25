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
        static readonly GamePython Python = new GamePython();
        /// <summary>
        /// Filename excluding path and file extention
        /// </summary>
        public static string Filename { get; } = "foo";
        /// <summary>
        /// Path Excluding filename and extention 
        /// </summary>
        public static string File_Path { get; } = "Bar";


        /// <summary>
        /// String Contents of file
        /// </summary>
        public string Contents = "";


        /// <summary>
        /// This is only set to be able to be set so that the Hotreloader can hotreload this, it should never have to be manually reset by the user.
        /// </summary>
        public PyObject Script_python { get; set; } = Python.Import(File_Path, Filename);


        // Full Filename including path and file extention
        public string Filename_full { get; } = File_Path + "\\" + Filename + ".py";


    }
}
