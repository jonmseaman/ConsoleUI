using ConsoleUI;

namespace DemoApp
{
    internal static class Menus
    {
        internal static void SetupMenu(Window window)
        {
            var page = new Page("Menus");
            MenuBar menuBar = SetupMenuBar(page);

            page.Controls.Add(menuBar);

            window.Add(page);
        }

        internal static MenuBar SetupMenuBar(Page page)
        {
            var menuBar = new MenuBar(page);
            menuBar.Left = 0;
            menuBar.Top = 0;
            menuBar.Width = page.Width;

            SetupFileMenu(page, menuBar);
            SetupEditMenu(page, menuBar);
            SetupViewMenu(page, menuBar);

            return menuBar;
        }

        private static void SetupEditMenu(Page page, MenuBar menuBar)
        {
            var menu = new Menu(page);
            menu.Text = "Edit";

            AddMenuItem(menu, "Cut");
            AddMenuItem(menu, "Copy");
            menu.AddSeparator();
            AddMenuItem(menu, "Paste");

            menuBar.Menus.Add(menu);
        }

        private static void SetupFileMenu(Page page, MenuBar menuBar)
        {
            var menu = new Menu(page);
            menu.Text = "File";

            AddMenuItem(menu, "New");
            AddMenuItem(menu, "Open");
            menu.AddSeparator();
            AddMenuItem(menu, "Save");
            menu.AddSeparator();
            AddMenuItem(menu, "Close");

            menuBar.Menus.Add(menu);
        }

        private static void AddMenuItem(Menu menu, string text)
        {
            var menuItem = menu.AddMenuItem(text);

            menuItem.Selected += (s, e) =>
            {
                ((Page)menu.Owner).SetFooterText("Selected: " + ((MenuItem)s).Text);
            };

            menuItem.Enter += (s, e) =>
            {
                ((Page)menu.Owner).SetFooterText("Enter: " + ((MenuItem)s).Text);
            };
        }

        private static void SetupViewMenu(Page page, MenuBar menuBar)
        {
            var menu = new Menu(page);
            menu.Text = "Animals";
            
            AddMenuItem(menu, "Dog");
            AddMenuItem(menu, "Cat");
            menu.AddSeparator();
            AddMenuItem(menu, "Mouse");
            menu.AddSeparator();
            AddMenuItem(menu, "Monkey");
            menu.AddSeparator();
            AddMenuItem(menu, "Horse");
            menu.AddSeparator();
            AddMenuItem(menu, "Hummingbird hawk-moth");

            menuBar.Menus.Add(menu);
        }
    }
}