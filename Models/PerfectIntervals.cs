namespace Parallelograph.Models
{
    internal struct PerfectFifth
    {
        public int TopVoice { get; }
        public int BottomVoice { get; }
        public int Pos { get; }

        public PerfectFifth(int topVoice, int bottomVoice, int pos)
        {
            TopVoice = topVoice;
            BottomVoice = bottomVoice;
            Pos = pos;
        }

    }

    internal struct PerfectOctave
    {
        public int TopVoice { get; }
        public int BottomVoice { get; }
        public int Pos { get; }

        public PerfectOctave(int topVoice, int bottomVoice, int pos)
        {
            TopVoice = topVoice;
            BottomVoice = bottomVoice;
            Pos = pos;
        }

    }
}