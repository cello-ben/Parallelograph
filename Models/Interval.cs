using Parallelograph.Util.Consts;

namespace Parallelograph.Models
{
    internal readonly struct Interval(int topVoice, int bottomVoice, int pos)
    {
        public int TopVoice { get; } = topVoice;
        public int BottomVoice { get; } = bottomVoice;
        public int Pos { get; } = pos;

        public override string ToString()
        {
            return $"{Consts.VOICE_NAMES[TopVoice]} - {Consts.VOICE_NAMES[BottomVoice]} at position {Pos} (note {Pos + 1})";
        }

    }
}