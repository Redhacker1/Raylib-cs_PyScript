using Raylib_cs;
using RaylibTest.Python;

namespace RaylibTest.MainAssembly
{
    class Game
    {
        readonly GameIO game_IO = new GameIO();

        readonly GamePython python = new GamePython();

        // Window Name
        readonly string Window_name = "Big Homo";

        // Initializes frame counter to keep track of when things should run...
        public uint frames; // Fox: Start from 0

        // Test Variables
        PythonTask Task;

        public void Startup()
        {
            // Creates the window
            Raylib.InitWindow((int) G_vars.Resolution.X, (int) G_vars.Resolution.Y, Window_name);
            // Sets the Target FPS to FPS_Limit
            Raylib.SetTargetFPS(G_vars.FPS_Limit);

            // Initializes python
            python.Initpython();
            python.InitPyFS();

            // Create task and execute Main() in python
            Task = new PythonTask();
            Task.Arguments = new dynamic[] {@"Scripts.Main", "Main", null};
            Task.TaskType = "Run Function";

            while (!Raylib.WindowShouldClose())
            {
                //Begins Drawing the frame
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.WHITE);

                //Increments the frame counter
                ++frames; // Fox: Do preincrementation, it's cheaper
                Gameloop();

                // Stops Drawing
                Raylib.EndDrawing();
            }

            //Shuts the Window down
            Raylib.CloseWindow();
        }

        void Gameloop()
        {
            Raylib.DrawFPS((int) G_vars.Resolution.X / 2, (int) G_vars.Resolution.Y / 2);
            dynamic answer = Task.Run_Task();

            // Every other frame
            if (frames % 2 == 0)
            {
            }

            // Every 5 Frames
            if (frames % 5 == 0)
            {
            }

            // Every 10 frames
            if (frames % 10 == 0)
            {
            }

            // Every 30 frames
            if (frames % 30 == 0)
            {
            }

            // The end of a full frame cycle (about 1 second if able to hit the target frame limit) 
            if (G_vars.FPS_Limit <= frames)
                //Resets the frame counter
                frames = 0;
        }
    }
}