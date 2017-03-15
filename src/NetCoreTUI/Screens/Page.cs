using System;
using System.ComponentModel;
using NetCoreTUI.Controls;
using Buffer = NetCoreTUI.Buffers.Buffer;

namespace NetCoreTUI.Screens
{
    public class Page : IControlContainer
    {
        public ConsoleColor BackgroundColor = ConsoleColor.Gray;
        public ConsoleColor ForegroundColor = ConsoleColor.DarkGray;
        private readonly Buffer _buffer;
        private readonly ControlCollection<Control> _controls;
        private readonly int _height;
        private readonly int _width;
        private Label _footer;
        private bool _visible;

        public Page(string name) : this(Console.WindowWidth, Console.WindowHeight, name)
        {
        }

        public Page(int width, int height, string name)
        {
            _width = width;
            _height = height;

            Name = name;
            try
            {
                Console.WindowHeight = height;
                Console.WindowWidth = width;
                if (Console.BufferHeight < Console.WindowHeight)
                {
                    Console.BufferHeight = Console.WindowHeight;
                }
                if (Console.BufferWidth < Console.WindowWidth)
                {
                    Console.BufferWidth = Console.WindowWidth;
                }
            }
            catch (PlatformNotSupportedException)
            {
                // TODO: Failed to resize window message box.
                width = Console.WindowWidth;
                height = Console.WindowHeight;
            }

            _buffer = new Buffer(0, 0, height, width);

            Clear();

            _controls = new ControlCollection<Control>(this);
        }

        public event EventHandler AfterPaint;

        public event EventHandler<CancelEventArgs> BeforePaint;

        public event EventHandler Shown;

        public Buffer Buffer
        {
            get
            {
                return _buffer;
            }
        }

        public ControlCollection<Control> Controls
        {
            get
            {
                return _controls;
            }
        }

        public Label Footer
        {
            get
            {
                if (_footer == null)
                {
                    _footer = new Label();

                    _footer.Top = Console.WindowHeight - 1;
                    _footer.Left = Console.WindowLeft;
                    _footer.Width = Console.WindowWidth;

                    _footer.BackgroundColor = BackgroundColor;
                    _footer.ForegroundColor = ForegroundColor;

                    _footer.Owner = this;
                }

                return _footer;
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }
        }

        public string Name { get; set; }

        public bool Visible
        {
            get
            {
                return _visible;
            }
        }

        public int Width
        {
            get
            {
                return _width;
            }
        }

        public void Clear()
        {
            var x = 0;

            while (x < _width)
            {
                var y = 0;

                while (y <= _height)
                {
                    // 32 is space
                    Buffer.Write((short)x, (short)y, ' ', ForegroundColor, BackgroundColor);

                    y++;
                }

                x++;
            }
        }

        public void Exit()
        {
            Hide();

            Controls.Exit();
        }

        public void Hide()
        {
            _visible = false;
        }

        public void Paint()
        {
            var args = new CancelEventArgs();

            OnBeforePaint(args);

            if (args.Cancel)
                return;

            Buffer.Paint();

            OnAfterPaint();
        }

        public void ResumeLayout()
        {
            foreach (var control in Controls)
                control.ResumeLayout();

            Footer.ResumeLayout();
        }

        public void SetFooterText(string text)
        {
            Footer.Text = text;
            _footer.Draw();
            _footer.Paint();
        }

        public virtual void Show()
        {
            ResumeLayout();

            Console.Title = Name;

            Console.CursorVisible = false;

            _visible = true;

            Draw();
            Paint();

            OnShown();

            Controls.SetFocus();
        }

        public void SuspendLayout()
        {
            foreach (var control in Controls)
                control.SuspendLayout();

            Footer.SuspendLayout();
        }

        internal void Show(InputControl focus)
        {
            ResumeLayout();

            Console.Title = Name;

            Console.CursorVisible = false;

            Draw();
            Paint();

            OnShown();

            Controls.SetFocus(focus);
        }

        protected void Draw()
        {
            Clear();

            foreach (var control in Controls)
                control.Draw();

            Footer.Draw();
        }

        protected virtual void OnAfterPaint()
        {
            AfterPaint?.Invoke(this, new System.EventArgs());
        }

        protected virtual void OnBeforePaint(CancelEventArgs args)
        {
            BeforePaint?.Invoke(this, args);
        }

        protected virtual void OnShown()
        {
            _visible = true;

            Shown?.Invoke(this, new System.EventArgs());
        }
    }
}