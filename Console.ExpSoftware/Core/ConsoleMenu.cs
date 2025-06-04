﻿//-----------------------------------------------------------------------
// <copyright file="ConsoleMenu.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>07.05.2025 14:27:39</date>
//
// <summary>
// Klasse zum Erstellen und Auswählen von Menüpunkten
// </summary>
//-----------------------------------------------------------------------

namespace Console.ExpSoftware
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class ConsoleMenu
    {
        private static Dictionary<string, Tuple<string, Action>> menuList = new Dictionary<string, Tuple<string, Action>>();
        private static List<char> inputKeys = new List<char>();

        public static void Add(string mpoint, string mtext)
        {
            if (menuList.ContainsKey(mpoint) == false)
            {
                menuList.Add(mpoint, new Tuple<string, Action>(mtext, null));
            }
        }

        public static void Add(string mpoint, string mtext, Action action)
        {
            if (menuList.ContainsKey(mpoint) == false)
            {
                menuList.Add(mpoint, new Tuple<string, Action>(mtext, action));
            }
        }

        public static void Wait(string text = "")
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.CursorVisible = false;
            if (string.IsNullOrEmpty(text) == true)
            {
                Console.Write('\n');
                Console.WriteLine("Eine Taste drücken, um zum Menü zurück zukehren!");
            }
            else
            {
                Console.Write('\n');
                Console.WriteLine($"Eine Taste drücken, um zum Menü zurück zukehren! {text}");
            }

            Console.ForegroundColor = defaultColor;
            Console.ReadKey();
            Console.CursorVisible = true;
        }

        public static void Print(string text, ConsoleColor setColor = ConsoleColor.White)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = setColor;
            Console.CursorVisible = false;

            Console.WriteLine(text);

            Console.ForegroundColor = defaultColor;
            Console.CursorVisible = true;
        }

        public static void Continue(string text = "")
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            Console.CursorVisible = false;
            if (string.IsNullOrEmpty(text) == true)
            {
                Console.Write('\n');
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Eine Taste für weiter drücken!");
                Console.ForegroundColor = defaultColor;
            }
            else
            {
                Console.Write('\n');
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{text}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Eine Taste für weiter drücken!");
                Console.ForegroundColor = defaultColor;
            }

            Console.ReadKey();
            Console.CursorVisible = true;
        }

        public static void Titel(string text)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            Console.Write('\n');
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{text}");
            Console.ForegroundColor = defaultColor;
        }

        public static string SelectKey(int left = 0, int top = 0)
        {
            string resultKeys = string.Empty;
            ConsoleColor defaultColor = Console.ForegroundColor;

            Console.Clear();
            Console.SetCursorPosition(left, top);
            int topStep = -1;
            foreach (KeyValuePair<string, Tuple<string, Action>> mtext in menuList)
            {
                topStep++;
                Console.SetCursorPosition(left, (top + topStep));
                Console.WriteLine($"{mtext.Key}. {mtext.Value.Item1}");
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write('\n');
            Console.WriteLine("Wählen Sie einen Menüpunkt oder 'x' für beenden [mit ESC letze Eingabe löschen!]");
            Console.ForegroundColor = defaultColor;

            do
            {
                Console.CursorVisible = false;
                char key = Console.ReadKey(true).KeyChar;
                if (key == '\u001b')
                {
                    inputKeys.Clear();
                    continue;
                }

                if (inputKeys != null && inputKeys.Count == 0)
                {
                    foreach (var _ in menuList.Where(menu => menu.Key.StartsWith(key.ToString().ToUpper(CultureInfo.CurrentCulture), StringComparison.OrdinalIgnoreCase) == true).Select(menu => new { }))
                    {
                        inputKeys.Add(key);
                        break;
                    }
                }
                else if (inputKeys != null && inputKeys.Count == 1)
                {
                    inputKeys.Add(key);
                }
                else if (inputKeys != null && inputKeys.Count == 2)
                {
                    inputKeys.Add(key);
                }

                string selectedKeys = string.Join(string.Empty, inputKeys);

                if (menuList.ContainsKey(selectedKeys.ToUpper(CultureInfo.CurrentCulture)) == true)
                {
                    resultKeys = selectedKeys.ToUpper(CultureInfo.CurrentCulture);
                    inputKeys.Clear();
                    Console.CursorVisible = true;
                    break;
                }

            } while (true);

            if (menuList.ContainsKey(resultKeys) == true)
            {
                menuList[resultKeys].Item2();
            }

            return resultKeys;
        }
    }
}
