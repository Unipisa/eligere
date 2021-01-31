using System;
using System.Collections.Generic;

#nullable disable

namespace EligereES.Models.DB
{
    public partial class ElectionRole
    {
        public ElectionRole()
        {
            ElectionStaff = new HashSet<ElectionStaff>();
        }

        public Guid Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ElectionStaff> ElectionStaff { get; set; }
    }
}
