using System;
using System.Collections.Generic;

namespace EligereES.Models.DB
{
    public partial class PollingStationCommissioner
    {
        public Guid Id { get; set; }
        public Guid PersonFk { get; set; }
        public Guid PollingStationCommissionFk { get; set; }

        public virtual Person PersonFkNavigation { get; set; }
    }
}
