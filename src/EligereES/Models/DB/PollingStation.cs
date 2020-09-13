using System;
using System.Collections.Generic;

namespace EligereES.Models.DB
{
    public partial class PollingStation
    {
        public PollingStation()
        {
            RelPollingStationPollingStationCommission = new HashSet<RelPollingStationPollingStationCommission>();
        }

        public Guid Id { get; set; }
        public string Ipaddress { get; set; }
        public string DigitalFootprint { get; set; }

        public virtual ICollection<RelPollingStationPollingStationCommission> RelPollingStationPollingStationCommission { get; set; }
    }
}
