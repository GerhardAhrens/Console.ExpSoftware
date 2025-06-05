namespace ConsoleN
{
    using System;

    public static partial class Console
    {
        public static void Continue(string text = "")
        {
            System.ConsoleColor defaultColor = System.Console.ForegroundColor;

            System.Console.CursorVisible = false;
            if (string.IsNullOrEmpty(text) == true)
            {
                System.Console.Write('\n');
                System.Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Eine Taste für weiter drücken!");
                System.Console.Write('\n');
                System.Console.ForegroundColor = defaultColor;
            }
            else
            {
                System.Console.Write('\n');
                System.Console.ForegroundColor = System.ConsoleColor.White;
                Console.WriteLine($"{text}");
                System.Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Eine Taste für weiter drücken!");
                System.Console.Write('\n');
                System.Console.ForegroundColor = defaultColor;
            }

            System.Console.ReadKey();
            System.Console.CursorVisible = true;
        }
    }
}
