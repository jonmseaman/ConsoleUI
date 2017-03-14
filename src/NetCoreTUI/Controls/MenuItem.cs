using System;

namespace NetCoreTUI.Controls
{
    public class MenuItem : Control
    {
        private bool _isSeparator;

        public MenuItem(IControlContainer owner)
        {
            Owner = owner;
            Height = 1;
            TabStop = true;

            FocusBackgroundColor = ConsoleColor.Red;
            FocusForegroundColor = ConsoleColor.White;
        }

        public event EventHandler Selected;

        public bool IsSeparator
        {
            get
            {
                return _isSeparator;
            }
            set
            {
                SetProperty(ref _isSeparator, value);
            }
        }

        public override bool TabStop
        {
            get
            {
                if (IsSeparator)
                    return false;

                return base.TabStop;
            }

            set
            {
                if (!IsSeparator)
                    base.TabStop = value;
            }
        }

        internal void Select()
        {
            OnSelected();
        }

        protected virtual void OnSelected()
        {
            Selected?.Invoke(this, new System.EventArgs());
        }
    }
}