namespace ConsoleN
{
    public static partial class Console
    {
        public static void Wait(string message = "")
        {
            System.ConsoleColor defaultColor = System.Console.ForegroundColor;

            System.Console.ForegroundColor = System.ConsoleColor.Red;
            System.Console.CursorVisible = false;
            if (string.IsNullOrEmpty(message) == true)
            {
                System.Console.Write('\n');
                System.Console.WriteLine("Eine Taste drücken, um zum Menü zurück zukehren!");
            }
            else
            {
                System.Console.Write('\n');
                System.Console.WriteLine($"Eine Taste drücken, um zum Menü zurück zukehren! {message}");
            }

            System.Console.ForegroundColor = defaultColor;
            System.Console.ReadKey();
            System.Console.CursorVisible = true;
        }
    }
}
