using System;
using System.Collections.Generic;

namespace EligereES.Models.DB
{
    public partial class IdentificationCommissionerAffinityRel
    {
        public Guid Id { get; set; }
        public Guid ElectionFk { get; set; }
        public Guid PersonFk { get; set; }

        public virtual Election ElectionFkNavigation { get; set; }
        public virtual Person PersonFkNavigation { get; set; }
    }
}
