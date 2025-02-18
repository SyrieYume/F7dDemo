using System.Runtime.InteropServices;


class Win32API {
    public const UInt32 STD_OUTPUT_HANDLE = 0xfffffff5;
    public const UInt32 ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x4;
    public const UInt32 CP_UTF8 = 65001;

    [StructLayout(LayoutKind.Sequential)]
    public struct COORD { public Int16 X, Y; }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct CONSOLE_FONT_INFOEX {
        public Int32 cbSize;
        public UInt32 nFont;
        public COORD dwFontSize;
        public UInt32 FontFamily;
        public UInt32 FontWeight;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string FaceName;
    }

    [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
    public static extern int SetConsoleOutputCP(UInt32 wCodePageID);

    [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
    public static extern UInt64 GetStdHandle(UInt32 nStdHandle);

    [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
    public static extern unsafe int GetConsoleMode(UInt64 handle, ref UInt32 lpMode);

    [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
    public static extern int SetConsoleMode(UInt64 handle, UInt32 dwMode);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern int SetCurrentConsoleFontEx(UInt64 hConsoleOutput, int bMaximumWindow, ref CONSOLE_FONT_INFOEX lpConsoleCurrentFontEx);
}