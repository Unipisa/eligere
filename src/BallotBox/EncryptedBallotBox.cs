using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography;
using System.Text.Json;

namespace EligereVS.BallotBox
{
    public class GuardianSecret
    {
        public string? Secret { get; set; }
        public int NumOfGuardians { get; set; }
        public int Quorum { get; set; }
    }

    public class SecureBallotBox : IBallotBox
    {
        private const string MetadataStore = "MetadataStore";
        private const string VotesStore = "VotesStore";
        private const string TicketStore = "TicketStore";

        private const string Metadata_LastOpened = "LastOpened";
        private const string Metadata_Log = "Log";
        private const string Metadata_Key = "Key";
        private const string Metadata_Tally = "Tally";

        private PersistentStore _bbox;
        private IDataProtectionProvider _dataProtector;

        public static IBallotBox CreateSecureBallotBox(IDataProtectionProvider protector, string path, string ballotBoxName)
        {
            return new SecureBallotBox(protector, path, ballotBoxName);
        }

        private void WriteMetadataLog(string log)
        {
            if (_bbox.IsStoreOpen(MetadataStore))
            {
                var meta = _bbox[MetadataStore];
                lock(meta)
                {
                    Dictionary<string, string> logdata;
                    if (meta.Get(Metadata_Log) == null)
                    {
                        logdata = new Dictionary<string, string>();
                    }
                    else
                    {
                        logdata = JsonSerializer.Deserialize<Dictionary<string, string>>(meta.Get(Metadata_Log));
                    }

                    // Ensure key is unique by including the line number
                    logdata.Add($"({logdata.Count}) " + DateTime.UtcNow.ToString(), log);
                    meta.Put(Metadata_Log,JsonSerializer.Serialize(logdata));
                }
            }
        }

        private string createNonce(int length)
        {
            var data = new byte[length];
            using (var rnd = RandomNumberGenerator.Create())
            {
                rnd.GetBytes(data);
            }
            return Convert.ToBase64String(data);
        }

        private SecureBallotBox(IDataProtectionProvider protector, string path, string ballotBoxName)
        {
            _dataProtector = protector;
            _bbox = new(path, ballotBoxName, MetadataStore, VotesStore, TicketStore);
            _bbox.Open();

            var meta = _bbox[MetadataStore];
            lock (this)
            {
                if (meta.Get(Metadata_Key) == null)
                {
                    var secret = new GuardianSecret() { Secret = createNonce(1024), NumOfGuardians = 0, Quorum = 0 };
                    meta.Put(Metadata_Key, JsonSerializer.Serialize(secret));
                }
                meta.Put(Metadata_LastOpened, JsonSerializer.Serialize(DateTime.UtcNow));
                WriteMetadataLog($"BallotBox accessed {DateTime.UtcNow}");
            }
        }


        public bool IsBallotBoxOpen => throw new NotImplementedException();

        public string[] GetGuardianKeys(int numberOfGuardians, int quorum)
        {
            var meta = _bbox[MetadataStore];
            lock (this)
            {
                if (meta.Get(Metadata_Key) == null)
                    throw new Exception("Internal error: ballotbox not containing secret");

                var secret = JsonSerializer.Deserialize<GuardianSecret>(meta.Get(Metadata_Key));

                if (secret.NumOfGuardians != 0 && secret.NumOfGuardians != numberOfGuardians)
                    throw new Exception($"Internal error: Guardian keys already generated with {secret.NumOfGuardians} instead of {numberOfGuardians}");

                if (secret.NumOfGuardians == 0)
                {
                    secret.NumOfGuardians = numberOfGuardians;
                    secret.Quorum = quorum;
                    meta.Put(Metadata_Key, JsonSerializer.Serialize(secret));
                }

                var content = secret.Secret;
                var stride = content.Length / numberOfGuardians;
                var protector = _dataProtector.CreateProtector("GuardianKeys");
                var slices = new List<string>();
                for (var i = 0; i < numberOfGuardians - 1; i++)
                {
                    var s = protector.Protect(i.ToString() + "#" + content.Substring(i * stride, stride));
                    slices.Add(s);
                }
                slices.Add(protector.Protect((numberOfGuardians - 1) + "#" + content.Substring((numberOfGuardians - 1) * stride)));
                return slices.ToArray();
            }
        }

        private bool TestKeys(string[] keys)
        {
            var meta = _bbox[MetadataStore];
            lock (this)
            {
                if (meta.Get(Metadata_Key) == null)
                    throw new Exception("Internal error: ballotbox not containing secret");

                var secret = JsonSerializer.Deserialize<GuardianSecret>(meta.Get(Metadata_Key));

                if (secret.NumOfGuardians == 0)
                    throw new Exception("Internal error: guardian keys not generated");

                var content = secret.Secret;
                var protector = _dataProtector.CreateProtector("GuardianKeys");
                var decslices = new string[secret.NumOfGuardians];
                foreach (var s in keys)
                {
                    var cs = protector.Unprotect(s);
                    var sep = cs.IndexOf("#");
                    if (sep == -1 || sep == cs.Length - 1) return false;
                    int idx;
                    if (!int.TryParse(cs.Substring(0, sep), out idx))
                        return false;
                    decslices[idx] = cs.Substring(sep + 1);
                }

                var deccont = String.Join("", decslices);
                return content == deccont;
            }
        }

        public CastVoteResult CastVote(IDataProtectionProvider dataprotection, VoteTicket ticket, string[] voteContent)
        {
            if (voteContent == null || voteContent.Length == 0)
                throw new ArgumentException("Internal error: Empty vote content");

            var protector = dataprotection.CreateProtector("SecureBallotBox");
            var voteId = Guid.NewGuid().ToString();

            var ticketsDb = _bbox[TicketStore];
            var secureVoteStore = _bbox[VotesStore];
            var meta = _bbox[MetadataStore];

            lock (this)
            {
                if (meta.Get(Metadata_Tally) != null)
                    return CastVoteResult.BallotBoxTallied;

                if (ticketsDb.Get(ticket.HashId) != null)
                    return CastVoteResult.TicketAlreadyUsed;

                ticketsDb.Put(ticket.HashId, ticket.ToJson());

                foreach (var v in voteContent)
                {
                    var secureVote = protector.Protect(v);
                    var vote = new VoteContent() { ElectionId = ticket.ElectionId, SecureVote = secureVote };
                    secureVoteStore.Put(voteId, vote.ToJson());
                }
            }
            return CastVoteResult.VoteCasted;
        }

        public void Close()
        {
            _bbox.Close();
        }

        public bool IsTicketUsed(VoteTicket ticket)
        {
            lock (this)
            {
                var ticketsDb = _bbox[TicketStore];
                return ticketsDb.Get(ticket.HashId) != null;
            }
        }

        public Dictionary<string, Dictionary<string, int>>? Tally(string[] keys)
        {
            var result = new Dictionary<string, Dictionary<string, int>>();

            lock (this)
            {
                if (!TestKeys(keys))
                    return null;

                var protector = _dataProtector.CreateProtector("SecureBallotBox");
                var ballots = new List<VoteContent>();
                var meta = _bbox[MetadataStore];
                var secureVoteStore = _bbox[VotesStore];

                if (meta.Get(Metadata_Tally) == null)
                {
                    meta.Put(Metadata_Tally, DateTime.UtcNow.ToString());
                }
                WriteMetadataLog("Tally performed");
                using (var it = secureVoteStore.NewIterator())
                {
                    it.SeekToFirst();
                    while (it.Valid())
                    {
                        ballots.Add(VoteContent.FromJson(it.StringValue()));
                        it.Next();
                    }
                }

                var clearBallots = ballots.ConvertAll(b => (b.ElectionId, protector.Unprotect(b.SecureVote)));

                foreach (var ballot in clearBallots.GroupBy(b => b.ElectionId))
                {
                    var count = ballot.GroupBy(b => b.Item2);
                    foreach (var c in count)
                    {
                        if (ballot.Key != null && !result.ContainsKey(ballot.Key))
                            result.Add(ballot.Key, new Dictionary<string, int>());

                        result[ballot.Key].Add(c.Key, c.Count());
                    }
                }
            }
            return result;
        }
    }
}
