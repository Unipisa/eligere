using System;
using System.Collections.Generic;

#nullable disable

namespace EligereES.Models.DB
{
    public partial class PollingStationCommissioner
    {
        public PollingStationCommissioner()
        {
            PollingStationCommission = new HashSet<PollingStationCommission>();
        }

        public Guid Id { get; set; }
        public Guid PersonFk { get; set; }
        public Guid PollingStationCommissionFk { get; set; }
        public string VirtualRoom { get; set; }
        public bool AvailableForRemoteRecognition { get; set; }

        public virtual Person PersonFkNavigation { get; set; }
        public virtual PollingStationCommission PollingStationCommissionFkNavigation { get; set; }
        public virtual ICollection<PollingStationCommission> PollingStationCommission { get; set; }
    }
}
