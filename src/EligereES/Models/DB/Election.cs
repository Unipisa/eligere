using System;
using System.Collections.Generic;

namespace EligereES.Models.DB
{
    public partial class Election
    {
        public Election()
        {
            ElectionStaff = new HashSet<ElectionStaff>();
            EligibleCandidate = new HashSet<EligibleCandidate>();
            PollingStationCommission = new HashSet<PollingStationCommission>();
            Voter = new HashSet<Voter>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Configuration { get; set; }
        public DateTime PollStartDate { get; set; }
        public DateTime PollEndDate { get; set; }
        public DateTime? ElectorateListClosingDate { get; set; }
        public Guid? ElectionTypeFk { get; set; }

        public virtual ElectionType ElectionTypeFkNavigation { get; set; }
        public virtual ICollection<ElectionStaff> ElectionStaff { get; set; }
        public virtual ICollection<EligibleCandidate> EligibleCandidate { get; set; }
        public virtual ICollection<PollingStationCommission> PollingStationCommission { get; set; }
        public virtual ICollection<Voter> Voter { get; set; }
    }
}
