using Microsoft.AspNetCore.DataProtection;

namespace EligereVS.BallotBox
{
    public interface IBallotBox
    {
        public void Close();
        public string[] GetGuardianKeys(int numberOfGuardians, int quorum);
        public CastVoteResult CastVote(IDataProtectionProvider protector, VoteTicket ticket, string[] voteContent);
        public bool IsTicketUsed(VoteTicket ticket);
        public Dictionary<string, Dictionary<string, int>>? Tally(string[] keys);
        public bool IsBallotBoxOpen { get;  }
    }
}