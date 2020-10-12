using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EligereVS.Models
{
    public class BallotContent
    {
        public string ToJson()
        {
            return JsonSerializer.Serialize<BallotContent>(this);
        }

        public static BallotContent FromJson(string data)
        {
            return JsonSerializer.Deserialize<BallotContent>(data);
        }

        public string ElectionId { get; set; }
        public string SecureVote { get; set; }
    }
}
