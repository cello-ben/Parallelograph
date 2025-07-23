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
        private string[] VOICE_NAMES = { "Soprano", "Alto", "Tenor", "Bass" };
        private bool parallelFifths = false;
        private bool parallelOctaves = false;
        private HashSet<PerfectFifth> fifths = new();
        private HashSet<PerfectOctave> octaves = new();
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
            }
            catch (Exception ex)
            {
                DBG.WriteError(ex, "Failed to create parser. Please make sure you are passing a valid file.");
            }
        }

        private void CheckParallelFifths()
        {
            PerfectFifth[] start = {
                new(voices![0][0], voices![1][0], 0), //Soprano/Alto
                new(voices![0][0], voices![2][0], 0), //Soprano/Tenor
                new(voices![0][0], voices![3][0], 0), //Soprano/Bass
                new(voices![1][0], voices![2][0], 0), //Alto/Tenor
                new(voices![1][0], voices![3][0], 0), //Alto/Bass
                new(voices![2][0], voices![3][0], 0) //Tenor/Bass
            };

            foreach (PerfectFifth fifth in start)
            {
                if (Math.Abs(fifth.TopVoice - fifth.BottomVoice) % PERFECT_FIFTH_DIFFERENTIAL == 0)
                {
                    fifths.Add(fifth);
                }
            }
            for (int i = 1; i < voices!.Count; i++)
            {
                PerfectFifth[] curr = {
                        new(voices![0][i], voices![1][i], i),
                        new(voices![0][i], voices![2][i], i),
                        new(voices![0][i], voices![3][i], i),
                        new(voices![1][i], voices![2][i], i),
                        new(voices![1][i], voices![3][i], i),
                        new(voices![2][i], voices![3][i], i)
                    };

                foreach (PerfectFifth fifth in curr)
                {
                    // PerfectFifth check = new(voices!)
                    // if (fifths.Contains)
                }
            }
        }

        private void CheckParallelOctaves()
        {
            parallelOctaves = true;
        }
        public void CheckParallels()
        {
            CheckParallelFifths();
            CheckParallelOctaves();
        }
    }
}