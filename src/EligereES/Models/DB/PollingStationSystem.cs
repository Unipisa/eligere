using System;
using System.Collections.Generic;

#nullable disable

namespace EligereES.Models.DB
{
    public partial class PollingStationSystem
    {
        public PollingStationSystem()
        {
            RelPollingStationSystemPollingStationCommission = new HashSet<RelPollingStationSystemPollingStationCommission>();
        }

        public Guid Id { get; set; }
        public string Ipaddress { get; set; }
        public string DigitalFootprint { get; set; }

        public virtual ICollection<RelPollingStationSystemPollingStationCommission> RelPollingStationSystemPollingStationCommission { get; set; }
    }
}
