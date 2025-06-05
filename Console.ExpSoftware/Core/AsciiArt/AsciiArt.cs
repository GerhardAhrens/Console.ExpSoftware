namespace ConsoleN
{
    using System;

    public static partial class Console
    {
        public static void AsciiArt(string message, ConsoleColor? color = null)
        {
            WriteLine(AsciiChars.GetAsciiArt(message), color);
        }
    }
}
