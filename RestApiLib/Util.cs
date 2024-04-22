using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RestApiLib
{
    public static class Util
    {
        public static string GetLocalPath(this HttpListenerRequest httpListenerRequest)
        {
            return $"{httpListenerRequest.Url.LocalPath}";
        }
        public static OSPlatform[] OS_SystemList
        {
            get
            {
                return new OSPlatform[] { OSPlatform.Linux, OSPlatform.Windows, OSPlatform.FreeBSD };
            }
        }
        public static string GetOS_System()
        {

            foreach (OSPlatform item in OS_SystemList)
            {
                if (RuntimeInformation.IsOSPlatform(item))
                    return item.ToString();
            }
            return string.Empty;
        }

        public static void WriteAsyncString(this HttpListenerResponse response, string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            response.ContentLength64 = buffer.Length;
            using Stream output = response.OutputStream;
            output.WriteAsync(buffer);
            output.FlushAsync();
        }
        public static void WriteAsyncByte(this HttpListenerResponse response, byte[] buffer)
        {


            using Stream output = response.OutputStream;
            output.WriteAsync(buffer);
            output.FlushAsync();
        }
    }
}
