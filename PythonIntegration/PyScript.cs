using Python.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RaylibTest
{

    class PyScript
    {
        //Filename excluding path and file extention
        public static string filename = "foo";
        // Path Excluding filename and extention 
        public static string path = "Bar";

        // Full Filename including path and file extention
       string filename_full = path + "\\" + filename + ".py";

        GamePython Python = new GamePython();
        public PyObject Script_python { get => Python.Import(path, filename); }

    }
}
