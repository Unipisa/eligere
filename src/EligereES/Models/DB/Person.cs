using System;
using System.Collections.Generic;

#nullable disable

namespace EligereES.Models.DB
{
    public partial class Person
    {
        public Person()
        {
            ElectionStaff = new HashSet<ElectionStaff>();
            EligibleCandidate = new HashSet<EligibleCandidate>();
            IdentificationCommissionerAffinityRel = new HashSet<IdentificationCommissionerAffinityRel>();
            Log = new HashSet<Log>();
            PollingStationCommissioner = new HashSet<PollingStationCommissioner>();
            RemoteIdentificationCommissioner = new HashSet<RemoteIdentificationCommissioner>();
            UserLogin = new HashSet<UserLogin>();
            UserLoginRequest = new HashSet<UserLoginRequest>();
            Voter = new HashSet<Voter>();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PublicId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string Attributes { get; set; }

        public virtual ICollection<ElectionStaff> ElectionStaff { get; set; }
        public virtual ICollection<EligibleCandidate> EligibleCandidate { get; set; }
        public virtual ICollection<IdentificationCommissionerAffinityRel> IdentificationCommissionerAffinityRel { get; set; }
        public virtual ICollection<Log> Log { get; set; }
        public virtual ICollection<PollingStationCommissioner> PollingStationCommissioner { get; set; }
        public virtual ICollection<RemoteIdentificationCommissioner> RemoteIdentificationCommissioner { get; set; }
        public virtual ICollection<UserLogin> UserLogin { get; set; }
        public virtual ICollection<UserLoginRequest> UserLoginRequest { get; set; }
        public virtual ICollection<Voter> Voter { get; set; }
    }
}
