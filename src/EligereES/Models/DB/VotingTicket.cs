using System;
using System.Collections.Generic;

namespace EligereES.Models.DB
{
    public partial class VotingTicket
    {
        public VotingTicket()
        {
            Voter = new HashSet<Voter>();
        }

        public Guid Id { get; set; }
        public string Hash { get; set; }
        public string Content { get; set; }
        public Guid? VoterFk { get; set; }

        public virtual ICollection<Voter> Voter { get; set; }
    }
}
