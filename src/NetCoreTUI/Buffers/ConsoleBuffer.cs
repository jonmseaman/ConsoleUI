using System;
using ConsoleUI;

namespace NetCoreTUI.Buffers
{
    public abstract class ConsoleBuffer
    {
        protected ConsoleBuffer(int left, int top, int height, int width)
        {
            Left = left;
            Top = top;
            Height = height;
            Width = width;
            Region = new SmallRect()
            {
                Bottom = (short)(top + height),
                Left = (short)left,
                Right = (short)(left + width),
                Top = (short)top
            };
        }

        public int Top { get; }
        public int Left { get; }
        public int Width { get; }
        public int Height { get; }
        public Coord Size => new Coord((short)Width, (short)Height);
        public Coord Position => new Coord((short)Left, (short)Top);
        public SmallRect Region { get; }


        public abstract void SetCharInfo(int x, int y, ConsoleCharInfo charInfo);
        public abstract ConsoleCharInfo GetCharInfo(int x, int y);
        public abstract void Paint();

        #region Write
        public virtual void Write(int x, int y, ConsoleColor fg)
        {
            var charInfo = GetCharInfo(x, y);
            charInfo.ForegroundColor = fg;
            SetCharInfo(x, y, charInfo);
        }

        public virtual void Write(int x, int y, ConsoleColor fg, ConsoleColor bg)
        {
            var charInfo = GetCharInfo(x, y);
            charInfo.ForegroundColor = fg;
            charInfo.BackgroundColor = bg;
            SetCharInfo(x, y, charInfo);
        }

        public virtual void Write(int x, int y, char c)
        {
            var charInfo = GetCharInfo(x, y);
            charInfo.Char = c;
            SetCharInfo(x, y, charInfo);
        }

        public virtual void Write(int x, int y, char c, ConsoleColor fg)
        {
            var charInfo = new ConsoleCharInfo()
            {
                Char = c,
                ForegroundColor = fg,
                BackgroundColor = GetCharInfo(x, y).BackgroundColor
            };
            SetCharInfo(x, y, charInfo);
        }

        public virtual void Write(int x, int y, char c, ConsoleColor fg, ConsoleColor bg)
        {
            var charInfo = new ConsoleCharInfo()
            {
                Char = c,
                ForegroundColor = fg,
                BackgroundColor = bg
            };
            SetCharInfo(x, y, charInfo);
        }

        public virtual void Write(int x, int y, string text)
        {
            for (var i = 0; i < text.Length && x < Width; i++, x++)
            {
                var charInfo = GetCharInfo(x, y);
                charInfo.Char = text[i];
                SetCharInfo(x, y, charInfo);
            }
        }

        public virtual void Write(int x, int y, string text, ConsoleColor fg)
        {
            for (var i = 0; i < text.Length && x < Width; i++, x++)
            {
                var charInfo = new ConsoleCharInfo()
                {
                    Char = text[i],
                    ForegroundColor = fg,
                    BackgroundColor = GetCharInfo(x, y).BackgroundColor
                };
                SetCharInfo(x, y, charInfo);
            }
        }

        public virtual void Write(int x, int y, string text, ConsoleColor fg, ConsoleColor bg)
        {
            for (var i = 0; i < text.Length && x < Width; i++, x++)
            {
                var charInfo = new ConsoleCharInfo()
                {
                    Char = text[i],
                    ForegroundColor = fg,
                    BackgroundColor = bg
                };
                SetCharInfo(x, y, charInfo);
            }
        }
        #endregion

        public const char TestChar = (char)0x2562;

        public static ConsoleBuffer CreateBuffer(int left, int top, int height, int width)
        {
#if WINDOWS
            var buffer = new WindowsBuffer(left, top, height, width);
#else
            var buffer = new NetCoreBuffer(left, top, height, width);
#endif
            return buffer;
        }
    }
}