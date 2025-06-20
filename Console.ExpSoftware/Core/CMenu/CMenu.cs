﻿namespace ConsoleN
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public static partial class Console
    {
        public static CMenu Menu(string displayText, params string[] args)
        {
            return new CMenu(displayText, true, args);
        }

        public static CMenu Menu(string displayText, bool showNumbers, params string[] args)
        {
            return new CMenu(displayText, true, showNumbers, args);
        }
    }
    public class CMenu
    {
        private string _displayText;
        private bool _showNumbers;
        private int _selectedIndex;
        private List<MenuOption> _options;
        private ConsoleKey _key;
        private ConsoleKey _prevKey;

        public CMenu(string displayText, bool selectFirst = true, params string[] options)
        {
            _options = [];
            _displayText = displayText;
            Init(selectFirst, options);
        }

        public CMenu(string displayText, bool selectFirst = true, bool showNumbers = true, params string[] options)
        {
            _options = [];
            _displayText = displayText;
            _showNumbers = showNumbers && options.Length < 10;
            Init(selectFirst, options);
        }


        private void Init(bool selectFirst, string[] options)
        {
            _selectedIndex = selectFirst ? 0 : -1;

            for (var i = 0; i < options.Length; i++)
                _options.Add(new MenuOption(options[i], _selectedIndex == i));
        }

        public void Show()
        {
            System.Console.Clear();
            System.Console.WriteLine(_displayText);
            if (_showNumbers)
            {
                System.Console.WriteLine("(Navigieren Sie mit den Pfeiltasten nach oben oder unten, und drücken Sie die Eingabetaste; oder verwenden Sie die Zifferntasten für eine Auswahl.)");
            }
            else
            {
                System.Console.WriteLine("(Navigieren Sie mit den Pfeiltasten nach oben oder unten und drücken Sie die Eingabetaste.)");
            }

            for (int i = 0; i < _options.Count; i++)
            {
                MenuOption option = _options[i];
                System.Console.ForegroundColor = option.Selected ? ConsoleColor.Green : ConsoleColor.White;
                var numberSign = _showNumbers ? i + 1 + ")" : "";
                System.Console.WriteLine($"{numberSign} {option.Option}");
            }

            System.Console.ResetColor();
        }


        public int Select()
        {
            Show();
            var end = false;
            while (!end)
            {
                _key = System.Console.KeyAvailable ? System.Console.ReadKey(true).Key : ConsoleKey.Clear;
                if (_key == _prevKey) continue;
                _options[_selectedIndex].Selected = false;

                switch (_key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        _selectedIndex = _selectedIndex - 1 >= 0 ? _selectedIndex - 1 : _options.Count - 1;
                        break;

                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        _selectedIndex = _selectedIndex + 1 < _options.Count ? _selectedIndex + 1 : 0;
                        break;

                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        _selectedIndex = 0;
                        end = true;
                        break;


                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        _selectedIndex = 1;
                        end = true;
                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        _selectedIndex = 2;
                        end = true;
                        break;

                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        _selectedIndex = 3;
                        end = true;
                        break;

                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        _selectedIndex = 4;
                        end = true;
                        break;

                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        _selectedIndex = 5;
                        end = true;
                        break;

                    case ConsoleKey.D7:
                    case ConsoleKey.NumPad7:
                        _selectedIndex = 6;
                        end = true;
                        break;

                    case ConsoleKey.D8:
                    case ConsoleKey.NumPad8:
                        _selectedIndex = 7;
                        end = true;
                        break;

                    case ConsoleKey.D9:
                    case ConsoleKey.NumPad9:
                        _selectedIndex = 8;
                        end = true;
                        break;

                    case ConsoleKey.Enter:
                        end = true;
                        break;
                }

                Console.WriteLine(_selectedIndex.ToString(CultureInfo.CurrentCulture));

                if (_selectedIndex >= 0 && _selectedIndex < _options.Count)
                {
                    _options[_selectedIndex].Selected = true;
                }

                if (_selectedIndex > _options.Count - 1)
                {
                    _selectedIndex = _options.Count - 1;
                    end = false;
                }

                Show();
                _prevKey = _key;
            }

            return _selectedIndex;
        }
    }
}
