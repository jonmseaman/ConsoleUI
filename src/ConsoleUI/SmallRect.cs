using System.Runtime.InteropServices;

namespace ConsoleUI
{
    public static partial class NativeMethods
    {
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