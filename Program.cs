using Raylib_cs;
using Python.Runtime;
using System;
using System.IO;
using RaylibTest;
using System.Threading.Tasks;

static class Program
{
    public static Game GameClass = new Game();

    public static void Main()
    {
        GameClass.Startup();
    }
}
