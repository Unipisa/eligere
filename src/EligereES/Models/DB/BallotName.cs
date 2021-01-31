using System;
using System.Collections.Generic;

#nullable disable

namespace EligereES.Models.DB
{
    public partial class BallotName
    {
        public BallotName()
        {
            EligibleCandidate = new HashSet<EligibleCandidate>();
        }

        public Guid Id { get; set; }
        public string BallotNameLabel { get; set; }
        public Guid ElectionFk { get; set; }
        public Guid? PartyFk { get; set; }
        public int? SequenceOrder { get; set; }

        public virtual Election ElectionFkNavigation { get; set; }
        public virtual Party PartyFkNavigation { get; set; }
        public virtual ICollection<EligibleCandidate> EligibleCandidate { get; set; }
    }
}
