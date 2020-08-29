using System;
using RaylibTest.MainAssembly;

static class Program
{
    //Initializes Class with game code including game Loop
    public static Game game = new Game();
    //Initializes the global variables

    public static void Main()
    {
        Console.WriteLine("");
        game.Startup();
    }
}
