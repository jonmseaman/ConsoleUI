#if WINDOWS

using System;
using System.Text;
using NetCoreTUI.Buffers;

namespace ConsoleUI
{
    public class WindowsBuffer : ConsoleBuffer
    {
        public SmallRect Rectangle;
        private NativeMethods.CharInfo[] buffer;

        public WindowsBuffer(int left, int top, int height, int width) : base(left, top, height, width)
        {
            buffer = new NativeMethods.CharInfo[width * height];
            Console.OutputEncoding = Encoding.UTF8;
            var cp = NativeMethods.SetConsoleOutputCP(65001);
            Console.WriteLine("Test Char: ");
            Console.WriteLine(TestChar);
            Write(5, 5, TestChar, ConsoleColor.Red, ConsoleColor.Red);
            if (!cp) Console.WriteLine("Did not set the code page.");
            Console.WriteLine(NativeMethods.GetConsoleOutputCP());

            Rectangle = new SmallRect() { Top = (short)Top, Left = (short)Left, Bottom = (short)(Top + Height), Right = (short)(Left + Width) };
        }

        public Coord Coord
        {
            get
            {
                return new Coord((short)Left, (short)Top);
            }
        }

        public NativeMethods.CharInfo[] Value
        {
            get
            {
                return buffer;
            }
        }

        public override void Paint()
        {
            NativeMethods.Paint(this);
        }

        public override void SetCharInfo(int x, int y, ConsoleCharInfo charInfo)
        {
            buffer[GetIndex(x, y)].Char.UnicodeChar = charInfo.Char;
            buffer[GetIndex(x, y)].Char.UnicodeChar = (char)(y * Width + x);
            buffer[GetIndex(x, y)].Char.UnicodeChar = TestChar;
            SetColor(x, y, charInfo.ForegroundColor, charInfo.BackgroundColor);
        }

        public override ConsoleCharInfo GetCharInfo(int x, int y)
        {
            var native = buffer[GetIndex(x, y)];
            var c = new ConsoleCharInfo
            {
                ForegroundColor = GetForegroundColor(x, y),
                BackgroundColor = GetBackgroundColor(x, y),
                Char = native.Char.UnicodeChar
            };
            return c;
        }

        private int GetIndex(int x, int y)
        {
            var index = (Width * y) + x;
            return index;
        }

        public void SetColor(int x, int y, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            var index = (Width * y) + x;

            if (index < buffer.Length)
            {
                SetColor(index, foregroundColor, backgroundColor);
            }
        }

        internal void SetColor(int index, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            if (index < buffer.Length)
            {
                var fc = NativeMethods.ConsoleColorToColorAttribute(foregroundColor, false);
                var bc = NativeMethods.ConsoleColorToColorAttribute(backgroundColor, true);

                var attrs = buffer[index].Attributes;

                attrs &= ~((short)NativeMethods.Color.ForegroundMask);
                // C#'s bitwise-or sign-extends to 32 bits.
                attrs = (short)(((uint)(ushort)attrs) | ((uint)(ushort)fc));

                attrs &= ~((short)NativeMethods.Color.BackgroundMask);
                // C#'s bitwise-or sign-extends to 32 bits.
                attrs = (short)(((uint)(ushort)attrs) | ((uint)(ushort)bc));

                buffer[index].Attributes = attrs;
            }
        }

        public ConsoleColor GetBackgroundColor(int x, int y)
        {
            var index = (Width * y) + x;

            if (index < buffer.Length)
            {
                var attrs = buffer[index].Attributes;

                return NativeMethods.ColorAttributeToConsoleColor((NativeMethods.Color)attrs & NativeMethods.Color.BackgroundMask);
            }

            return ConsoleColor.Black;
        }

        public ConsoleColor GetForegroundColor(int x, int y)
        {
            var index = (Width * y) + x;

            if (index < buffer.Length)
            {
                var attrs = buffer[index].Attributes;

                return NativeMethods.ColorAttributeToConsoleColor((NativeMethods.Color)attrs & NativeMethods.Color.ForegroundMask);
            }

            return ConsoleColor.Black;
        }
    }
}
#endif
