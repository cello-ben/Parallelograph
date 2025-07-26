using Parallelograph.Models;
using Parallelograph.Util.Consts;
using Parallelograph.Util.Debug;
using Parallelograph.Util.Exceptions;

namespace Parallelograph.Controllers
{
    internal class ParallelChecker
    {
        public readonly HashSet<string> ParallelsPresent = [];
        private readonly HashSet<Interval> _intervals = [];
        private readonly List<List<int>>? _voices;

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
                DBG.WriteLine("Constructor logic finished successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.Write("Exception found:");
                Console.Error.Write(ex);
                Console.Error.Write("Failed to create parser. Please make sure you are passing a valid file.");
            }
        }

        private bool IsPerfect(Interval interval, int differential, string intervalName)
        {
            int top = _voices![interval.TopVoice][interval.Pos], bottom = _voices![interval.BottomVoice][interval.Pos]; //TODO more bounds checking before unwrapping.
            int noteDistance = Math.Abs(top - bottom);
            return noteDistance % differential == 0;
        }
        
        public void _checkParallels(string intervalName, int differential)
        {
            DBG.WriteLine($"Checking parallel {intervalName}s.");
            Interval[] start = [
                new(0, 1, 0),
                new(0, 2, 0),
                new(0, 3, 0),
                new(1, 2, 0),
                new(1, 3, 0),
                new(2, 3, 0)
            ];

            foreach (Interval interval in start)
            {
                if (IsPerfect(interval, differential, intervalName))
                {
                    DBG.WriteLine($"Perfect interval: {interval}");
                    _intervals.Add(interval);
                }
            }

            for (int i = 1; i < _voices!.Count; i++)
            {
                Interval[] curr = [
                        new(0, 1, i),
                        new(0, 2, i),
                        new(0, 3, i),
                        new(1, 2, i),
                        new(1, 3, i),
                        new(2, 3, i)
                    ];

                foreach (Interval interval in curr)
                {
                    string topVoiceName = Consts.VOICE_NAMES[interval.TopVoice],
                    bottomVoiceName = Consts.VOICE_NAMES[interval.BottomVoice];
                    int top = _voices![interval.TopVoice][interval.Pos], bottom = _voices![interval.BottomVoice][interval.Pos];
                    string topNoteData = $"({Consts.NOTE_NAMES[top % 12]}{top / 12}) ({top})",
                    bottomNoteData = $"({Consts.NOTE_NAMES[bottom % 12]}{bottom / 12}) ({bottom})";

                    if (IsPerfect(interval, differential, intervalName))
                    {
                        DBG.WriteLine($"{interval} is a perfect {intervalName}.");
                        Interval check = new(interval.TopVoice, interval.BottomVoice, interval.Pos - 1);
                        _intervals.Add(interval);
                        if (_intervals.Contains(check))
                        {
                            ParallelsPresent.Add(intervalName);
                            string topName = Consts.VOICE_NAMES[interval.TopVoice], bottomName = Consts.VOICE_NAMES[interval.BottomVoice];
                            Console.WriteLine($"Parallel {intervalName}s found between {topName} and {bottomName} at {interval.Pos}.");
                        }
                    }
                }
            }

            _intervals.Clear();
        }

        public void CheckParallels()
        {
            Dictionary<string, int> intervals = new() {
                {"fifth", Consts.PERFECT_FIFTH_DIFFERENTIAL},
                {"octave", Consts.PERFECT_OCTAVE_DIFFERENTIAL}
            };
            
            foreach (KeyValuePair<string, int> interval in intervals)
            {
                _checkParallels(interval.Key, interval.Value);
            }
        }
    }
}
