using System;
using System.Collections.Generic;

namespace EligereES.Models.DB
{
    public partial class EligibleCandidate
    {
        public Guid Id { get; set; }
        public Guid PersonFk { get; set; }
        public Guid BallotNameFk { get; set; }

        public virtual BallotName BallotNameFkNavigation { get; set; }
        public virtual Person PersonFkNavigation { get; set; }
    }
}
