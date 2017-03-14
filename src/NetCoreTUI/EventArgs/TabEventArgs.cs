namespace NetCoreTUI.EventArgs
{
    public class TabEventArgs : System.EventArgs
    {
        public bool Shift { get; set; }

        public TabEventArgs()
        {

        }

        public TabEventArgs(bool shift)
        {
            Shift = shift;
        }
    }
}