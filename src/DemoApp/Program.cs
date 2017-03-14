using ConsoleUI;
using System;

namespace DemoApp
{
    internal class Program
    {
        private static Window window = new Window();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            if (Console.BufferHeight < 40)
            {
                Console.BufferHeight = 40;
            }
            if (Console.BufferWidth < 132)
            {
                Console.BufferWidth = 132;
            }

            try
            {
                Console.SetWindowSize(132, 40);
                Console.SetBufferSize(132, 40);
            }
            catch (PlatformNotSupportedException)
            {
                // Can only change the window size on Windows.                
            }

            Utils.SetWindowPosition(0, 0);

            Labels.SetupLabelwindow(window);
            TextBoxes.SetupTextBoxwindow(window);
            ListBoxes.SetupListBoxwindow(window);
            KeyPressedEvents.SetupKeyPressedEventwindow(window);
            ProgressBars.SetupProgressBars(window);
            LoginPage.SetupLoginPage(window);
            LoadingPage.SetupLoadingPage(window);
            Menus.SetupMenu(window);

            Showwindow();
        }

        private static void Showwindow()
        {
            for (int i = 0; i < window.Count; i++)
            {
                window.Show(i);
            }
        }
    }
}