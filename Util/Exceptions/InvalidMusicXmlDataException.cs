namespace Parallelograph.Util.Exceptions
{
    internal class InvalidMusicXmlDataException : Exception
    {
        public InvalidMusicXmlDataException()
        {

        }

        public InvalidMusicXmlDataException(string message) : base(message)
        {

        }

        public InvalidMusicXmlDataException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}