using System;

namespace NetCoreTUI.EventArgs
{
    public class KeyPressedEventArgs : System.EventArgs
    {
        private ConsoleKeyInfo _info;

        public KeyPressedEventArgs(ConsoleKeyInfo info)
        {
            _info = info;
        }

        public bool Handled
        {
            get;
            set;
        }

        public ConsoleKeyInfo Info
        {
            get
            {
                return _info;
            }
        }
    }
}