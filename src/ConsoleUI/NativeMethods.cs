using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleUI
{
    public static class NativeMethods
    {
        /// <summary>
        /// Paints the buffer on the console window.
        /// </summary>
        /// <param name="buffer">The buffer which will be painted on the Console window.</param>
        internal static void Paint(Buffer buffer)
        {
            var prevLeft = Console.CursorLeft;
            var prevTop = Console.CursorTop;
            var prevFg = Console.ForegroundColor;
            var prevBg = Console.BackgroundColor;

            var sz = buffer.Size;
            var pos = buffer.Coord;
            var reg = buffer.Rectangle;

            var index = 0;
            for (var y = pos.Y; y < pos.Y + sz.Y; y++)
            {
                Console.SetCursorPosition(pos.X, y);
                for (var x = pos.X; x < pos.Y + sz.X; x++)
                {
                    // TODO: Allow bottom right.
                    if (reg.Left <= x && x < reg.Right && reg.Top <= y && y < reg.Bottom && index != buffer.Value.Length - 1)
                    {
                        var output = buffer.Value[index++];
                        if (Console.ForegroundColor != output.ForegroundColor)
                            Console.ForegroundColor = output.ForegroundColor;
                        if (Console.BackgroundColor != output.BackgroundColor)
                            Console.BackgroundColor = output.BackgroundColor;
                        Console.Write(output.Char);
                    }
                }
            }
            Console.SetCursorPosition(prevLeft, prevTop);
            Console.ForegroundColor = prevFg;
            Console.BackgroundColor = prevBg;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }
    }
}