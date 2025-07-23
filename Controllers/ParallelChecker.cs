using Parallelograph.Models;
using Parallelograph.Util;
using Parallelograph.Util.Exceptions;

namespace Parallelograph.Controllers
{
    internal class ParallelChecker
    {
        private const int VOICE_COUNT = 4;
        private const int PERFECT_FIFTH_DIFFERENTIAL = 7;
        private const int PERFECT_OCTAVE_DIFFERENTIAL = 12;
        private readonly string[] VOICE_NAMES = { "Soprano", "Alto", "Tenor", "Bass" };
        public bool ParallelFifths = false;
        public bool ParallelOctaves = false;
        private HashSet<Interval> fifths = new();
        private HashSet<Interval> octaves = new();
        private List<List<int>>? voices;

        public ParallelChecker(string? xmlFilePath)
        {
            try
            {
                MusicXmlParser parser = new(xmlFilePath ?? "");
                voices = parser.CoalesceAndGetFourParts();
                DBG.WriteLine("In ParallelChecker constructor.");
                if (voices is null || voices.Count != VOICE_COUNT)
                {
                    throw new InvalidMusicXmlDataException("Invalid number of voices.");
                }
                if (voices[0].Count != voices[1].Count || voices[1].Count != voices[2].Count || //TODO ensure correct checks.
                    voices[2].Count != voices[3].Count || voices[0].Count != voices[2].Count ||
                    voices[0].Count != voices[3].Count || voices[1].Count != voices[3].Count)
                {
                    throw new InvalidMusicXmlDataException("Note count mismatch between voices. Currently, only homorhythmic analysis is supported.");
                }
                for (int i = 0; i < voices!.Count; i++)
                {
                    DBG.WriteLine($"i = {i}\nNotes:");
                    foreach (int note in voices![i])
                    {
                        DBG.Write($"{note} ");
                    }
                    DBG.Write("\n");
                }
                DBG.WriteLine("Constructor logic finished successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception found.");
                DBG.WriteError(ex, "Failed to create parser. Please make sure you are passing a valid file.");
            }
        }
        private bool IsPerfectFifth(Interval fifth)
        {
            return Math.Abs(voices![fifth.TopVoice][fifth.Pos] - voices![fifth.BottomVoice][fifth.Pos]) % PERFECT_FIFTH_DIFFERENTIAL == 0;
        }
        private void CheckParallelFifths()
        {
            DBG.WriteLine("Checking parallel fifths.");
            Interval[] start = {
                new(0, 1, 0),
                new(0, 2, 0),
                new(0, 3, 0),
                new(1, 2, 0),
                new(1, 3, 0),
                new(2, 3, 0)
            };

            foreach (Interval interval in start)
            {
                if (IsPerfectFifth(interval))
                {
                    fifths.Add(interval);
                }
            }
            for (int i = 1; i < voices!.Count; i++)
            {
                Interval[] curr = {
                        new(0, 1, i),
                        new(0, 2, i),
                        new(0, 3, i),
                        new(1, 2, i),
                        new(1, 3, i),
                        new(2, 3, i)
                    };

                foreach (Interval interval in curr)
                {
                    if (IsPerfectFifth(interval))
                    {
                        DBG.WriteLine($"{interval.ToString()} is a perfect fifth.");
                        Interval check = new(interval.TopVoice, interval.BottomVoice, interval.Pos - 1);
                        fifths.Add(new(interval.TopVoice, interval.BottomVoice, interval.Pos));
                        if (fifths.Contains(check))
                        {
                            ParallelFifths = true;
                            Console.WriteLine($"Parallel fifths found between {VOICE_NAMES[interval.TopVoice]} and {VOICE_NAMES[interval.BottomVoice]} at {interval.Pos}.");
                        }
                    }
                }
            }
        }

        private void CheckParallelOctaves()
        {
            // parallelOctaves = true;
        }
        public void CheckParallels()
        {
            CheckParallelFifths();
            CheckParallelOctaves();
        }
    }
}