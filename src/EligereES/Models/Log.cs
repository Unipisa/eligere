using System;
using System.Collections.Generic;

namespace EligereES.Models
{
    public partial class Log
    {
        public Guid Id { get; set; }
        public Guid PersonFk { get; set; }
        public string AccountProvider { get; set; }
        public string UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string LogEntry { get; set; }

        public virtual Person PersonFkNavigation { get; set; }
    }
}
