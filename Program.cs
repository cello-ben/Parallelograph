using Parallelograph.Controllers;

namespace Parallelograph
{
    internal class Program
    {
        private static void Usage()
        {
            Console.WriteLine("Usage: ./parallelograph <music_xml_file_path>");
        }
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Usage();
                return;
            }
            ParallelChecker checker = new();
            checker.CheckParallels();
            MusicXmlParser parser = new(args[0]);
        }
    }
}