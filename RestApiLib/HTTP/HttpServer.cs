using Newtonsoft.Json;
using System.Net;

namespace RestApiLib.HTTP
{
    public class HttpServer : IDisposable
    {
        private HttpListener server = new HttpListener()
        {
            AuthenticationSchemes = AuthenticationSchemes.Anonymous,
        };
        private string ipport = "";
        private Dictionary<string, Func<HttpContext, QueryParametrs, string>> PrefixesEvent = new Dictionary<string, Func<HttpContext, QueryParametrs, string>>();


        private string f_json_setting = "setting.json";
        public HttpServerSetting httpServerSetting = new HttpServerSetting();

        public void Init()
        {
            ipport = httpServerSetting.GetData();
        }
        public void Setting()
        {
            if (File.Exists(f_json_setting))
            {
                HttpServerSetting json_obj = JsonConvert.DeserializeObject<HttpServerSetting>(File.ReadAllText(f_json_setting));
                if (json_obj != null)
                    httpServerSetting = json_obj;
            }
            else
            {
                string json = JsonConvert.SerializeObject(httpServerSetting, Formatting.Indented);
                File.WriteAllText(f_json_setting, json);
            }
        }


        public void Dispose()
        {
            server.Abort();
            server.Close();
        }

        public void MapFile(Func<HttpContext, QueryParametrs, string> RequestDelegate)
        {

            PrefixesEvent.Add("___file", RequestDelegate);
        }
        public void MapError(Func<HttpContext, QueryParametrs, string> RequestDelegate)
        {

            PrefixesEvent.Add("___error", RequestDelegate);
        }
        public void Map(string pattern, Func<HttpContext, QueryParametrs, string> RequestDelegate)
        {

            string perf_ = $"{ipport}{pattern}";
            if (!perf_.EndsWith("/"))
                perf_ = $"{perf_}/";
            server.Prefixes.Add(perf_);
            PrefixesEvent.Add(pattern, RequestDelegate);
        }

        public void Run()
        {
            server.Start();
            Console.WriteLine($"Сервер запущен по: {ipport}");
            while (true)
            {
                try
                {
                    HttpListenerContext context = server.GetContext();

                    HttpListenerRequest request = context.Request;
                    HttpListenerResponse response = context.Response;
                    string localpath_ = request.GetLocalPath();
                    Console.WriteLine($"Aдрес приложения: {request.RemoteEndPoint}");


                    HttpContext httpContext = new HttpContext
                    {
                        IPEndPointClient = request.RemoteEndPoint,
                        LocalPath = localpath_,
                        httpListenerRequestl = request,
                    };

                    if (!string.IsNullOrEmpty(Path.GetExtension(localpath_)))
                    {
                        if (PrefixesEvent.ContainsKey("___file"))
                        {
                            string full_path_ = "";
                            bool is_error = false;
                            foreach (string path in httpServerSetting.Paths)
                            {
                                full_path_ = Path.GetFullPath(path + localpath_);
                                httpContext.LocalPath = full_path_;
                                if (File.Exists(full_path_))
                                {
                                    if (Path.GetExtension(full_path_) == ".png")
                                    {
                                        FileInfo fileInfo = new FileInfo(full_path_);
                                        response.AddHeader("Content-Type", "image/png");
                                        response.WriteAsyncByte(File.ReadAllBytes(full_path_));
                                    }
                                    else
                                    {
                                        response.WriteAsyncString(PrefixesEvent["___file"](httpContext, new QueryParametrs(request.Url.Query)));
                                    }
                                    is_error = false;
                                }
                                else
                                {
                                    is_error = true;
                                }

                            }
                            if (is_error)
                            {
                                foreach (string path in httpServerSetting.Paths)
                                {
                                    full_path_ = Path.GetFullPath(path + "/error.html");
                                    httpContext.LocalPath = full_path_;
                                    if (File.Exists(full_path_))
                                    {


                                        response.WriteAsyncString(PrefixesEvent["___error"](httpContext, new QueryParametrs(request.Url.Query)));

                                    }


                                }
                            }

                        }
                        continue;
                    }




                    if (PrefixesEvent.ContainsKey(localpath_))
                    {

                        Console.WriteLine(localpath_);
                        response.WriteAsyncString(PrefixesEvent[localpath_](httpContext, new QueryParametrs(request.Url.Query)));
                        continue;

                    }
                    response.WriteAsyncString("Error");
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }

            }


        }
    }
}
