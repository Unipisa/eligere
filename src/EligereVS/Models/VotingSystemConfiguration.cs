using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EligereVS.Models
{
    public class VotingSystemConfiguration
    {
        public static VotingSystemConfiguration FromJson(string json)
        {
            return JsonSerializer.Deserialize<VotingSystemConfiguration>(json);
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public string ElectionSystemAPI { get; set; }
        public string GuardianAPI { get; set; }
        public string MediatorAPI { get; set; }

    }
}
