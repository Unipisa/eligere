using System;
using System.Collections.Generic;

#nullable disable

namespace EligereES.Models.DB
{
    public partial class Election
    {
        public Election()
        {
            BallotName = new HashSet<BallotName>();
            ElectionStaff = new HashSet<ElectionStaff>();
            IdentificationCommissionerAffinityRel = new HashSet<IdentificationCommissionerAffinityRel>();
            PollingStationCommission = new HashSet<PollingStationCommission>();
            Voter = new HashSet<Voter>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Configuration { get; set; }
        public DateTime PollStartDate { get; set; }
        public DateTime PollEndDate { get; set; }
        public bool Active { get; set; }
        public DateTime? ElectorateListClosingDate { get; set; }
        public Guid? ElectionTypeFk { get; set; }
        public Guid? PollingStationGroupId { get; set; }
        public bool Configuring { get; set; }

        public virtual ElectionType ElectionTypeFkNavigation { get; set; }
        public virtual ICollection<BallotName> BallotName { get; set; }
        public virtual ICollection<ElectionStaff> ElectionStaff { get; set; }
        public virtual ICollection<IdentificationCommissionerAffinityRel> IdentificationCommissionerAffinityRel { get; set; }
        public virtual ICollection<PollingStationCommission> PollingStationCommission { get; set; }
        public virtual ICollection<Voter> Voter { get; set; }
    }
}
