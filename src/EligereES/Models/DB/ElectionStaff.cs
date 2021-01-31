using System;
using System.Collections.Generic;

#nullable disable

namespace EligereES.Models.DB
{
    public partial class ElectionStaff
    {
        public Guid Id { get; set; }
        public Guid PersonFk { get; set; }
        public Guid ElectionRoleFk { get; set; }
        public Guid? ElectionFk { get; set; }

        public virtual Election ElectionFkNavigation { get; set; }
        public virtual ElectionRole ElectionRoleFkNavigation { get; set; }
        public virtual Person PersonFkNavigation { get; set; }
    }
}
