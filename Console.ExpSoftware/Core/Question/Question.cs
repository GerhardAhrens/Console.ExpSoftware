namespace ConsoleN
{
    using System;

    public static partial class Console
    {
        public static bool Question(string message)
        {
            return new ConsoleQuestion(message).GetAnswer();
        }
        public static bool Confirm(string message, bool defaultValue)
        {
            return new ConsoleQuestion(message, defaultValue).GetAnswer();
        }
    }

    public class ConsoleQuestion(string Message)
    {
        private bool? _defaultValue;

        public ConsoleQuestion(string Message, bool defaultValue) : this(Message)
        {

            _defaultValue = defaultValue;
        }
        public bool GetAnswer()
        {
            var confirmActions = "j/n";
            if (_defaultValue.HasValue)
            {
                confirmActions = _defaultValue.Value ? "J/n" : "j/N";
            }

            Console.Write($"{Message} [{confirmActions}]: ");
            var result = Console.ReadKey();
            Console.WriteLine();
            if (result.Key == ConsoleKey.Enter)
            {
                if (_defaultValue.HasValue)
                {
                    return _defaultValue.Value;
                }
                else
                {
                    Console.Error("Wählen Sie j oder n aus");
                    return GetAnswer();
                }
            }

            return result.Key == ConsoleKey.J;
        }
    }
}
