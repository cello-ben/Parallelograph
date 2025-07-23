namespace Parallelograph.Models
{
    internal readonly struct Interval
    {
        public int TopVoice { get; }
        public int BottomVoice { get; }
        public int Pos { get; }

        public Interval(int topVoice, int bottomVoice, int pos)
        {
            TopVoice = topVoice;
            BottomVoice = bottomVoice;
            Pos = pos;
        }

        public override string ToString()
        {
            return $"{TopVoice} - {BottomVoice} at {Pos}";
        }

    }
}