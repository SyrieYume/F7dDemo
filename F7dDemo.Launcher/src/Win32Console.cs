using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


// Windows控制台相关函数
partial class Win32Console {
    private const string ESC = "\x1B";
    private const string RESET = $"{ESC}[0m";

    private static readonly Func<int, int, int, string> RGB = (r, g, b) => $"{ESC}[38;2;{r};{g};{b}m";

    private static readonly Dictionary<string, string> Seqs = new() {
        { "reset", $"{ESC}[0m" },
        { "blue", RGB(76,186,250) },
        { "green", RGB(96,200,135) },
        { "red", RGB(247,101,104) },
        { "orange", RGB(96,200,135) },
        { "b", RGB(76,186,250) },
        { "g", RGB(96,200,135) },
        { "r", RGB(247,101,104) }
    };

    [GeneratedRegex("<([a-z]+)>(.*?)</\\1>")]
    private static partial Regex SeqsRegex();


    // 用例参考: Win32Console.WriteLine("Hello, <b> World </b>")
    public static void WriteLine(string text) {
        string processed = SeqsRegex().Replace(text, match =>
            $"{Seqs[match.Groups[1].Value]}{match.Groups[2].Value}{RESET}"
        );
        Console.WriteLine(processed);
    }


    // 开启虚拟终端序列
    public static bool EnableVTMode() {
        ulong hConsole = Win32API.GetStdHandle(Win32API.STD_OUTPUT_HANDLE);
        uint dwMode = 0;

        Win32API.GetConsoleMode(hConsole, ref dwMode);

        dwMode |= Win32API.ENABLE_VIRTUAL_TERMINAL_PROCESSING;
        if (Win32API.SetConsoleMode(hConsole, dwMode) == 0)
            return false;

        return true;
    }


    // 设置控制台字体
    public static bool SetFont(string fontName, short fontSize) {
        Win32API.SetConsoleOutputCP(Win32API.CP_UTF8);

        ulong hConsole = Win32API.GetStdHandle(Win32API.STD_OUTPUT_HANDLE);

        var cfi = new Win32API.CONSOLE_FONT_INFOEX {
            cbSize = Marshal.SizeOf<Win32API.CONSOLE_FONT_INFOEX>(),
            dwFontSize = new Win32API.COORD { X = (short)(fontSize / 2), Y = fontSize },
            FaceName = fontName
        };

        return Win32API.SetCurrentConsoleFontEx(hConsole, 0, ref cfi) != 0;
    }


    // 使用到的Win32API
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
        public static extern Int32 SetConsoleOutputCP(UInt32 wCodePageID);

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt64 GetStdHandle(UInt32 nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern unsafe Int32 GetConsoleMode(UInt64 handle, ref UInt32 lpMode);

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 SetConsoleMode(UInt64 handle, UInt32 dwMode);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 SetCurrentConsoleFontEx(UInt64 hConsoleOutput, int bMaximumWindow, ref CONSOLE_FONT_INFOEX lpConsoleCurrentFontEx);
    }
}