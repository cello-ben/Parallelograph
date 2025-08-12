using System.Xml.Linq;
using Parallelograph.Models;

namespace Parallelograph.Util.Debug
{
    internal static class DBG //TODO non-static and save state if prefixed.
    {
        internal static void WriteLine(string message)
        {
#if DEBUG
            Console.WriteLine($"<DEBUG> {message}");
#endif
        }

        internal static void Write(string message)
        {
#if DEBUG
            Console.Write($"{message}");
#endif
        }

        internal static void WriteError(Exception ex, string message)
        {
            Console.Error.WriteLine(ex.Message);
            Console.Error.WriteLine(message);
        }

        internal static void PrintMap(List<List<int>> voices)
        {
#if DEBUG
            Console.WriteLine("Map:");
            Console.WriteLine($"Number of notes per voice: {voices.Count}");
            string[] parts = ["Soprano: ", "Alto: ", "Tenor: ", "Bass: "];
            for (int i = 0; i < Consts.Consts.VOICE_COUNT; i++)
            {
                for (int j = 0; j < voices.Count; j++)
                {
                    int mod = voices[i][j] % 12;
                    parts[i] += $"{voices[i][j]} ({mod}) - ({Consts.Consts.NOTE_NAMES[mod]})\t";
                }
            }

            foreach (string part in parts)
            {
                Console.WriteLine(part);
            }
#endif
        }

        internal static void PrintTree(XDocument document)
        {
#if DEBUG
            foreach (var node in document.DescendantNodes())
            {
                Console.WriteLine(node);
            }
#endif
        }
    }
}