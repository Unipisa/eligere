using System;
using System.Collections.Generic;

namespace EligereES.Models.DB
{
    public partial class RelPollingStationSystemPollingStationCommission
    {
        public Guid Id { get; set; }
        public Guid PollingStationCommissionFk { get; set; }
        public Guid PollingStationSystemFk { get; set; }

        public virtual PollingStationCommission PollingStationCommissionFkNavigation { get; set; }
        public virtual PollingStationSystem PollingStationSystemFkNavigation { get; set; }
    }
}
