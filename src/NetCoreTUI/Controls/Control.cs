using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NetCoreTUI.Buffers;
using NetCoreTUI.Enums;
using NetCoreTUI.EventArgs;

namespace NetCoreTUI.Controls
{
    public class Control : INotifyPropertyChanged
    {
        public ConsoleColor BackgroundColor = ConsoleColor.Blue;
        public ConsoleColor FocusBackgroundColor = ConsoleColor.Blue;
        public ConsoleColor FocusForegroundColor = ConsoleColor.Gray;
        public ConsoleColor ForegroundColor = ConsoleColor.Gray;

        public bool HasShadow = false;
        protected int X;
        protected int Y;
        private BorderStyle _borderStyle;

        private char _doubleBorderBottomLeft = (char)0x255A;
        private char _doubleBorderBottomRight = (char)0x255D;
        private char _doubleBorderHorizontal = (char)0x2550;
        private char _doubleBorderTopLeft = (char)0x2554;
        private char _doubleBorderTopRight = (char)0x2557;
        private char _doubleBorderVertical = (char)0x2551;

        private bool _hasFocus;
        private int _height;
        private int _left;
        private IControlContainer _owner;

        private ConsoleBuffer _preserved;
        private char _singleBorderBottomLeft = (char)0x2514;
        private char _singleBorderBottomRight = (char)0x2518;
        private char _singleBorderHorizontal = (char)0x2500;
        private char _singleBorderTopLeft = (char)0x250C;
        private char _singleBorderTopRight = (char)0x2510;
        private char _singleBorderVertical = (char)0x2502;

        private int _tabOrder;
        private bool _tabStop;
        private string _text;
        private TextAlign _textAlign;
        private int _top;
        private bool _visible;

        private int _width;

        public Control()
        {
            LayoutSuspended = true;

            Height = 1;
            Visible = true;

            PropertyChanged += (s, e) =>
            {
                HandlePropertyChanged(e);
            };
        }

        public event EventHandler AfterPaint;

        public event EventHandler<CancelEventArgs> BeforePaint;

        public event EventHandler Enter;

        public event EventHandler EscPressed;

        public event EventHandler Leave;

        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<TabEventArgs> TabPressed;

        public BorderStyle BorderStyle
        {
            get
            {
                return _borderStyle;
            }
            set
            {
                if (value != BorderStyle.None && Height < 3)
                    Height = 3;

                SetProperty(ref _borderStyle, value);
            }
        }

        public int Bottom
        {
            get
            {
                return Top + Height - 1;
            }
        }

        public int ClientBottom
        {
            get
            {
                return ClientTop + ClientHeight - 1;
            }
        }

        public int ClientHeight
        {
            get
            {
                return Height - (Offset * 2);
            }
        }

        public int ClientLeft
        {
            get
            {
                return Left + Offset;
            }
        }

        public int ClientRight
        {
            get
            {
                return ClientLeft + ClientWidth - 1;
            }
        }

        public int ClientTop
        {
            get
            {
                return Top + Offset;
            }
        }

        public virtual int ClientWidth
        {
            get
            {
                return Width - (Offset * 2);
            }
        }

        public bool HasFocus
        {
            get
            {
                return _hasFocus;
            }
            set
            {
                SetProperty(ref _hasFocus, value);
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                SetProperty(ref _height, value);
            }
        }

        public int Left
        {
            get
            {
                return _left;
            }
            set
            {
                SetProperty(ref _left, value);
            }
        }

        public IControlContainer Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                SetProperty(ref _owner, value);
            }
        }

        public int Right
        {
            get
            {
                return Left + Width - 1;
            }
        }

        public int TabOrder
        {
            get
            {
                return _tabOrder;
            }
            set
            {
                SetProperty(ref _tabOrder, value);
            }
        }

        public virtual bool TabStop
        {
            get
            {
                return _tabStop;
            }
            set
            {
                SetProperty(ref _tabStop, value);
            }
        }

        public virtual string Text
        {
            get
            {
                return _text;
            }
            set
            {
                SetProperty(ref _text, value);
            }
        }

        public TextAlign TextAlign
        {
            get
            {
                return _textAlign;
            }
            set
            {
                SetProperty(ref _textAlign, value);
            }
        }

        public int Top
        {
            get
            {
                return _top;
            }
            set
            {
                SetProperty(ref _top, value);
            }
        }

        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                SetProperty(ref _visible, value);
            }
        }

        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                SetProperty(ref _width, value);
            }
        }

        protected bool LayoutSuspended
        {
            get;
            set;
        }

        protected int Offset
        {
            get
            {
                return BorderStyle == BorderStyle.None ? 0 : 1;
            }
        }

        protected bool ShouldDraw
        {
            get
            {
                if (LayoutSuspended)
                    return false;

                if (!Visible)
                    return false;

                if (Owner == null)
                    return false;

                return true;
            }
        }

        public virtual void Draw()
        {
            if (!ShouldDraw)
                return;

            DrawBackground();
            DrawBorder();
            DrawControl();
            DrawShadow();
        }

        public void Focus()
        {
            if (!TabStop)
                return;

            HasFocus = true;

            OnEnter();
        }

        public void Hide()
        {
            Visible = false;
        }

        public void ResumeLayout()
        {
            LayoutSuspended = false;
        }

        public void Show()
        {
            Visible = true;
        }

        public void SuspendLayout()
        {
            LayoutSuspended = true;
        }

        internal virtual void Paint()
        {
            if (!ShouldDraw)
                return;

            var args = new CancelEventArgs();            

            OnBeforePaint(args);

            if (args.Cancel)
                return;

            Owner.Buffer.Paint();

            OnAfterPaint();
        }

        internal void PreserveArea(int left, int top, int height, int width)
        {
            if (Owner == null)
                return;

            _preserved = ConsoleBuffer.CreateBuffer(left, top, height, width);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var sourceIndex = ((top + y) * Owner.Buffer.Width) + (left + x);
                    var targetIndex = (y * width) + x;

                    var info = Owner.Buffer.GetCharInfo(x, y);
                    _preserved.SetCharInfo(x, y, info);
                    //_preserved.Value[targetIndex] = Owner.Buffer.Value[sourceIndex];
                }
            }
        }

        internal void RestoreArea()
        {
            if (Owner == null)
                return;

            if (_preserved == null)
                return;

            for (int y = 0; y < _preserved.Height; y++)
            {
                for (int x = 0; x < _preserved.Width; x++)
                {
                    var tx = (_preserved.Left + x);
                    var ty = (_preserved.Top + y);

                    var targetIndex = ((_preserved.Top + y) * Owner.Buffer.Width) + (_preserved.Left + x);
                    var sourceIndex = (y * _preserved.Width) + x;

                    Owner.Buffer.SetCharInfo(tx, ty, _preserved.GetCharInfo(x, y));
                    //Owner.Buffer.Value[targetIndex] = _preserved.Value[sourceIndex];
                }
            }

            
            Owner.Buffer.Paint();
        }

        protected virtual void Blur()
        {
            if (!TabStop)
                return;

            HasFocus = false;

            OnLeave();
        }

        protected virtual void DrawBackground()
        {
            if (!ShouldDraw)
                return;

            for (int x = Left; x < Right + 1; x++)
            {
                for (int y = Top; y < Bottom + 1; y++)
                {
                    Owner.Buffer.Write(x, y, ' ', ForegroundColor, BackgroundColor);
                }
            }
        }

        protected virtual void DrawBorder()
        {
            if (!ShouldDraw)
                return;

            if (BorderStyle == BorderStyle.None)
                return;

            if (BorderStyle == BorderStyle.Single)
                DrawSingleBorder();

            if (BorderStyle == BorderStyle.Double)
                DrawDoubleBorder();
        }

        protected virtual void DrawControl()
        {
            if (!ShouldDraw)
                return;

            DrawText();
        }

        protected virtual void DrawShadow()
        {
            if (!HasShadow)
                return;

            if (!ShouldDraw)
                return;

            var right = Right + 1;
            var bottom = Bottom + 1;

            if (bottom < Owner.Buffer.Region.Bottom)
                for (int i = Left + 1; i <= Right + 1; i++)
                {
                    if (i < Owner.Buffer.Region.Right)
                        Owner.Buffer.Write(i, bottom, ConsoleColor.DarkGray, ConsoleColor.Black);
                }

            if (right < Owner.Buffer.Region.Right)
                for (int i = Top + 1; i <= Bottom; i++)
                {
                    if (i < Owner.Buffer.Region.Bottom)
                        Owner.Buffer.Write(i, bottom, ConsoleColor.DarkGray, ConsoleColor.Black);
                }
        }

        protected virtual void DrawText()
        {
            if (!ShouldDraw)
                return;

            Write(Text);
        }

        protected virtual void HandlePropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Height" || e.PropertyName == "Width" || e.PropertyName == "Left" || e.PropertyName == "Top")
            {
                Draw();
                Paint();
            }

            if (e.PropertyName == "Visible")
            {
                if (Visible)
                {
                    Draw();
                    Paint();
                }
            }
        }

        protected virtual void OnAfterPaint()
        {
            AfterPaint?.Invoke(this, new System.EventArgs());
        }

        protected virtual void OnBeforePaint(CancelEventArgs args)
        {
            BeforePaint?.Invoke(this, args);
        }

        protected virtual void OnEnter()
        {
            Enter?.Invoke(this, new System.EventArgs());
        }

        protected virtual void OnEscPressed()
        {
            EscPressed?.Invoke(this, new System.EventArgs());
        }

        protected virtual void OnLeave()
        {
            Leave?.Invoke(this, new System.EventArgs());
        }

        /// <summary>
        ///     Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">
        ///     Name of the property used to notify listeners.  This
        ///     value is optional and can be provided automatically when invoked from compilers
        ///     that support <see cref="CallerMemberNameAttribute" />.
        /// </param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler eventHandler = PropertyChanged;

            eventHandler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnTabPressed(bool shift)
        {
            TabPressed?.Invoke(this, new TabEventArgs(shift));
        }

        protected virtual string OnTruncateText(string text)
        {
            text = text.Remove(ClientWidth - 3, text.Length - ClientWidth + 3);
            text += "...";

            return text;
        }

        protected virtual void OnWrite(int x, int y, string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            if (Owner == null)
                return;

            Owner.Buffer.Write((short)x, (short)y, text, foregroundColor, backgroundColor);
        }

        protected virtual void ReadKey()
        {
        }

        /// <summary>
        ///     Checks if a property already matches a desired value.  Sets the property and
        ///     notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">
        ///     Name of the property used to notify listeners.  This
        ///     value is optional and can be provided automatically when invoked from compilers that
        ///     support CallerMemberName.
        /// </param>
        /// <returns>
        ///     True if the value was changed, false if the existing value matched the
        ///     desired value.
        /// </returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void Write(string text)
        {
            if (!ShouldDraw)
                return;

            if (ClientWidth < 1)
                return;

            if (text == null)
                text = string.Empty;

            if (text.Length > ClientWidth)
            {
                text = OnTruncateText(text);
            }

            switch (TextAlign)
            {
                case TextAlign.Left:
                    {
                        text = text.PadRight(ClientWidth);

                        break;
                    }
                case TextAlign.Center:
                    {
                        var padding = (ClientWidth - text.Length) / 2;

                        text = string.Format("{0}{1}{0}", new string(' ', padding), text);

                        text = text.PadLeft(ClientWidth);

                        break;
                    }
                case TextAlign.Right:
                    {
                        text = text.PadLeft(ClientWidth);

                        break;
                    }
            }

            OnWrite(X + ClientLeft, Y + ClientTop, text, HasFocus ? FocusForegroundColor : ForegroundColor, HasFocus ? FocusBackgroundColor : BackgroundColor);
        }

        private void DrawDoubleBorder()
        {
            if (!ShouldDraw)
                return;

            Owner.Buffer.Write((short)Left, (short)Top, _doubleBorderTopLeft, ForegroundColor, BackgroundColor);
            Owner.Buffer.Write((short)Right, (short)Top, _doubleBorderTopRight, ForegroundColor, BackgroundColor);
            Owner.Buffer.Write((short)Left, (short)Bottom, _doubleBorderBottomLeft, ForegroundColor, BackgroundColor);
            Owner.Buffer.Write((short)Right, (short)Bottom, _doubleBorderBottomRight, ForegroundColor, BackgroundColor);

            for (int i = Left + 1; i < Right; i++)
            {
                Owner.Buffer.Write((short)i, (short)Top, _doubleBorderHorizontal, ForegroundColor, BackgroundColor);
                Owner.Buffer.Write((short)i, (short)Bottom, _doubleBorderHorizontal, ForegroundColor, BackgroundColor);
            }

            for (int i = Top + 1; i < Bottom; i++)
            {
                Owner.Buffer.Write((short)Left, (short)i, _doubleBorderVertical, ForegroundColor, BackgroundColor);
                Owner.Buffer.Write((short)Right, (short)i, _doubleBorderVertical, ForegroundColor, BackgroundColor);
            }
        }

        private void DrawSingleBorder()
        {
            if (!ShouldDraw)
                return;

            Owner.Buffer.Write((short)Left, (short)Top, _singleBorderTopLeft, ForegroundColor, BackgroundColor);
            Owner.Buffer.Write((short)Right, (short)Top, _singleBorderTopRight, ForegroundColor, BackgroundColor);
            Owner.Buffer.Write((short)Left, (short)Bottom, _singleBorderBottomLeft, ForegroundColor, BackgroundColor);
            Owner.Buffer.Write((short)Right, (short)Bottom, _singleBorderBottomRight, ForegroundColor, BackgroundColor);

            for (int i = Left + 1; i < Right; i++)
            {
                Owner.Buffer.Write((short)i, (short)Top, _singleBorderHorizontal, ForegroundColor, BackgroundColor);
                Owner.Buffer.Write((short)i, (short)Bottom, _singleBorderHorizontal, ForegroundColor, BackgroundColor);
            }

            for (int i = Top + 1; i < Bottom; i++)
            {
                Owner.Buffer.Write((short)Left, (short)i, _singleBorderVertical, ForegroundColor, BackgroundColor);
                Owner.Buffer.Write((short)Right, (short)i, _singleBorderVertical, ForegroundColor, BackgroundColor);
            }
        }
    }
}