using System;
using System.Collections.Generic;

namespace EligereES.Models
{
    public partial class ElectionType
    {
        public ElectionType()
        {
            Election = new HashSet<Election>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefaultConfiguration { get; set; }

        public virtual ICollection<Election> Election { get; set; }
    }
}
