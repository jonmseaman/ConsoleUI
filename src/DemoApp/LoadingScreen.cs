using ConsoleUI;

namespace DemoApp
{
    internal static class LoadingPage
    {
        internal static void SetupLoadingPage(Window window)
        {
            // initialise a new instance of the helper loading Page
            var page = new ConsoleUI.LoadingPage("Loading Page");

            // set the message text
            page.Message = "Doing some work, please wait";

            // each Page has a footer that can display text
            page.Footer.Text = "Working...";

            // add the Page to the window collection
            window.Add(page);

            // there are no controls in the Page that can have the focus so
            // sleep for a bit before exiting.
            page.Shown += (s, e) =>
            {
                System.Threading.Thread.Sleep(3000);

                (s as Page).Exit();
            };
        }
    }
}