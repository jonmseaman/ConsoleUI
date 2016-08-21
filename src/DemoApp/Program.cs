﻿using ConsoleUI;
using System;

namespace DemoApp
{
    internal class Program
    {
        private static ScreenCollection screens = new ScreenCollection();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            if (Console.BufferHeight < 40) {
                Console.BufferHeight = 40;
            }
            if (Console.BufferWidth < 132) {
                Console.BufferWidth = 132;
            }
            Console.WindowHeight = 40;
            Console.WindowWidth = 132;

            Utils.SetWindowPosition(0, 0);

            Labels.SetupLabelScreens(screens);
            TextBoxes.SetupTextBoxScreens(screens);
            ListBoxes.SetupListBoxScreens(screens);
            KeyPressedEvents.SetupKeyPressedEventScreens(screens);
            ProgressBars.SetupProgressBars(screens);
            LoginScreen.SetupLoginScreen(screens);
            LoadingScreen.SetupLoadingScreen(screens);
            Menus.SetupMenu(screens);

            ShowScreens();
        }

        private static void ShowScreens()
        {
            for (int i = 0; i < screens.Count; i++)
            {
                screens.Show(i);
            }
        }
    }
}