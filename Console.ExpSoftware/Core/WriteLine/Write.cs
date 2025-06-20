namespace ConsoleN
{
    using System;

    public static partial class Console
    {
        internal static void Write(string message, ConsoleColor? color = null)
        {
            DoWrite(message, color);
        }

        private static void DoWrite(string message, ConsoleColor? color = null)
        {
            if (color.HasValue)
            {
                System.Console.ForegroundColor = color.Value;
            }

            System.Console.Write(message);

            if (color.HasValue)
            {
                System.Console.ResetColor();
            }
        }
    }
}
