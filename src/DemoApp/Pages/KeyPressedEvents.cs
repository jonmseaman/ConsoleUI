using NetCoreTUI.Controls;
using NetCoreTUI.Enums;
using NetCoreTUI.Screens;

namespace DemoApp
{
    internal static class KeyPressedEvents
    {
        internal static void SetupKeyPressedEventwindow(Window window)
        {
            ListBoxPopup(window);
        }

        private static void ListBoxPopup(Window window)
        {
            var page = new Page("List Box Pop Up");

            MenuBar menuBar = Menus.SetupMenuBar(page);
            
            var control1 = new ListBox();

            control1.Left = 0;
            control1.Top = 1;
            control1.Width = page.Width;
            control1.Height = page.Height - 2;
            control1.BorderStyle = BorderStyle.Double;

            for (int i = 0; i < 150; i++)
            {
                control1.Items.Add(string.Format("Item {0} - Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua", i + 1));
            }

            page.Controls.Add(control1);

            var textBox = new TextBox();
            textBox.BorderStyle = BorderStyle.Double;
            textBox.Width = 8;
            textBox.Left = (page.Width / 2) - (textBox.Width / 2);
            textBox.Top = (page.Height / 2) - (textBox.Height / 2);
            textBox.MaxLength = 6;
            textBox.BackgroundColor = System.ConsoleColor.DarkGreen;
            textBox.FocusBackgroundColor = System.ConsoleColor.DarkGreen;
            textBox.Visible = false;
            textBox.TreatEnterKeyAsTab = false;
            textBox.HasShadow = true;

            page.Controls.Add(textBox);

            page.Footer.Text = page.Name + ". Press C to popup a text box, enter or escape.";

            page.Controls.Add(menuBar);

            window.Add(page);

            control1.KeyPressed += (s, e) =>
            {
                if (e.Info.Key == System.ConsoleKey.C)
                {
                    textBox.Visible = true;
                    textBox.Focus();
                    e.Handled = true;
                }
            };

            control1.Selected += (s, e) =>
            {
                page.Footer.Text = control1.SelectedItem;
            };

            textBox.TextChanged += (s, e) =>
            {
                page.Footer.Text = string.Format("{0} => {1}", e.OrignalText ?? string.Empty, e.NewText);
            };

            textBox.Leave += (s, e) =>
            {
                //textBox.Visible = false;
            };

            control1.Enter += (s, e) =>
            {
                textBox.Visible = false;
            };
        }
    }
}