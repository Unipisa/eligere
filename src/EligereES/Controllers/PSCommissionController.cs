using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Web;
using EligereES.Models;
using EligereES.Models.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace EligereES.Controllers
{
    [Route("PSCommission")]
    [AuthorizeRoles(EligereRoles.Admin, EligereRoles.ElectionOfficer, EligereRoles.PollingStationPresident, EligereRoles.PollingStationStaff)]
    public class PSCommissionController : Controller
    {
        private ESDB _context;
        private IConfiguration Configuration;

        public PSCommissionController(ESDB ctxt, IConfiguration configuration)
        {
            _context = ctxt;
            Configuration = configuration;
        }


        private async Task<(Person, Dictionary<Guid, Election>, int, Dictionary<PollingStationCommission, List<PollingStationCommissioner>>)> GetElectionData(Person person)
        {
            var psc = from c in _context.PollingStationCommission
                      join cc in _context.PollingStationCommissioner on c.Id equals cc.PollingStationCommissionFk
                      join el in _context.Election on c.ElectionFk equals el.Id
                      where cc.PersonFk == person.Id
                      select new { Election = el, Commission = c, Member = cc };

            var psd = new Dictionary<PollingStationCommission, List<PollingStationCommissioner>>();
            var e = new List<Election>();
            var ae = new List<Election>();
            foreach (var p in await psc.ToListAsync())
            {
                if (!psd.ContainsKey(p.Commission)) psd.Add(p.Commission, new List<PollingStationCommissioner>());
                psd[p.Commission].Add(p.Member);
                if (!e.Contains(p.Election)) e.Add(p.Election);
                if (p.Election.Active ?? false)
                {
                    if (!ae.Contains(p.Election)) ae.Add(p.Election);
                }
            }

            var elections = e.ToDictionary(e => e.Id);

            if (psd.Count > 1)
            {
                var pg = psd.Keys.GroupBy(psc => psc.PollingStationGroupId);
                if (pg.Count() > 1)
                    throw new Exception("Internal error: contact support - (Too many PollingStationGroupId for commissioner)");
                if (pg.First().Key == null)
                    throw new Exception("Internal error: contact support - (Many elections with empty PollingStationGroupId for commissioner)");
                foreach (var ps in psd.Keys)
                {
                    if (ps.PollingStationGroupId != elections[ps.ElectionFk].PollingStationGroupId)
                        throw new Exception("Internal error: contact support - (The election PollingStationGroupId does not match the commission one)");
                }
            }


            return (person, elections, ae.Count, psd);
        }

        public async Task<IActionResult> Index()
        {
            var pq = from p in _context.Person
                     join u in _context.UserLogin on p.Id equals u.PersonFk
                     where u.Provider == "AzureAD" && u.UserId == this.User.Identity.Name
                     select p;

            if (await pq.CountAsync() != 1)
                throw new Exception("Internal error! Too many persons associated with login " + this.User.Identity.Name);
            
            var person = await pq.FirstAsync();

            return View( await GetElectionData(person) );
        }

        [HttpGet("Recognize/{id}")]
        public async Task<IActionResult> Recognize(string id)
        {
            var pq = from p in _context.Person
                     join u in _context.UserLogin on p.Id equals u.PersonFk
                     where u.Provider == "AzureAD" && u.UserId == this.User.Identity.Name
                     select p;

            if (await pq.CountAsync() != 1)
                throw new Exception("Internal error! Too many persons associated with login " + this.User.Identity.Name);

            var person = await pq.FirstAsync();

            var voter = await _context.Person.FirstOrDefaultAsync(p => p.PublicId == id);
            if (voter == null) return NotFound("PublicId not found");

            var (per, elections, ae, psd) = await GetElectionData(person);

            var electionKeys = elections.Keys.ToList();

            var availableVotes = await (from v in _context.Voter
                                 where electionKeys.Contains(v.ElectionFk) && v.PersonFk == voter.Id
                                 select v).Include(v => v.RecognitionFkNavigation).ToListAsync();

            // ToDO: verificare che tutti i voti delle elezioni attive



            return View((voter, elections, ae, availableVotes));
        }

        private string GenerateOTP()
        {
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var rnd = new Random();
            var ret = new System.Text.StringBuilder();
            for (var i = 0; i < 3; i++)
            {
                ret.Append(alphabet[rnd.Next(alphabet.Length)]);
            }
            return ret.ToString();
        }

        [HttpPost("Recognize/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recognize(string id, string mobile, string rectype, string idnum, DateTime? idexp)
        {
            var pq = from p in _context.Person
                     join u in _context.UserLogin on p.Id equals u.PersonFk
                     where u.Provider == "AzureAD" && u.UserId == this.User.Identity.Name
                     select p;

            if (await pq.CountAsync() != 1)
                throw new Exception("Internal error! Too many persons associated with login " + this.User.Identity.Name);

            var person = await pq.FirstAsync();

            var voter = await _context.Person.FirstOrDefaultAsync(p => p.PublicId == id);
            if (voter == null) return NotFound("PublicId not found");

            var (per, elections, ae, psd) = await GetElectionData(person);

            var electionKeys = elections.Keys.ToList();

            var availableVotes = await (from v in _context.Voter
                                        where electionKeys.Contains(v.ElectionFk) && v.PersonFk == voter.Id
                                        select v).Include(v => v.RecognitionFkNavigation).ToListAsync();

            
            // To avoid international prefix check
            if (voter.GetPersonAttributes().Mobile != mobile)
            {
                var a = voter.GetPersonAttributes();
                a.Mobile = mobile.Length < 5 ? null : mobile.Replace(" ", "").Replace("-", "");
                voter.SetPersonAttributes(a);
            }

            var otp = GenerateOTP();

            foreach (var v in availableVotes)
            {
                var recognition = v.RecognitionFkNavigation;
                if (recognition == null)
                {
                    recognition = new Recognition()
                    {
                        Id = Guid.NewGuid(),
                    };
                    v.RecognitionFk = recognition.Id;
                    _context.Recognition.Add(recognition);
                }

                recognition.Idtype = rectype;
                recognition.UserId = User.Identity.Name;
                recognition.AccountProvider = "AzureAD";
                recognition.Otp = otp;
                recognition.State = 0;
                recognition.Validity = DateTime.Now + TimeSpan.FromMinutes(30);

                if (rectype != "KnownPerson")
                {
                    recognition.Idnum = idnum;
                    recognition.Idexpiration = idexp;
                }
            }

            await _context.SaveChangesAsync();

            if (mobile.Length > 5)
            {
                var m = HttpUtility.UrlEncode(mobile);
                var endpoint = Configuration.GetValue<string>("SendSMSEndPoint");
                var req = WebRequest.Create(endpoint + "?msg=Codice%20di%20voto%20di%20Eligere%20-%20" + otp + "&num=" + m);
                var resp = req.GetResponse();
            }

            return RedirectToAction("Recognize", new { id = id });
        }

        [AuthorizeRoles(EligereRoles.PollingStationStaff)]
        [HttpGet("Counters")]
        public async Task<IActionResult> Counters()
        {
            var electionsq = from e in _context.Election
                            where (e.Active ?? false)
                            join v in _context.Voter on e.Id equals v.ElectionFk
                            where v.Vote.HasValue
                            select new { ElectionID = e.Id, ElectionName = e.Name,  Vote = v.Vote };

            var counters = new Dictionary<Guid, int>();
            foreach (var v in await electionsq.ToListAsync())
            {
                if (!counters.ContainsKey(v.ElectionID))
                    counters.Add(v.ElectionID, 0);
                counters[v.ElectionID] = counters[v.ElectionID] + 1;
            }

            var names = (await _context.Election.Where(e => (e.Active ?? false)).ToListAsync());
            var elnames = new Dictionary<Guid, string>();
            foreach (var e in names) { elnames.Add(e.Id, e.Name); }

            return View((counters, elnames));
        }
    }
}
