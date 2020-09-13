using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EligereES.Models.DB;
using EligereES.Models.Extensions;

namespace EligereES.Controllers
{
    public class SetupController : Controller
    {
        private readonly ESDB _context;

        public SetupController(ESDB ctxt)
        {
            _context = ctxt;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Dbg()
        {
            var r = _context.ElectionType.First(q => q.Id == Guid.Parse("159bb357-6141-45fd-a474-24bf336aa92e"));
            var conf = new ElectionConfiguration()
            {
                Notes = null,
                EligibleSeats = 1,
                RoundQuorum = 0.1,
                RoundQuorumType = QuorumType.PotentialVoters,
                ValidityQuorum = 0.2,
                ValidityQuorumType = QuorumType.PotentialVoters,
                ElectionQuorum = 0.5,
                ElectionQuorumType = QuorumType.PotentialVoters,
                WeightedVoters = true
            };
            r.DefaultConfiguration = conf.ToJson();
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
