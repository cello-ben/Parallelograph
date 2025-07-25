using System.Globalization;
using Parallelograph.Controllers;
using Parallelograph.Util;

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

            Console.WriteLine("The following parallels were found:");
            foreach (string interval in parallelChecker.ParallelsPresent)
            {
                Console.WriteLine(STR.ToTitleCase(interval));
            }
        }
    }
}