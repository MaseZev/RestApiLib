using RestApiLib;
using RestApiLib.HTTP;
using System.Collections.Generic;

using (HttpServer sv = new HttpServer())
{

    sv.Setting();
    sv.Init();

    sv.Map("/", (HttpContext context, QueryParametrs queryParametrs) =>
    {

        return queryParametrs.ToString();
    });
    sv.Map("/os", (HttpContext context, QueryParametrs queryParametrs) =>
    {

        return string.Join(",", from os in Util.OS_SystemList select os.ToString());
    });
    sv.Run();

}
