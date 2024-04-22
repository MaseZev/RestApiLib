using RestApiLib;
using RestApiLib.HTTP;
using System.Collections.Generic;
using System.IO;
using System.Text;

using (HttpServer sv = new HttpServer())
{

    sv.Setting();
    sv.Init();
    sv.MapFile((HttpContext context, QueryParametrs queryParametrs) =>
    {
        Console.WriteLine(context.LocalPath);
        return File.ReadAllText(context.LocalPath);
    });
    sv.MapError((HttpContext context, QueryParametrs queryParametrs) =>
    {

        return File.ReadAllText(context.LocalPath);
    });
    sv.Map("/", (HttpContext context, QueryParametrs queryParametrs) =>
    {

        Console.WriteLine(context.LocalPath);


        return "Абракадабра!";
    });


    sv.Run();

}
