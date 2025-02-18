Win32API.SetConsoleOutputCP(Win32API.CP_UTF8);
Console.Clear();
Win32Console.SetFont("SimHei", 18);
try {
    Console.BufferWidth = 90;
    Console.WindowWidth = 90;
    Console.WindowHeight = 25;
}catch { }

Win32Console.EnableVTMode();

var server = new MyHttpServer("./", 3051);
server.Start();

Win32Console.WriteLine("<g>永远的7日之都(Demo)</g> 的HTTP服务已启动...");
Win32Console.WriteLine($"请在浏览器打开 <b>http://localhost:{server.port}</b>");
Win32Console.WriteLine("按任意键结束程序...");
Console.ReadKey(true);

server.Stop();
Win32Console.WriteLine("程序已 <r>关闭</r>.");