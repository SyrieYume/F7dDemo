using System.Net;
using System.Web;

class MyHttpServer {
    private readonly HttpListener _listener;
    private readonly string _basePath;

    public uint port;

    // MIME类型映射字典
    private static readonly Dictionary<string, string> MimeTypes = new() {
        { ".jpg",  "image/jpeg" },
        { ".png",  "image/png" },
        { ".gif",  "image/gif" },
        { ".html", "text/html" },
        { ".txt",  "text/plain" },
        { ".yaml", "text/plain" },
        { ".css",  "text/css" },
        { ".js",   "application/javascript" },
        { ".mp3",  "audio/mpeg" },
        { ".ttf",  "font/ttf" },
        { ".svg",  "image/svg+xml" },
    };


    public MyHttpServer(string basePath, uint port) {
        this.port = port;
        while(!CheckPort(this.port)) this.port++;

        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://localhost:{this.port}/");
        _basePath = Path.GetFullPath(basePath);
    }


    // 检查端口是否可用
    public static bool CheckPort(uint port) {
        try {
            HttpListener? listener = null;
            listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{port}/");
            listener.Start();
            listener.Stop();
            return true;
        } catch (Exception ex) {
            return false;
        }
    }

 
    // 启动HTTP服务
    public void Start() {
        _listener.Start();
        Task.Run(Listen);
    }


    // 监听端口并处理请求
    private async Task Listen() {
        try {
            while (_listener.IsListening) {
                var context = await _listener.GetContextAsync();
                ProcessRequest(context);
            }
        }
        catch (Exception ex) {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }


    // 处理HTTP请求
    private async void ProcessRequest(HttpListenerContext context) {
        try {
            var request = context.Request;
            var response = context.Response;

            // 只处理GET请求
            if (request.HttpMethod != "GET") {
                response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                response.Close();
                return;
            }

            // 获取请求路径并转换为本地路径
            string requestPath = request.Url!.AbsolutePath;
            string decodedPath = HttpUtility.UrlDecode(requestPath);
            if(decodedPath == "/")
                decodedPath = "/index.html";
            string localPath = Path.Combine(_basePath, decodedPath.TrimStart('/'));

            Win32Console.WriteLine($"GET <g>{decodedPath}</g>");

            // 防止路径遍历攻击
            if (!localPath.StartsWith(_basePath, StringComparison.Ordinal)) {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Close();
                return;
            }

            // 检查文件是否存在
            if (!File.Exists(localPath)) {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Close();
                return;
            }

            // 设置Content-Type
            string extension = Path.GetExtension(localPath);
            if (MimeTypes.TryGetValue(extension, out string? mimeType))
                response.ContentType = mimeType;
            else
                response.ContentType = "application/octet-stream";

            // 读取文件并发送
            using (FileStream fs = File.OpenRead(localPath)) {
                response.ContentLength64 = fs.Length;
                await fs.CopyToAsync(response.OutputStream);
            }

            response.StatusCode = (int)HttpStatusCode.OK;
            response.Close();
        }
        catch (Exception ex) {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.Close();
            Console.WriteLine($"Request error: {ex.Message}");
        }
    }


    // 关闭HTTP服务
    public void Stop() {
        _listener.Stop();
        _listener.Close();
    }
}