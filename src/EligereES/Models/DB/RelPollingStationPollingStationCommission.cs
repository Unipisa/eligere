using System;
using System.Collections.Generic;

namespace EligereES.Models.DB
{
    public partial class RelPollingStationPollingStationCommission
    {
        public Guid Id { get; set; }
        public Guid PollingStationCommissionFk { get; set; }
        public Guid PollingStationFk { get; set; }

        public virtual PollingStationCommission PollingStationCommissionFkNavigation { get; set; }
        public virtual PollingStation PollingStationFkNavigation { get; set; }
    }
}
