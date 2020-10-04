using System;
using System.Collections.Generic;

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

        public virtual Person PersonFkNavigation { get; set; }
        public virtual PollingStationCommission PollingStationCommissionFkNavigation { get; set; }
        public virtual ICollection<PollingStationCommission> PollingStationCommission { get; set; }
    }
}
