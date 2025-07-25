namespace Parallelograph.Util.Debug
{
    internal static class DBG //TODO non-static and save state if prefixed.
    {
        internal static void WriteLine(string message)
        {
#if DEBUG
            Console.WriteLine($"Debug: {message}");
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
    }
}