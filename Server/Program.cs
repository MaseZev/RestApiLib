using RestApiLib;
using RestApiLib.HTTP;
using System.Collections.Generic;

using (HttpServer sv = new HttpServer())
{

    sv.Setting();
    sv.Init();

    sv.Map("/", () =>
    {
        Console.WriteLine("sad");
        return "asd";
    });
    sv.Map("/os", () =>
    {

        return string.Join(",", from os in Util.OS_SystemList select os.ToString());
    });
    sv.Run();

}
