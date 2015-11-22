﻿using System;

namespace ConsoleUI
{
    public class Buffer
    {
        public NativeMethods.SmallRect Rectangle;
        private NativeMethods.CharInfo[] buffer;

        public Buffer(int left, int top, int height, int width)
        {
            Left = left;
            Top = top;
            Height = height;
            Width = width;

            buffer = new NativeMethods.CharInfo[width * height];

            Rectangle = new NativeMethods.SmallRect() { Top = (short)Top, Left = (short)Left, Bottom = (short)(Top + Height), Right = (short)(Left + Width) };
        }

        public NativeMethods.Coord Coord
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

        public NativeMethods.CharInfo[] Value
        {
            get
            {
                return buffer;
            }
        }

        public int Width { get; private set; }

        public void Write(int x, int y, byte ascii, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            var index = (Width * y) + x;

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

                buffer[index].Char.AsciiChar = ascii;
            }
        }

        public void Write(int x, int y, string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var fc = NativeMethods.ConsoleColorToColorAttribute(foregroundColor, false);
            var bc = NativeMethods.ConsoleColorToColorAttribute(backgroundColor, true);

            for (int i = 0; i < text.Length; i++)
            {
                var index = (Width * y) + x + i;

                if (index < buffer.Length)
                {
                    var attrs = buffer[index].Attributes;

                    attrs &= ~((short)NativeMethods.Color.ForegroundMask);
                    // C#'s bitwise-or sign-extends to 32 bits.
                    attrs = (short)(((uint)(ushort)attrs) | ((uint)(ushort)fc));

                    attrs &= ~((short)NativeMethods.Color.BackgroundMask);
                    // C#'s bitwise-or sign-extends to 32 bits.
                    attrs = (short)(((uint)(ushort)attrs) | ((uint)(ushort)bc));

                    buffer[index].Attributes = attrs;

                    buffer[index].Char.AsciiChar = (byte)text[i];
                }
            }
        }
    }
}