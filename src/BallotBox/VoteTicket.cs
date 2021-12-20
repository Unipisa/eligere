using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EligereVS.BallotBox
{
    public class VoteTicket
    {
        public string ToJson()
        {
            return JsonSerializer.Serialize<VoteTicket>(this);
        }

        public static VoteTicket? FromJson(string data)
        {
            return JsonSerializer.Deserialize<VoteTicket>(data);
        }

        public string? HashId { get; set; }
        public string? ElectionId { get; set; }
        public DateTime Issued { get; set; }
        public DateTime Expiration { get; set; }
        public string? ElectionName { get; set; }
    }
}
