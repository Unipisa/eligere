using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligereVS.BallotBox
{
    public enum CastVoteResult
    {
        VoteCasted = 0,
        TicketAlreadyUsed = 1,
        BallotBoxTallied = 2
    }
}
