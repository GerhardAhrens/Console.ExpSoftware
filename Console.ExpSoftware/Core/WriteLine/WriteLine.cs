namespace ConsoleN
{
    using System;

    public static partial class Console
    {
        public static void Clear()
        {
            System.Console.Clear();
        }

        public static void WriteBool(bool value, string trueMessage, string falseMessage)
        {
            if (value)
            {
                Success(trueMessage ?? "True");
            }
            else
            {
                Error(falseMessage ?? "False");
            }
        }

        public static void WriteLine(string message, ConsoleColor? color = null)
        {
            DoWriteLine(message, color);
        }

        public static void WriteLine()
        {
            DoWriteLine("");
        }

        public static void Error(string message, bool showIcon = false)
        {
            if (showIcon)
            {
                message = ConsoleHelpers.IsLegacy ? "X" : "❌ " + message;
            }

            DoWriteLine(message, ConsoleColor.Red);
        }

        public static void Success(string message, bool showIcon = false)
        {
            if (showIcon)
            {
                message = ConsoleHelpers.IsLegacy ? "" : "✅ " + message;
            }

            DoWriteLine(message, ConsoleColor.Green);
        }

        public static void Info(string message, bool showIcon = false)
        {
            if (showIcon)
            {
                message = ConsoleHelpers.IsLegacy ? "i " : "❕" + message;
            }

            DoWriteLine(message, ConsoleColor.Blue);
        }

        public static void Warning(string message, bool showIcon = false)
        {
            if (showIcon)
            {
                message = ConsoleHelpers.IsLegacy ? "! " : "⚠️ " + message;
            }

            DoWriteLine(message, ConsoleColor.Yellow);
        }

        public static void Titel(string message)
        {
            DoWriteLine(message, ConsoleColor.Yellow);
        }

        public static void Print(string message, ConsoleColor setColor = ConsoleColor.White)
        {
            DoWriteLine(message, setColor);
        }

        private static void DoWriteLine(string message, ConsoleColor? color = null)
        {
            if (color.HasValue)
            {
                System.Console.ForegroundColor = color.Value;
            }

            System.Console.WriteLine(message);
            System.Console.ResetColor();
        }
    }
}
