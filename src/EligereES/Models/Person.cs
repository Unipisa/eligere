using System;
using System.Collections.Generic;

namespace EligereES.Models
{
    public partial class Person
    {
        public Person()
        {
            ElectionStaff = new HashSet<ElectionStaff>();
            EligibleCandidate = new HashSet<EligibleCandidate>();
            Log = new HashSet<Log>();
            PollingStationCommissioner = new HashSet<PollingStationCommissioner>();
            Voter = new HashSet<Voter>();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PublicId { get; set; }

        public virtual ICollection<ElectionStaff> ElectionStaff { get; set; }
        public virtual ICollection<EligibleCandidate> EligibleCandidate { get; set; }
        public virtual ICollection<Log> Log { get; set; }
        public virtual ICollection<PollingStationCommissioner> PollingStationCommissioner { get; set; }
        public virtual ICollection<Voter> Voter { get; set; }
    }
}
