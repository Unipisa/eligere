using System;
using System.Collections.Generic;

#nullable disable

namespace EligereES.Models.DB
{
    public partial class Party
    {
        public Party()
        {
            BallotName = new HashSet<BallotName>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<BallotName> BallotName { get; set; }
    }
}
