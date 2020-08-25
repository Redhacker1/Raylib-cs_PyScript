using RaylibTest.Python;
using RaylibTest.Queue;
using Python.Runtime;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RaylibTest.MainAssembly
{
    class Game
    {

        //Global Variables


        //Contains a Dictionary of Script paths and their contents for caching (will work out hot reloading later)
        static public Dictionary<string, string> Scripts = new Dictionary<string, string>();
        // Scripts run once and known to be safe to run again without error handling
        static public List<string> Safe_Scripts = new List<string>();
        // Dictionary of FileWatchers
        public Dictionary<string, System.IO.FileSystemWatcher> File_Watchers = new Dictionary<string , System.IO.FileSystemWatcher>();
        // FPS limit
        int FPS_Limit = 500;
        //Resolution
        Vector2 Resolution = new Vector2(1024, 768);
        // Window Name
        readonly string Window_name = Console.ReadLine();
        //Contains List of Actions to run in Queue, as well as info about it (Currently Unused)
        static private List<Task_base> Queue_List = new List<Task_base> { };
        //Initializes frame counter to keep track of when things should run...
        public short frames = 1;

        //Test Variables
        PythonTask Task = null;
        readonly GameIO game_IO = new GameIO();
        readonly GamePython Python = new GamePython();
        readonly PythonAbstractions abstractions = new PythonAbstractions();


        public void Startup()
        {
            //Creates the window
            Raylib.InitWindow((int)Resolution.X, (int)Resolution.Y, Window_name);
            // Sets the Target FPS to FPS_Limit
            Raylib.SetTargetFPS(FPS_Limit);

            //Raylib.InitAudioDevice();
            // Creates a new reference to Python Abstraction

            //Initializes python
            abstractions.Initpython();
            Task = new PythonTask();
            game_IO.InitPyFS();
            Task.Arguments = new dynamic[] {"Main"};
            Task.TaskType = "Run Function";

            while (!Raylib.WindowShouldClose())
            {
                //Increments the frame counter
                frames++;
                Gameloop();

                // Stops Drawing
                Raylib.EndDrawing();
            }
            //Shuts the Window down
            Raylib.CloseWindow();

        }

        void Gameloop()
        {
            // Per frame Logic goes here to keep it organized
            if (frames >= 1)
            {
                //Begins Drawing the frame
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.WHITE);
                Raylib.DrawFPS((int)Resolution.X/2, (int)Resolution.Y/2);

                Task.Run_Task();
            }
            // Every other frame
            if (Is_Divisible(frames, 2))
            {
            }
            // Every 5 Frames
            if (Is_Divisible(frames, 5))
            {
            }
            // Every 10 frames
            if (Is_Divisible(frames, 10))
            {
            }
            // Every 30 frames
            if (Is_Divisible(frames, 30))
            {
            }
            // The end of a full frame cycle (about 1 second if able to hit the target frame limit) 
            if (FPS_Limit <= frames)
            {
                //Resets the frame counter
                frames = 0;
            }
        }

        // Checks to see if the number is divisable by another number
        public bool Is_Divisible(short number, short divisible_by)
        {
            return (number % divisible_by) == 0;
        }
    } 
}
