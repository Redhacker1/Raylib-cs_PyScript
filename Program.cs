using Raylib_cs;
using Python.Runtime;
using System;
using System.IO;
using RaylibTest;
using System.Threading.Tasks;
using RaylibTest.MainAssembly;

static class Program
{
    public static Game game = new Game();

    public static void Main()
    {
        game.Startup();
    }
}
