using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


// Windows控制台相关函数
partial class Win32Console {
    private const string ESC = "\x1B";
    private const string RESET = $"{ESC}[0m";

    private static readonly Func<int,int,int,string> RGB = (r, g, b) => $"{ESC}[38;2;{r};{g};{b}m";

    private static readonly Dictionary<string, string> Seqs = new() {
        { "reset", $"{ESC}[0m" },
        { "blue", RGB(76,186,250) },
        { "green", RGB(96,200,135) },
        { "orange", RGB(96,200,135) },
        { "b", RGB(76,186,250) },
        { "g", RGB(96,200,135) },
        { "r", RGB(247,101,104) }
    };

    [GeneratedRegex("<([a-z]+)>(.*?)</\\1>")]
    private static partial Regex SeqsRegex();

    // 用例参考: Colorful.WriteLine("Hello, <b> World </b>")
    public static void WriteLine(string text) {
        string processed = SeqsRegex().Replace(text, match => 
            $"{ Seqs[match.Groups[1].Value] }{ match.Groups[2].Value }{ RESET }"
        );
        Console.WriteLine(processed);
    }


    // 开启虚拟终端序列
    public static bool EnableVTMode() {
        UInt64 hConsole = Win32API.GetStdHandle(Win32API.STD_OUTPUT_HANDLE);
        UInt32 dwMode = 0;
        
        Win32API.GetConsoleMode(hConsole, ref dwMode);

        dwMode |= Win32API.ENABLE_VIRTUAL_TERMINAL_PROCESSING;
        if(Win32API.SetConsoleMode(hConsole, dwMode) == 0)
            return false;

        return true;
    }

    // 设置控制台字体
    public static bool SetFont(string fontName, Int16 fontSize) {
        UInt64 hConsole = Win32API.GetStdHandle(Win32API.STD_OUTPUT_HANDLE);

        var cfi = new Win32API.CONSOLE_FONT_INFOEX {
            cbSize = Marshal.SizeOf<Win32API.CONSOLE_FONT_INFOEX>(),
            dwFontSize = new Win32API.COORD { X = (short)(fontSize / 2), Y = fontSize },
            FaceName = fontName
        };

        return Win32API.SetCurrentConsoleFontEx(hConsole, 0, ref cfi) != 0;
    }
}