using Parallelograph.Models;
using Parallelograph.Util.Consts;
using Parallelograph.Util.Debug;
using Parallelograph.Util.Exceptions;

namespace Parallelograph.Controllers
{
    internal class ParallelChecker
    {
        
        public bool HasParallelFifths = false;
        public bool HasParallelOctaves = false;
        private HashSet<Interval> _fifths = [];
        private HashSet<Interval> _octaves = [];
        private List<List<int>>? _voices;

        public ParallelChecker(string? xmlFilePath)
        {
            try
            {
                MusicXmlParser parser = new(xmlFilePath ?? "");
                _voices = parser.CoalesceAndGetFourParts();
                DBG.WriteLine("In ParallelChecker constructor.");
                if (_voices is null || _voices.Count != Consts.VOICE_COUNT)
                {
                    throw new InvalidMusicXmlDataException("Invalid number of voices.");
                }
                if (_voices[0].Count != _voices[1].Count || _voices[1].Count != _voices[2].Count || //TODO ensure correct checks.
                    _voices[2].Count != _voices[3].Count || _voices[0].Count != _voices[2].Count ||
                    _voices[0].Count != _voices[3].Count || _voices[1].Count != _voices[3].Count)
                {
                    throw new InvalidMusicXmlDataException("Note count mismatch between voices. Currently, only homorhythmic analysis is supported.");
                }
                for (int i = 0; i < _voices!.Count; i++)
                {
                    DBG.WriteLine($"i = {i}\nNotes:");
                    foreach (int note in _voices![i])
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

        private bool IsPerfect(Interval interval, int differential, string intervalName)
        {
            int top = _voices![interval.TopVoice][interval.Pos], bottom = _voices![interval.BottomVoice][interval.Pos];
            int noteDistance = Math.Abs(top - bottom);
            bool isPerfect = noteDistance % differential == 0;

            if (isPerfect)
            {
                string topVoiceName = Consts.VOICE_NAMES[interval.TopVoice],
                    bottomVoiceName = Consts.VOICE_NAMES[interval.BottomVoice];

                string topNoteData = $"({Consts.NOTE_NAMES[top % 12]}{top / 12}) ({top})",
                    bottomNoteData = $"({Consts.NOTE_NAMES[bottom % 12]}{bottom / 12}) ({bottom})";

                Console.WriteLine();
                Console.WriteLine($"Found perfect {intervalName}:");
                Console.WriteLine($"Top voice: {topVoiceName} {topNoteData}");
                Console.WriteLine($"Bottom voice: {bottomVoiceName} {bottomNoteData}");
                Console.WriteLine();
            }

            return noteDistance % differential == 0;
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
                if (IsPerfect(interval, Consts.PERFECT_FIFTH_DIFFERENTIAL, "fifth"))
                {
                    _fifths.Add(interval);
                }
            }
            for (int i = 1; i < _voices!.Count; i++)
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
                    if (IsPerfect(interval, Consts.PERFECT_FIFTH_DIFFERENTIAL, "fifth"))
                    {
                        // DBG.WriteLine($"{interval.ToString()} is a perfect fifth.");
                        Interval check = new(interval.TopVoice, interval.BottomVoice, interval.Pos - 1);
                        _fifths.Add(interval);
                        if (_fifths.Contains(check))
                        {
                            HasParallelFifths = true;
                            string topName = Consts.VOICE_NAMES[interval.TopVoice], bottomName = Consts.VOICE_NAMES[interval.BottomVoice];
                            Console.WriteLine($"Parallel fifths found between {topName} and {bottomName} at {interval.Pos}.");
                        }
                    }
                }
            }
        }
        private void CheckParallelOctaves()
        {
            DBG.WriteLine("Checking parallel octaves.");
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
                if (IsPerfect(interval, Consts.PERFECT_OCTAVE_DIFFERENTIAL, "octave"))
                {
                    _octaves.Add(interval);
                }
            }
            for (int i = 1; i < _voices!.Count; i++)
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
                    if (IsPerfect(interval, Consts.PERFECT_OCTAVE_DIFFERENTIAL, "octave"))
                    {
                        // DBG.WriteLine($"{interval.ToString()} is a perfect octave.");
                        Interval check = new(interval.TopVoice, interval.BottomVoice, interval.Pos - 1);
                        _octaves.Add(interval);
                        if (_octaves.Contains(check))
                        {
                            HasParallelOctaves = true;
                            string topName = Consts.VOICE_NAMES[interval.TopVoice], bottomName = Consts.VOICE_NAMES[interval.BottomVoice];
                            Console.WriteLine($"Parallel octaves found between {topName} and {bottomName} at {interval.Pos}.");
                        }
                    }
                }
            }
        }
        public void CheckParallels()
        {
            CheckParallelFifths();
            CheckParallelOctaves(); //TODO add unison support.
        }
    }
}