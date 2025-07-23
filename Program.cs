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
            ParallelChecker parallelChecker = new(args[0]);
            parallelChecker.CheckParallels();
            bool parallelFifths = parallelChecker.ParallelFifths;
            bool parallelOctaves = parallelChecker.ParallelOctaves;
        }
    }
}