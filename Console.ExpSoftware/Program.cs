//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2025
// </copyright>
// <Template>
// 	Version 2.0.2025.0, 28.4.2025
// </Template>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>04.05.2025 19:34:00</date>
//
// <summary>
// Konsolen Applikation mit Menü
// </summary>
//-----------------------------------------------------------------------

namespace Console.ExpSoftware
{
    /* Imports from NET Framework */
    using System;

    public class Program
    {
        private static void Main(string[] args)
        {
            ConsoleMenu.Add("01", "Auswahl Menüpunkt 1", () => MenuPoint1("1"));
            ConsoleMenu.Add("10", "Auswahl Menüpunkt 10", () => MenuPoint1("10"));
            ConsoleMenu.Add("99", "Auswahl Menüpunkt 99", () => MenuPoint1("99"));
            ConsoleMenu.Add("2", "Auswahl Menüpunkt 2", () => MenuPoint2());
            ConsoleMenu.Add("X", "Beenden", () => ApplicationExit());

            do
            {
                _ = ConsoleMenu.SelectKey(2, 2);
            }
            while (true);
        }

        private static void ApplicationExit()
        {
            Environment.Exit(0);
        }

        private static void MenuPoint1(string param)
        {
            Console.Clear();

            ConsoleMenu.Wait(param);
        }

        private static void MenuPoint2()
        {
            Console.Clear();

            ConsoleMenu.Wait();
        }
    }
}
