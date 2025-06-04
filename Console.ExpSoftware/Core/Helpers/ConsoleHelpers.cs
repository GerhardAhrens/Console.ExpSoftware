namespace ConsoleN
{
    using System;

    using static System.Console;

    internal static class ConsoleHelpers
    {
        public static bool IsLegacy
        {
            get
            {
                return Environment.OSVersion.Platform == PlatformID.Win32NT &&
                    (OutputEncoding.CodePage != 1200 /* UTF-16 */ && OutputEncoding.CodePage != 65001 /* UTF-8 */);
            }
        }
    }
}
