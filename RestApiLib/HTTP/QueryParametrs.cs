using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApiLib.HTTP
{
    public class QueryParametrs
    {
        private Dictionary<string, string> Querys = new Dictionary<string, string>();


        public QueryParametrs(string parametrs)
        {
            if (parametrs.StartsWith("?"))
                parametrs = parametrs.Substring(1);

            foreach (string param in parametrs.Split("&"))
            {
                string[] chart_ = param.Split('=');
                if (chart_.Length == 2)
                    Querys.Add(chart_[0], chart_[1]);

            }



        }
        public override string ToString()
        {
            return string.Join("\n", from item in Querys select $"{item.Key}: {item.Value}");
        }
        public string this[string index]
        {
            get
            {
                if (Querys.ContainsKey(index))
                    return Querys[index];
                return string.Empty;
            }

        }
    }
}
