﻿using RaylibTest.Python;
using Raylib_cs;
using System.Collections.Generic;
using System;

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
        readonly GamePython python = new GamePython();


        public void Startup()
        {
            //Creates the window
            Raylib.InitWindow((int)G_vars.Resolution.X, (int)G_vars.Resolution.Y, Window_name);
            // Sets the Target FPS to FPS_Limit
            Raylib.SetTargetFPS(G_vars.FPS_Limit);

            //Initializes python
            python.Initpython();
            Task = new PythonTask();
            python.InitPyFS();
            Task.Arguments = new dynamic[] { @"Scripts.Main", "Main", new dynamic[] {true} };
            Task.TaskType = "Run Function";


            while (!Raylib.WindowShouldClose())
            {

                //Begins Drawing the frame
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.WHITE);

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
                Raylib.DrawFPS((int)G_vars.Resolution.X/2, (int)G_vars.Resolution.Y/2);

                var answer = Task.Run_Task();
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
