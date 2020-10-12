using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EligereVS.Models
{
    public class VoteInformation
    {
        public List<ElectionDescription> ElectionDescription { get; set; }
        public List<VoteTicket> AvailableTickets { get; set; }
        public List<VoteTicket> UsedTickets { get; set; }
    }
}
