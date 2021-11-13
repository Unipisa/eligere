using System;
using System.Collections.Generic;

namespace EligereES.Models.DB
{
    public partial class RemoteIdentificationCommissioner
    {
        public Guid Id { get; set; }
        public Guid PollingStationCommissionFk { get; set; }
        public Guid PersonFk { get; set; }
        public string VirtualRoom { get; set; }
        public bool AvailableForRemoteRecognition { get; set; }

        public virtual Person PersonFkNavigation { get; set; }
        public virtual PollingStationCommission PollingStationCommissionFkNavigation { get; set; }
    }
}
