using System;
using System.Collections.Generic;

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
        public string LogoUri { get; set; }
        public string ElectionFk { get; set; }

        public virtual ICollection<BallotName> BallotName { get; set; }
    }
}
