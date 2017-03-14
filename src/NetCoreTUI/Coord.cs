using System.Runtime.InteropServices;

namespace NetCoreTUI
{
    public static partial class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short x, short y)
            {
                X = x;
                Y = y;
            }
        };
    }
}