using System;
using RaylibTest.MainAssembly;

static class Program
{
    //Initializes Class with game code including game Loop
    public static Game game = new Game();
    //Initializes the global variables
    public static G_vars Global_Variables = new G_vars();

    public static void Main()
    {
        Global_Variables.HelloThere();
        Console.WriteLine("");
        game.Startup();
    }
}
