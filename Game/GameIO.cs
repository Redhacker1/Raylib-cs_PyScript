using System.IO;

namespace RaylibTest.MainAssembly
{
    class GameIO
    {

        public string Read_file(string path)
        {
            string contents;
            try
            {
                StreamReader thing = new StreamReader(path);
                contents = thing.ReadToEnd();
                thing.Close();
                return contents;
            }
            catch (IOException)
            {
                contents = Read_file(path);
                return contents;
            }
        }
    }
}
