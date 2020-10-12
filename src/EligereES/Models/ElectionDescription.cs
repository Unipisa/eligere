using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EligereES.Models
{
    public class ElectionCandidate
    {
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PublicId { get; set; }
    }

    public class ElectionDescription
    {
        public string ElectionId { get; set; }
        public string ElectionName { get; set; }
        public DateTime PollStartDate { get; set; }
        public DateTime PollEndDate { get; set; }
        public List<ElectionCandidate> Candidates { get; set; }
    }
}
