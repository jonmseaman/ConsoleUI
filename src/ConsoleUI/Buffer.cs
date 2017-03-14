using System;
using System.Text;

namespace ConsoleUI
{
    public class Buffer
    {
        public struct ConsoleCharInfo
        {
            public char Char;
            public ConsoleColor ForegroundColor;
            public ConsoleColor BackgroundColor;
        }
        public NativeMethods.SmallRect Rectangle;
        private ConsoleCharInfo[] _prevWrite;
        private ConsoleCharInfo[] buffer;

        public Buffer(int left, int top, int height, int width)
        {
            Left = left;
            Top = top;
            Height = height;
            Width = width;

            _prevWrite = new ConsoleCharInfo[width * height];
            buffer = new ConsoleCharInfo[width * height];

            Rectangle = new NativeMethods.SmallRect() { Top = (short)Top, Left = (short)Left, Bottom = (short)(Top + Height), Right = (short)(Left + Width) };
        }

        public NativeMethods.Coord Position
        {
            get
            {
                return new NativeMethods.Coord((short)Left, (short)Top);
            }
        }

        public int Height { get; private set; }

        public int Left { get; private set; }

        public NativeMethods.Coord Size
        {
            get
            {
                return new NativeMethods.Coord((short)Width, (short)Height);
            }
        }

        public int Top { get; private set; }

        public ConsoleCharInfo[] Value
        {
            get
            {
                return buffer;
            }
        }

        public int Width { get; private set; }

        public ConsoleColor GetBackgroundColor(int x, int y)
        {
            var index = (Width * y) + x;

            if (index < buffer.Length)
            {
                return buffer[index].BackgroundColor;
            }

            return ConsoleColor.Black;
        }

        public ConsoleColor GetForegroundColor(int x, int y)
        {
            var index = (Width * y) + x;

            if (index < buffer.Length)
            {
                return buffer[index].ForegroundColor;
            }

            return ConsoleColor.White;
        }

        public void SetColor(int x, int y, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            var index = (Width * y) + x;

            if (index < buffer.Length)
            {
                SetColor(index, foregroundColor, backgroundColor);
            }
        }

        public void Write(int x, int y, char c, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            var index = (Width * y) + x;

            if (index < buffer.Length)
            {
                SetColor(x, y, foregroundColor, backgroundColor);
                buffer[index].Char = c;
            }
        }

        public void Write(int x, int y, char c, ConsoleColor foregroundColor)
        {
            var backgroundColor = GetBackgroundColor(x, y);

            Write(x, y, c, foregroundColor, backgroundColor);
        }

        public void Write(int x, int y, string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            if (string.IsNullOrEmpty(text))
                return;

            for (int i = 0; i < text.Length; i++)
            {
                var index = (Width * y) + x + i;

                if (index < buffer.Length)
                {
                    buffer[index].Char = text[i];
                    buffer[index].ForegroundColor = foregroundColor;
                    buffer[index].BackgroundColor = backgroundColor;
                }
            }
        }

        internal void SetColor(int index, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            if (index < buffer.Length)
            {
                buffer[index].ForegroundColor = foregroundColor;
                buffer[index].BackgroundColor = backgroundColor;
            }
        }

        /// <summary>
        /// Paints the buffer on the console window.
        /// </summary>
        public void Paint()
        {
            var prevLeft = Console.CursorLeft;
            var prevTop = Console.CursorTop;
            var prevFg = Console.ForegroundColor;
            var prevBg = Console.BackgroundColor;

            var sz = Size;
            var pos = Position;
            var reg = Rectangle;

            var index = 0;
            for (var y = pos.Y; y < pos.Y + sz.Y; y++)
            {
                Console.SetCursorPosition(pos.X, y);
                for (var x = pos.X; x < pos.Y + sz.X; x++)
                {
                    // TODO: Allow bottom right.
                    if (reg.Left <= x && x < reg.Right && reg.Top <= y && y < reg.Bottom && index != buffer.Length - 1)
                    {
                        var output = buffer[index++];
                        if (output.Equals(_prevWrite[index])) continue;
                        if (Console.ForegroundColor != output.ForegroundColor)
                            Console.ForegroundColor = output.ForegroundColor;
                        if (Console.BackgroundColor != output.BackgroundColor)
                            Console.BackgroundColor = output.BackgroundColor;
                        if (Console.CursorTop != y || Console.CursorLeft != x)
                                Console.SetCursorPosition(x, y);
                        Console.Write(output.Char);
                        _prevWrite[index] = output;
                    }
                }
            }
            Console.SetCursorPosition(prevLeft, prevTop);
            Console.ForegroundColor = prevFg;
            Console.BackgroundColor = prevBg;
        }
    }
}