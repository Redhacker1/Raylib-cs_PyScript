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

        // Window Name
        readonly string Window_name = "Hello";
        //Initializes frame counter to keep track of when things should run...
        public short frames = 1;

        //Test Variables
        PythonTask Task = null;
        readonly GameIO game_IO = new GameIO();
        readonly PythonAbstractions abstractions = new PythonAbstractions();
        G_vars Global_Variables = Program.Global_Variables;


        public void Startup()
        {
            if (Global_Variables == null)
            {
                Global_Variables = new G_vars();
                Program.Global_Variables = Global_Variables;
            }
            //Creates the window
            Raylib.InitWindow((int)G_vars.Resolution.X, (int)G_vars.Resolution.Y, Window_name);
            // Sets the Target FPS to FPS_Limit
            Raylib.SetTargetFPS(G_vars.FPS_Limit);

            if (G_vars.FPS_Limit == 0)
            {
                G_vars.FPS_Limit = 144;
            }

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
                Raylib.DrawFPS((int)G_vars.Resolution.X/2, (int)G_vars.Resolution.Y/2);

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
            if (G_vars.FPS_Limit <= frames)
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
