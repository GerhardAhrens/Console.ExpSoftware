﻿namespace ConsoleN
{
    public class MenuOption(string option, bool selected)
    {
        public bool Selected { get; set; } = selected;
        public string Option => option;
    }
}
