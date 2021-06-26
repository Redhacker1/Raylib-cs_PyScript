using RaylibTest.MainAssembly;

namespace RaylibTest.Rendering
{
    class ShaderHelper
    {
        public string GetShaderText(string Shadername)
        {
            GameIO IO = new GameIO();
            return IO.Read_file(@"Shaders\" + Shadername);
        }

        void MakeVertexShader()
        {
        }
    }
}