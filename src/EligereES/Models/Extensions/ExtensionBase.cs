using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EligereES.Models.Extensions
{
    public class Extension<T>
    {
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, typeof(T));
        }

        public static T FromJson(string data)
        {
            return JsonSerializer.Deserialize<T>(data);
        }

    }
}
