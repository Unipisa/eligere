using System;
using System.Collections.Generic;

#nullable disable

namespace EligereES.Models.DB
{
    public partial class TempComm
    {
        public Guid PersonFk { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dipartimento { get; set; }
    }
}
