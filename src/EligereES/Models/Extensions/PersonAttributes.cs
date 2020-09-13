using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace EligereES.Models.Extensions
{
    public class PersonAttributes
    {
        public string ToJson()
        {
            return JsonSerializer.Serialize<PersonAttributes>(this);
        }

        public static PersonAttributes FromJson(string data)
        {
            return JsonSerializer.Deserialize<PersonAttributes>(data);
        }

        public string Role { get; set; }
    }
}
