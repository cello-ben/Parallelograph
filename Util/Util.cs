using Parallelograph.Models;

namespace Parallelograph.Util
{
    internal static class STR
    {
        const char LOWER_UPPER_ASCII_DIFFERENTIAL = (char)32;
        public static string ToTitleCase(string s) //TODO make idiomatic
        {
            if (s is null || s.Length == 0)
            {
                return "";
            }

            char[] chars = s.ToCharArray();
            if (chars[0] >= 'a' && chars[0] <= 'z')
            {
                chars[0] -= LOWER_UPPER_ASCII_DIFFERENTIAL;
            }

            for (int i = 1; i < chars.Length; i++)
            {
                if (chars[i] >= 'A' && chars[i] <= 'Z')
                {
                    chars[i] += LOWER_UPPER_ASCII_DIFFERENTIAL;
                }
            }

            return new string(chars);
        }
    }
}