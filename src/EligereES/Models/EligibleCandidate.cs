using System;
using System.Collections.Generic;

namespace EligereES.Models
{
    public partial class EligibleCandidate
    {
        public Guid Id { get; set; }
        public Guid PersonFk { get; set; }
        public Guid ElectionFk { get; set; }

        public virtual Election ElectionFkNavigation { get; set; }
        public virtual Person PersonFkNavigation { get; set; }
    }
}
