using System;
using System.Text;

namespace NetCoreTUI.Buffers
{
    internal class NetCoreBuffer : ConsoleBuffer
    {
        public SmallRect Rectangle;

        /// <summary>
        /// Buffer which stores the current state of the console window.
        /// </summary>
        private static ConsoleCharInfo[] _windowBuffer;

        private static int _windowWidth;
        private static int _windowHeight;

        public NetCoreBuffer(int left, int top, int height, int width) : base(left, top, height, width)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(TestChar);
            if (_windowBuffer == null)
            {
                _windowHeight = Console.WindowHeight;
                _windowWidth = Console.WindowWidth;
                _windowBuffer = new ConsoleCharInfo[_windowWidth * _windowWidth];
            }
            Value = new ConsoleCharInfo[width * height];

            Rectangle = new SmallRect() { Top = (short)Top, Left = (short)Left, Bottom = (short)(Top + Height), Right = (short)(Left + Width) };
        }

        public ConsoleCharInfo[] Value { get; }

        public ConsoleColor GetBackgroundColor(int x, int y)
        {
            var index = (Width * y) + x;

            if (index < Value.Length)
            {
                return Value[index].BackgroundColor;
            }

            return ConsoleColor.Black;
        }

        public ConsoleColor GetForegroundColor(int x, int y)
        {
            var index = (Width * y) + x;

            if (index < Value.Length)
            {
                return Value[index].ForegroundColor;
            }

            return ConsoleColor.White;
        }

        /// <summary>
        /// Paints the buffer on the console window.
        /// </summary>
        public override void Paint()
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
                for (var x = pos.X; x < pos.Y + sz.X; x++, index++)
                {
                    // TODO: Allow bottom right.
                    if (reg.Left <= x && x < reg.Right && reg.Top <= y && y < reg.Bottom && index != Value.Length - 1)
                    {
                        var output = Value[index];
                        if (output.Equals(GetCachedCharInfo(x, y))) continue;
                        if (Console.ForegroundColor != output.ForegroundColor)
                            Console.ForegroundColor = output.ForegroundColor;
                        if (Console.BackgroundColor != output.BackgroundColor)
                            Console.BackgroundColor = output.BackgroundColor;
                        if (Console.CursorTop != y || Console.CursorLeft != x)
                                Console.SetCursorPosition(x, y);
                        Console.Write(output.Char);
                        CacheCharInfo(x, y, output);
                    }
                }
            }
            Console.SetCursorPosition(prevLeft, prevTop);
            Console.ForegroundColor = prevFg;
            Console.BackgroundColor = prevBg;
        }

        private static void CacheCharInfo(int x, int y, ConsoleCharInfo info)
        {
            _windowBuffer[_windowWidth * y + x] = info;
        }

        private static ConsoleCharInfo GetCachedCharInfo(int x, int y)
        {
            return _windowBuffer[_windowWidth * y + x];
        }

        public override void SetCharInfo(int x, int y, ConsoleCharInfo charInfo)
        {
            Value[y * Width + x] = charInfo;
        }

        public override ConsoleCharInfo GetCharInfo(int x, int y)
        {
            return Value[y * Width + x];
        }
    }
}