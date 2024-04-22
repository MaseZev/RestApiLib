using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
namespace RestApiLib.HTTP
{




    public class HttpContext
    {
        public IPEndPoint IPEndPointClient;
        public HttpListenerRequest httpListenerRequestl;
        public string LocalPath = string.Empty;
        public override string ToString()
        {
            return $"{IPEndPointClient.Address.ToString()}";
        }
    }
}
