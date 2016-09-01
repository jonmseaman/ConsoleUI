using ConsoleUI;

namespace DemoApp
{
    internal static class ListBoxes
    {
        internal static void SetupListBoxwindow(Window window)
        {
            BasicListBoxPage(window);
            SingleBorderListBoxPage(window);
            DoubleBorderListBoxPage(window);
        }

        private static void BasicListBoxPage(Window window)
        {
            var page = new Page("Basic List Boxes");
            
            var control1 = new ListBox();

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = 20;
            control1.Height = 10;
            
            for (int i = 0; i < 20; i++)
            {
                control1.Items.Add(string.Format("Item {0}", i + 1));
            }

            var control2 = new ListBox();

            control2.Left = 30;
            control2.Top = 0;
            control2.Width = 30;
            control2.Height = 15;

            for (int i = 0; i < 40; i++)
            {
                control2.Items.Add(string.Format("Item {0}", i + 1));
            }

            var control3 = new ListBox();

            control3.Left = 0;
            control3.Top = 11;
            control3.Width = 25;
            control3.Height = 10;

            for (int i = 0; i < 5; i++)
            {
                control3.Items.Add(string.Format("Item {0}", i + 1));
            }
            
            page.Controls.Add(control1);
            page.Controls.Add(control2);
            page.Controls.Add(control3);

            page.Footer.Text = page.Name + ". Press enter or escape.";

            window.Add(page);
        }
        private static void SingleBorderListBoxPage(Window window)
        {
            var page = new Page("Single Border List Boxes");
            page.SuspendLayout();

            var control1 = new ListBox();

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = 20;
            control1.Height = 10;
            control1.BorderStyle = BorderStyle.Single;

            for (int i = 0; i < 20; i++)
            {
                control1.Items.Add(string.Format("Item {0}", i + 1));
            }

            var control2 = new ListBox();

            control2.Left = 30;
            control2.Top = 0;
            control2.Width = 30;
            control2.Height = 15;
            control2.BorderStyle = BorderStyle.Single;

            for (int i = 0; i < 40; i++)
            {
                control2.Items.Add(string.Format("Item {0}", i + 1));
            }

            var control3 = new ListBox();

            control3.Left = 0;
            control3.Top = 11;
            control3.Width = 25;
            control3.Height = 10;
            control3.BorderStyle = BorderStyle.Single;

            for (int i = 0; i < 5; i++)
            {
                control3.Items.Add(string.Format("Item {0}", i + 1));
            }

            page.Controls.Add(control1);
            page.Controls.Add(control2);
            page.Controls.Add(control3);

            page.Footer.Text = page.Name + ". Press enter or escape.";

            window.Add(page);
        }
        private static void DoubleBorderListBoxPage(Window window)
        {
            var page = new Page("Double Border List Boxes");

            var control1 = new ListBox();

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = 20;
            control1.Height = 10;
            control1.BorderStyle = BorderStyle.Double;
            control1.HasShadow = true;

            for (int i = 0; i < 20; i++)
            {
                control1.Items.Add(string.Format("Item {0}", i + 1));
            }

            var control2 = new ListBox();

            control2.Left = 30;
            control2.Top = 0;
            control2.Width = 30;
            control2.Height = 15;
            control2.BorderStyle = BorderStyle.Double;
            control2.HasShadow = true;

            for (int i = 0; i < 40; i++)
            {
                control2.Items.Add(string.Format("Item {0}", i + 1));
            }

            var control3 = new ListBox();

            control3.Left = 0;
            control3.Top = 11;
            control3.Width = 25;
            control3.Height = 10;
            control3.BorderStyle = BorderStyle.Double;
            control3.HasShadow = true;

            for (int i = 0; i < 5; i++)
            {
                control3.Items.Add(string.Format("Item {0}", i + 1));
            }

            page.Controls.Add(control1);
            page.Controls.Add(control2);
            page.Controls.Add(control3);

            page.Footer.Text = page.Name + ". Press enter or escape.";

            window.Add(page);
        }
    }
}