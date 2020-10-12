using EligereES.Models.Extensions;
using System;
using System.Collections.Generic;

namespace EligereES.Models.DB
{
    public partial class VotingTicket
    {
        public VoteTicket GetPersonAttributes() { return VoteTicket.FromJson(Content); }
        public void SetPersonAttributes(PersonAttributes value) { Content = value.ToJson(); }
    }
}
