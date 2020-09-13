using System;
using System.Collections.Generic;

namespace EligereES.Models.DB
{
    public partial class PollingStationCommission
    {
        public PollingStationCommission()
        {
            RelPollingStationPollingStationCommission = new HashSet<RelPollingStationPollingStationCommission>();
        }

        public Guid Id { get; set; }
        public Guid ElectionFk { get; set; }
        public string Location { get; set; }
        public string DigitalLocation { get; set; }
        public string Description { get; set; }

        public virtual Election ElectionFkNavigation { get; set; }
        public virtual ICollection<RelPollingStationPollingStationCommission> RelPollingStationPollingStationCommission { get; set; }
    }
}
