using Newtonsoft.Json;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace RestApiLib.HTTP
{
    public class HttpServer : IDisposable
    {
        private HttpListener server = new HttpListener();
        private string ipport = "";
        private Dictionary<string, Func<string>> PrefixesEvent = new Dictionary<string, Func<string>>();
        private string f_json_setting = "setting.json";
        private HttpServerSetting httpServerSetting = new HttpServerSetting();
        public HttpServer()
        {









        }

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



        public void Map(string pattern, Func<string> RequestDelegate)
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



                    Console.WriteLine($"Aдрес приложения: {request}");
                    Console.WriteLine($"{localpath_}");

                    if (PrefixesEvent.ContainsKey(localpath_))
                    {

                        response.WriteAsyncString(PrefixesEvent[localpath_]());
                        continue;

                    }




                    response.WriteAsyncString("Error 202");
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }

            }


        }
    }
}
