using System.Text.Json;

namespace EligereVS.BallotBox
{
    public class VoteContent
    {
        public string ToJson()
        {
            return JsonSerializer.Serialize<VoteContent>(this);
        }

        public static VoteContent? FromJson(string data)
        {
            return JsonSerializer.Deserialize<VoteContent>(data);
        }

        public string? ElectionId { get; set; }
        public string? SecureVote { get; set; }
    }
}
