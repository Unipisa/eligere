using System;
using System.Collections.Generic;

namespace EligereES.Models.DB
{
    public partial class UserLogin
    {
        public Guid Id { get; set; }
        public string Provider { get; set; }
        public string UserId { get; set; }
        public Guid PersonFk { get; set; }
        public DateTime? LastLogin { get; set; }

        public virtual Person PersonFkNavigation { get; set; }
    }
}
