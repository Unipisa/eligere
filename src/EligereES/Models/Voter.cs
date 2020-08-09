using System;
using System.Collections.Generic;

namespace EligereES.Models
{
    public partial class Voter
    {
        public Guid Id { get; set; }
        public Guid ElectionFk { get; set; }
        public Guid PersonFk { get; set; }
        public DateTime? Vote { get; set; }
        public Guid? RecognitionFk { get; set; }
        public bool? Dropped { get; set; }
        public string DropReason { get; set; }
        public Guid? TicketId { get; set; }

        public virtual Election ElectionFkNavigation { get; set; }
        public virtual Person PersonFkNavigation { get; set; }
        public virtual Recognition RecognitionFkNavigation { get; set; }
    }
}
