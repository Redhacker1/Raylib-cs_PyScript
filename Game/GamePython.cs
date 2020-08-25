using Python.Runtime;
using System;
using System.Collections.Generic;
using System.IO;

namespace RaylibTest
{
    class GamePython
    {
        public readonly GameIO IOlib = new GameIO();



        public PyObject Import(string location, string Scriptname)
        {
            return PythonEngine.ImportModule(FSpath_to_PyPath(location) + "." + Scriptname);
        }

        //Actually Runs the python script: Heavily WIP!
        public void Python_Script_Run_Static(string Script)
        {
            //TODO: Disable Script With timed Log Error if Exception is thrown, Otherwise disable Exception Handling and cache the file. Remove the file from cache and repeat when the file has been edited (Also Probably a good Idea to reinitialize all entities that depended on it). Edit: Temporary Idea of behavior, Still need to reset this on file edit!
            if (false == Game.Scripts.ContainsKey(Script))
            {
                try
                {
                    string code = IOlib.Read_file(Script);
                    PythonEngine.Exec(code);
                    if (false == Game.Scripts.ContainsKey(Script))
                    {
                        Game.Scripts.Add(Script, code);
                    }
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
            else
            {
                PythonEngine.Exec(Game.Scripts[Script]);
                return;
            }
        }

        public void PythonScriptRunInternal(string ScriptLocation, string ScriptName)
        {
            ScriptLocation = FSpath_to_PyPath(ScriptLocation);
            PyObject Script = PythonEngine.ImportModule(ScriptLocation + "." + ScriptName);
        }

        // The next step, this should combine the speed of the Static Python pathway due to the caching with the hot reload ability of the Dynamic Pathway.
        public void Python_Script_unified(string Script)
        {
            // Currently Have not implemented support of External (Out of Set up area scripts)
            if (false == Game.Scripts.ContainsKey(Script))
            {
                Console.WriteLine("Currently Illegal operation, Will be implemented later");
                throw new NotImplementedException();
            }
            // Go down this path if the script has not been proven to be without syntax errors
            else if (false ==  Game.Safe_Scripts.Contains(Script)) 
            {
                try
                {
                    PythonEngine.Exec(Game.Scripts[Script]);
                    Game.Safe_Scripts.Add(Script);
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
            else
            {
                PythonEngine.Exec(Game.Scripts[Script]);
            }
        }

        public void PythonFunction(string FuncName, PyObject Script)
        {
            //ScriptLocation = FSpath_to_PyPath(ScriptLocation);
            PythonEngine.ReloadModule(Script);
            Script.InvokeMethod(FuncName);

        }

        string FSpath_to_PyPath(string FSPath)
        {
            FSPath = FSPath.Replace('\\', '/');
            string[] Path_Split = FSPath.Split('/');
            string pyPath = Path_Split[Path_Split.Length - 1];
            if (pyPath != "/")
            {
                pyPath = pyPath.Trim().Replace("/", "");
            }

            else
            {
                pyPath = Path_Split[Path_Split.Length - 2];
                pyPath = pyPath.Trim().Remove('/');
            }


            return pyPath;
        }


        public void Python_Script_Run_Dynamic(string Script)
        {
            //TODO: Use Filewatch and events for hotreloading, allowing us to cache the script like in the static version of this function.

            try
            {
                string a = IOlib.Read_file(Script);
                PythonEngine.Exec(a);
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
    }
}
