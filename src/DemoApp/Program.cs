using ConsoleUI;
using System;
using System.Text;

namespace DemoApp
{
    internal class Program
    {
        private static Window window = new Window();

        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
#if WINDOWS

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
#endif
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