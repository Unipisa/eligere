using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EligereES.Models.Client
{
    public class ESConfiguration
    {
        public static ESConfiguration FromJson(string json)
        {
            return JsonSerializer.Deserialize<ESConfiguration>(json);
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public string VotingSystemTicketAPI { get; set; }

    }
}
