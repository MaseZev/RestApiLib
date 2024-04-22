using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestApiLib.HTTP
{
    public class HttpServerSetting
    {

        private static int _deffoult_port = 5001;
        public Dictionary<string, (string, int)> IPAdress = new Dictionary<string, (string, int)>()
        {

            { "WINDOWS" , ("http://127.0.0.1" , _deffoult_port ) },
            { "LINUX" ,  ("http://62.109.19.41" , _deffoult_port) },
        };
        public string GetData()
        {
            (string, int) obj;
            string os__ = Util.GetOS_System();
            if (IPAdress.TryGetValue(os__, out obj))
            {
                return $"{obj.Item1}:{obj.Item2}";
            }
            return $"http://127.0.0.1:{_deffoult_port}";
        }
    }
}
