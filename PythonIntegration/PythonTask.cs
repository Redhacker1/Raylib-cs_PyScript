using RaylibTest.MainAssembly;
using RaylibTest.Queue;

namespace RaylibTest.Python
{
    class PythonTask : Task_base
    {
        PyScript Script_Module;

        public override dynamic Run_Task()
        {
            Script_Module = G_vars.Scripts[Arguments[0]];

            if (TaskType == "Run Script")
            {
                Script_Module.RunScript();
                return null;
            }

            if (TaskType == "Run Function")
                // Code cleanup
                return Script_Module.ToCSharp(
                    Script_Module.PythonFunction(Arguments[1], Arguments[2])
                );
            return null;
        }
    }
}