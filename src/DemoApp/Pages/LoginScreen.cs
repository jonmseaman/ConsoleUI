using ConsoleUI;

namespace DemoApp
{
    internal static class LoginPage
    {
        internal static void SetupLoginPage(Window window)
        {
            ListBoxPopup(window);
        }

        private static void ListBoxPopup(Window window)
        {
            var page = new ConsoleUI.LoginPage();
            page.Username = "admin";

            page.Footer.Text = "Try admin admin.";

            window.Add(page);

            page.Login += Page_Login;
        }

        private static void Page_Login(object sender, LoginEventArgs e)
        {
            System.Threading.Thread.Sleep(2000);

            e.Success = (e.Username == "admin" & e.Password == "admin");

            if (!e.Success)
                e.FailureMessage = "Take the hint.";
        }
    }
}