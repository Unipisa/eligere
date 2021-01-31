using System;
using System.Collections.Generic;

#nullable disable

namespace EligereES.Models.DB
{
    public partial class PollingStationCommission
    {
        public PollingStationCommission()
        {
            PollingStationCommissioner = new HashSet<PollingStationCommissioner>();
            RelPollingStationSystemPollingStationCommission = new HashSet<RelPollingStationSystemPollingStationCommission>();
            RemoteIdentificationCommissioner = new HashSet<RemoteIdentificationCommissioner>();
        }

        public Guid Id { get; set; }
        public Guid ElectionFk { get; set; }
        public string Location { get; set; }
        public string DigitalLocation { get; set; }
        public string Description { get; set; }
        public Guid? PresidentFk { get; set; }
        public Guid? PollingStationGroupId { get; set; }

        public virtual Election ElectionFkNavigation { get; set; }
        public virtual PollingStationCommissioner PresidentFkNavigation { get; set; }
        public virtual ICollection<PollingStationCommissioner> PollingStationCommissioner { get; set; }
        public virtual ICollection<RelPollingStationSystemPollingStationCommission> RelPollingStationSystemPollingStationCommission { get; set; }
        public virtual ICollection<RemoteIdentificationCommissioner> RemoteIdentificationCommissioner { get; set; }
    }
}
