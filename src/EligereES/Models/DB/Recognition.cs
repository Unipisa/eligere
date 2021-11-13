using System;
using System.Collections.Generic;

namespace EligereES.Models.DB
{
    public partial class Recognition
    {
        public Recognition()
        {
            Voter = new HashSet<Voter>();
        }

        public Guid Id { get; set; }
        public DateTime? Validity { get; set; }
        public string Otp { get; set; }
        public string Idnum { get; set; }
        public DateTime? Idexpiration { get; set; }
        public string Idtype { get; set; }
        public string AccountProvider { get; set; }
        public string UserId { get; set; }
        /// <summary>
        /// 0:Login;1:Recognized;2:Activated(byOTP)
        /// </summary>
        public int State { get; set; }
        public bool? RemoteIdentification { get; set; }

        public virtual ICollection<Voter> Voter { get; set; }
    }
}
