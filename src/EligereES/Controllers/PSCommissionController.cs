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
    public class PSCommissionController : Controller
    {
        private ESDB _context;
        private IConfiguration Configuration;
        private PersistentCommissionManager _manager;
        private string defaultProvider;

        public PSCommissionController(ESDB ctxt, PersistentCommissionManager manager, IConfiguration configuration)
        {
            _context = ctxt;
            Configuration = configuration;
            _manager = manager;
            defaultProvider = configuration.GetValue(typeof(string), "DefaultAuthProvider") as string;
        }


        private async Task<(Person, Dictionary<Guid, Election>, int, Dictionary<PollingStationCommission, List<PollingStationCommissioner>>)> GetElectionData(Person person)
        {
            var psc = from c in _context.PollingStationCommission
                      join cc in _context.PollingStationCommissioner on c.Id equals cc.PollingStationCommissionFk
                      join el in _context.Election on c.ElectionFk equals el.Id
                      where cc.PersonFk == person.Id && el.Active
                      select new { Election = el, Commission = c, Member = cc };

            var psd = new Dictionary<PollingStationCommission, List<PollingStationCommissioner>>();
            var e = new List<Election>();
            var ae = new List<Election>();
            foreach (var p in await psc.ToListAsync())
            {
                if (!psd.ContainsKey(p.Commission)) psd.Add(p.Commission, new List<PollingStationCommissioner>());
                psd[p.Commission].Add(p.Member);
                if (!e.Contains(p.Election)) e.Add(p.Election);
                if (p.Election.Active)
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

        private async Task<(Person, Dictionary<Guid, Election>, int, Dictionary<PollingStationCommission, List<RemoteIdentificationCommissioner>>)> GetElectionDataForRemoteIdentificationOfficer(Person person)
        {
            var psc = from c in _context.PollingStationCommission
                      join cc in _context.RemoteIdentificationCommissioner on c.Id equals cc.PollingStationCommissionFk
                      join el in _context.Election on c.ElectionFk equals el.Id
                      where cc.PersonFk == person.Id
                      select new { Election = el, Commission = c, Member = cc };

            var psd = new Dictionary<PollingStationCommission, List<RemoteIdentificationCommissioner>>();
            var e = new List<Election>();
            var ae = new List<Election>();
            foreach (var p in await psc.ToListAsync())
            {
                if (!psd.ContainsKey(p.Commission)) psd.Add(p.Commission, new List<RemoteIdentificationCommissioner>());
                psd[p.Commission].Add(p.Member);
                if (!e.Contains(p.Election)) e.Add(p.Election);
                if (p.Election.Active)
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

        [AuthorizeRoles(EligereRoles.Admin, EligereRoles.ElectionOfficer, EligereRoles.PollingStationPresident, EligereRoles.PollingStationStaff)]
        public async Task<IActionResult> Index()
        {
            var pq = from p in _context.Person
                     join u in _context.UserLogin on p.Id equals u.PersonFk
                     where u.Provider == defaultProvider && u.UserId == this.User.Identity.Name
                     select p;

            if (await pq.CountAsync() != 1)
                throw new Exception("Internal error! Too many persons associated with login " + this.User.Identity.Name);
            
            var person = await pq.FirstAsync();
            var now = DateTime.Now + TimeSpan.FromMinutes(15);
            var today = DateTime.Today;
            var available = await (from pc in _context.PollingStationCommissioner
                                   join c in _context.PollingStationCommission on pc.PollingStationCommissionFk equals c.Id
                                   join e in _context.Election on c.ElectionFk equals e.Id
                                   where e.PollStartDate <= now && e.PollEndDate > today && pc.PersonFk == person.Id && pc.AvailableForRemoteRecognition
                                   select pc).AnyAsync();

            ViewData["available"] = available;

            return View( await GetElectionData(person) );
        }
        
        [HttpGet("RemoteIdentification")]
        [AuthorizeRoles(EligereRoles.RemoteIdentificationOfficer)]
        public async Task<IActionResult> RemoteIdentification()
        {
            var pq = from p in _context.Person
                     join u in _context.UserLogin on p.Id equals u.PersonFk
                     where u.Provider == defaultProvider && u.UserId == this.User.Identity.Name
                     select p;

            if (await pq.CountAsync() != 1)
                throw new Exception("Internal error! Too many persons associated with login " + this.User.Identity.Name);

            var person = await pq.FirstAsync();
            var now = DateTime.Now + TimeSpan.FromMinutes(15);
            var today = DateTime.Today;
            var available = await (from pc in _context.RemoteIdentificationCommissioner
                                   join c in _context.PollingStationCommission on pc.PollingStationCommissionFk equals c.Id
                                   join e in _context.Election on c.ElectionFk equals e.Id
                                   where e.PollStartDate <= now && e.PollEndDate > today && pc.PersonFk == person.Id && pc.AvailableForRemoteRecognition
                                   select pc).AnyAsync();

            ViewData["available"] = available;

            return View(await GetElectionDataForRemoteIdentificationOfficer(person));
        }

        [HttpGet("Identify/{id}")]
        [AuthorizeRoles(EligereRoles.Admin, EligereRoles.ElectionOfficer, EligereRoles.PollingStationPresident, EligereRoles.PollingStationStaff, EligereRoles.RemoteIdentificationOfficer)]
        public async Task<IActionResult> Identify(string id, string otpresult)
        {
            var pq = from p in _context.Person
                     join u in _context.UserLogin on p.Id equals u.PersonFk
                     where u.Provider == defaultProvider && u.UserId == this.User.Identity.Name
                     select p;

            if (await pq.CountAsync() != 1)
                throw new Exception("Internal error! Too many persons associated with login " + this.User.Identity.Name);

            var person = await pq.FirstAsync();

            var voter = await _context.Person.FirstOrDefaultAsync(p => p.PublicId == id);
            if (voter == null) return NotFound("PublicId not found");

            List<Guid> electionKeys = null;
            Dictionary<Guid, Election> elections = null;
            int ae = 0;
            if (this.User.IsInRole(EligereRoles.RemoteIdentificationOfficer))
            {
                var (per, relections, rae, psd) = await GetElectionDataForRemoteIdentificationOfficer(person);
                electionKeys = relections.Keys.ToList();
                elections = relections;
                ae = rae;
            }
            else
            {
                var (per, celections, cae, psd) = await GetElectionData(person);
                electionKeys = celections.Keys.ToList();
                elections = celections;
                ae = cae;
            }

            var availableVotes = await (from v in _context.Voter
                                 where electionKeys.Contains(v.ElectionFk) && v.PersonFk == voter.Id
                                 select v).Include(v => v.RecognitionFkNavigation).ToListAsync();

            // ToDO: verificare che tutti i voti delle elezioni attive

            if (otpresult != null)
            {
                ViewData["OTPResult"] = otpresult;
            }


            return View((voter, elections, ae, availableVotes));
        }

        [HttpPost("Identify/Public/")]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(EligereRoles.Admin, EligereRoles.ElectionOfficer, EligereRoles.PollingStationPresident)]
        public async Task<IActionResult> PublicIdentificationElection(string remote)
        {
            var pq = from p in _context.Person
                     join u in _context.UserLogin on p.Id equals u.PersonFk
                     where u.Provider == defaultProvider && u.UserId == this.User.Identity.Name
                     select p;

            if (await pq.CountAsync() != 1)
                throw new Exception("Internal error! Too many persons associated with login " + this.User.Identity.Name);

            var person = await pq.FirstAsync();

            var (per, elections, count, commissions) = await GetElectionData(person);

            if (!elections.Where(e => e.Value.ElectionConfiguration.IdentificationType == Models.Extensions.IdentificationType.Public).Any())
                return BadRequest("No election in the group is not configured to allow public identification");

            var pubElections = new List<Guid>();

            pubElections.AddRange(
                elections
                .Where(el => el.Value.ElectionConfiguration.IdentificationType == Models.Extensions.IdentificationType.Public)
                .Select(el => el.Key).ToList()
            );

            var otp = OTPSender.GenerateOTP();

            var voters = await _context.Voter.Where(v => pubElections.Contains(v.ElectionFk)).Include(v => v.RecognitionFkNavigation).ToListAsync();

            foreach (var v in voters)
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

                recognition.Idtype = "_PublicIdentification";
                recognition.UserId = User.Identity.Name;
                recognition.AccountProvider = defaultProvider;
                recognition.Otp = otp;
                recognition.State = 0;
                recognition.Validity = DateTime.Now + TimeSpan.FromMinutes(30);
                if (User.IsInRole(EligereRoles.RemoteIdentificationOfficer))
                {
                    recognition.RemoteIdentification = true;
                }
                else
                {
                    recognition.RemoteIdentification = remote == "off";
                }
            }

            await _context.SaveChangesAsync();


            var pubElList = elections.Where(el => pubElections.Contains(el.Key)).Select(el => el.Value).ToList();
            return View((pubElList, otp));
        }


        [HttpPost("Identify/{id}")]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(EligereRoles.Admin, EligereRoles.ElectionOfficer, EligereRoles.PollingStationPresident, EligereRoles.PollingStationStaff, EligereRoles.RemoteIdentificationOfficer)]
        public async Task<IActionResult> Identify(string id, string mobile, string remote, string rectype, string idnum, DateTime? idexp)
        {
            var pq = from p in _context.Person
                     join u in _context.UserLogin on p.Id equals u.PersonFk
                     where u.Provider == defaultProvider && u.UserId == this.User.Identity.Name
                     select p;

            if (await pq.CountAsync() != 1)
                throw new Exception("Internal error! Too many persons associated with login " + this.User.Identity.Name);

            var person = await pq.FirstAsync();

            var voter = await _context.Person.FirstOrDefaultAsync(p => p.PublicId == id);
            if (voter == null) return NotFound("PublicId not found");

            var voteruseridq = from u in _context.UserLogin
                              where u.PersonFk == voter.Id
                              select u.UserId;

            var voteruid = await voteruseridq.FirstAsync();

            List < Guid > electionKeys = null;
            Dictionary<Guid, Election> elections = null;
            int ae = 0;
            if (this.User.IsInRole(EligereRoles.RemoteIdentificationOfficer))
            {
                var (per, relections, rae, psd) = await GetElectionDataForRemoteIdentificationOfficer(person);
                electionKeys = relections.Keys.ToList();
                elections = relections;
                ae = rae;
            }
            else
            {
                var (per, celections, cae, psd) = await GetElectionData(person);
                electionKeys = celections.Keys.ToList();
                elections = celections;
                ae = cae;
            }

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

            var otp = OTPSender.GenerateOTP();

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
                recognition.AccountProvider = defaultProvider;
                recognition.Otp = otp;
                recognition.State = 0;
                recognition.Validity = DateTime.Now + TimeSpan.FromMinutes(30);
                if (User.IsInRole(EligereRoles.RemoteIdentificationOfficer))
                {
                    recognition.RemoteIdentification = true;
                } else
                {
                    recognition.RemoteIdentification = remote == "on";
                }

                if (rectype != "KnownPerson")
                {
                    recognition.Idnum = idnum;
                    recognition.Idexpiration = idexp;
                }
            }

            await _context.SaveChangesAsync();

            if (mobile.Length > 5)
            {
                if (mobile.StartsWith("\\u002B"))
                {
                    mobile = mobile.Replace("\\u002B", "+");
                }
                OTPSender.SendSMS(Configuration, otp, mobile);
            }

            string otpresult = null;
            try
            {
                OTPSender.SendMail(Configuration, otp, voteruid);
            } catch
            {
                otpresult = $"Error sending to {voteruid}";
            }

            return RedirectToAction("Identify", new { id = id, otpresult = otpresult });
        }

        [AuthorizeRoles(EligereRoles.PollingStationStaff)]
        [HttpGet("Counters")]
        public async Task<IActionResult> Counters()
        {
            var activeelections = from e in _context.Election where (e.Configuring || e.Active) select e;
            var electionsq = from e in activeelections
                            join v in _context.Voter on e.Id equals v.ElectionFk
                            where v.Vote.HasValue
                            orderby e.Name
                            select new { ElectionID = e.Id, ElectionName = e.Name,  Vote = v.Vote };

            var counters = new Dictionary<Guid, int>();
            foreach (var v in await activeelections.ToListAsync())
            {
                if (!counters.ContainsKey(v.Id))
                    counters.Add(v.Id, 0);
            }

            foreach (var v in await electionsq.ToListAsync())
            {
                counters[v.ElectionID] = counters[v.ElectionID] + 1;
            }

            var names = (await _context.Election.Where(e => (e.Configuring || e.Active)).ToListAsync());
            var elnames = new Dictionary<Guid, string>();
            foreach (var e in names) { elnames.Add(e.Id, e.Name); }

            return View((counters, elnames));
        }

        [AuthorizeRoles(EligereRoles.PollingStationPresident)]
        [HttpGet("ElectionControl")]
        public async Task<IActionResult> ElectionControl()
        {
            var pq = from p in _context.Person
                     join u in _context.UserLogin on p.Id equals u.PersonFk
                     where u.Provider == defaultProvider && u.UserId == this.User.Identity.Name
                     select p;

            if (await pq.CountAsync() != 1)
                throw new Exception("Internal error! Too many persons associated with login " + this.User.Identity.Name);

            var person = await pq.FirstAsync();

            var commissions = from c in _context.PollingStationCommission
                              join pc in _context.PollingStationCommissioner on c.Id equals pc.PollingStationCommissionFk
                              where c.PresidentFk == pc.Id && pc.PersonFk == person.Id
                              select c.ElectionFk;

            // FIXME: if president does not close after end of poll the UI may become inconsistent: to be fixed
            var now = DateTime.Now + TimeSpan.FromMinutes(90);
            var today = DateTime.Today;
            var elections = from e in _context.Election
                            where commissions.Contains(e.Id) && (e.PollStartDate <= now) && e.PollEndDate > today
                            select e;

            var elist = await elections.ToListAsync();
            if (elist.Count > 1 && elist.GroupBy(v => v.PollingStationGroupId).Count() > 1)
                throw new Exception("Invalid PollingStationGroupId count");

            if (elist.Count >1)
            {
                var gid = elist[0].PollingStationGroupId;
                elist = await (from e in _context.Election where e.PollingStationGroupId == gid select e).ToListAsync();
            }
               

            return View((person, elist));
        }

        [AuthorizeRoles(EligereRoles.PollingStationPresident)]
        [HttpGet("CommissionStatus")]
        public async Task<IActionResult> CommissionStatus()
        {
            var pq = from p in _context.Person
                     join u in _context.UserLogin on p.Id equals u.PersonFk
                     where u.Provider == defaultProvider && u.UserId == this.User.Identity.Name
                     select p;

            if (await pq.CountAsync() != 1)
                throw new Exception("Internal error! Too many persons associated with login " + this.User.Identity.Name);

            var person = await pq.FirstAsync();

            var commissions = from c in _context.PollingStationCommission
                              join pc in _context.PollingStationCommissioner on c.Id equals pc.PollingStationCommissionFk
                              where c.PresidentFk == pc.Id && pc.PersonFk == person.Id
                              select c.ElectionFk;

            // FIXME: if president does not close after end of poll the UI may become inconsistent: to be fixed
            var now = DateTime.Now + TimeSpan.FromMinutes(15);
            var today = DateTime.Today;
            var elections = from e in _context.Election
                            where commissions.Contains(e.Id) && (e.PollStartDate <= now) && e.PollEndDate > today
                            select e;

            var elist = await elections.ToListAsync();
            if (elist.Count > 1 && elist.GroupBy(v => v.PollingStationGroupId).Count() > 1)
                throw new Exception("Invalid PollingStationGroupId count");

            if (elist.Count > 1)
            {
                var gid = elist[0].PollingStationGroupId;
                elist = await (from e in _context.Election where e.Active && e.PollingStationGroupId == gid select e).ToListAsync();
            }

            var eids = elist.ConvertAll(e => e.Id);
            var commissioners = await (from c in _context.PollingStationCommission
                                       join pc in _context.PollingStationCommissioner on c.Id equals pc.PollingStationCommissionFk
                                       join p in _context.Person on pc.PersonFk equals p.Id
                                       where eids.Contains(c.ElectionFk)
                                       select new { PersonId=p.Id, FirstName = p.FirstName, LastName = p.LastName, Available = pc.AvailableForRemoteRecognition, VirtualRoom = pc.VirtualRoom }).Distinct().ToListAsync();
            _manager.CollectExpiredItems();
            var busycom = _manager.GetBusyCommissioners();
            var availableBusy = commissioners.Where(c => c.Available && busycom.Contains(c.PersonId)).OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList().ConvertAll(c => (c.FirstName, c.LastName, c.VirtualRoom));
            var available = commissioners.Where(c => c.Available).OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList().ConvertAll(c => (c.FirstName, c.LastName, c.VirtualRoom));
            var unavailable = commissioners.Where(c => !c.Available).OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList().ConvertAll(c => (c.FirstName, c.LastName, c.VirtualRoom));

            return View((person, elist, availableBusy, available, unavailable));
        }

        [AuthorizeRoles(EligereRoles.PollingStationPresident)]
        [HttpPost("ElectionControl")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ElectionControl(string confstate, string state)
        {
            var pq = from p in _context.Person
                     join u in _context.UserLogin on p.Id equals u.PersonFk
                     where u.Provider == defaultProvider && u.UserId == this.User.Identity.Name
                     select p;

            if (await pq.CountAsync() != 1)
                throw new Exception("Internal error! Too many persons associated with login " + this.User.Identity.Name);

            var person = await pq.FirstAsync();

            var commissions = from c in _context.PollingStationCommission
                              join pc in _context.PollingStationCommissioner on c.Id equals pc.PollingStationCommissionFk
                              where c.PresidentFk == pc.Id && pc.PersonFk == person.Id
                              select c.ElectionFk;

            // FIXME: if president does not close after end of poll the UI may become inconsistent: to be fixed
            var now = DateTime.Now + TimeSpan.FromMinutes(15);
            var today = DateTime.Today;
            var elections = from e in _context.Election
                            where commissions.Contains(e.Id) && (e.PollStartDate <= now) && e.PollEndDate > today
                            select e;

            var elist = await elections.ToListAsync();
            if (elist.Count > 1 && elist.GroupBy(v => v.PollingStationGroupId).Count() > 1)
                throw new Exception("Invalid PollingStationGroupId count");

            if (elist.Count > 1)
            {
                var gid = elist[0].PollingStationGroupId;
                elist = await (from e in _context.Election where e.PollingStationGroupId == gid select e).ToListAsync();
            }

            var config = confstate == "on";
            var active = state == "on";

            if (active) config = false;

            foreach (var e in elist)
            {
                if (e.PollStartDate <= now && e.PollEndDate > today)
                {
                    e.Configuring = config;
                    e.Active = active;
                }

                // Ensures that availability of commissioners is reset to unavailable
                var pscl = await (
                            from c in _context.PollingStationCommissioner
                            join com in _context.PollingStationCommission on c.PollingStationCommissionFk equals com.Id
                            join el in _context.Election on com.ElectionFk equals el.Id
                            where e.Id == el.Id
                            select c).ToListAsync();

                foreach (var c in pscl)
                {
                    c.AvailableForRemoteRecognition = false;
                }
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("ElectionControl");
        }
    }
}
