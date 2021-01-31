using System;
using System.Collections.Generic;

#nullable disable

namespace EligereES.Models.DB
{
    public partial class UserLoginRequest
    {
        public Guid Id { get; set; }
        public string Provider { get; set; }
        public string UserId { get; set; }
        public Guid PersonFk { get; set; }
        public bool Approved { get; set; }
        public DateTime? Approval { get; set; }
        public Guid? Approver { get; set; }

        public virtual Person PersonFkNavigation { get; set; }
    }
}
