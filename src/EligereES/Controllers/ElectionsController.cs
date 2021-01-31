using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EligereES.Models;
using EligereES.Models.DB;
using EligereES.Models.Client;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using EligereES.Models.Extensions;
using Microsoft.AspNetCore.Authorization;
using SQLitePCL;

namespace EligereES.Controllers
{
    [AuthorizeRoles(EligereRoles.Admin, EligereRoles.ElectionOfficer)]
    public class ElectionsController : Controller
    {
        private readonly ESDB _context;

        public ElectionsController(ESDB context)
        {
            _context = context;
        }

        private async Task<Dictionary<K,V>> ToDict<G,K, V>(IQueryable<G> q, Func<G,K> keysel, Func<G,V> valsel)
        {
            var ret = new Dictionary<K, V>();
            foreach (var g in (await q.ToListAsync()))
            {
                ret[keysel(g)] = valsel(g);
            }
            return ret;
        }

        public async Task<IActionResult> Index()
        {
            var commissions =
                await ToDict(from c in _context.PollingStationCommission
                group c by c.ElectionFk into cg
                select new { ElectionFk = cg.Key, Count = cg.Count() }, g => g.ElectionFk, g => g.Count);
            var voters =
                await ToDict(from v in _context.Voter
                group v by v.ElectionFk into vg
                select new { ElectionFk = vg.Key, Count = vg.Count() }, g => g.ElectionFk, g => g.Count);
            var ballots =
                await ToDict(from b in _context.BallotName
                group b by b.ElectionFk into bg
                select new { ElectionFk = bg.Key, Count = bg.Count() }, g => g.ElectionFk, g => g.Count);
            var elections = await _context.Election.ToListAsync();
            

            return View((elections, commissions, voters, ballots));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var election = await _context.Election
                .Include(e => e.ElectionTypeFkNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (election == null)
            {
                return NotFound();
            }

            return View(election);
        }

        public IActionResult Create()
        {
            ViewData["ElectionTypeFk"] = new SelectList(_context.ElectionType, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,PollStartDate,PollEndDate,ElectorateListClosingDate,ElectionTypeFk")] Election election)
        {
            if (ModelState.IsValid)
            {
                election.Id = Guid.NewGuid();
                election.Configuration = _context.ElectionType.Single(q => q.Id == election.ElectionTypeFk).DefaultConfiguration;
                _context.Add(election);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ElectionTypeFk"] = new SelectList(_context.ElectionType, "Id", "Name", election.ElectionTypeFk);
            return View(election);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var election = await _context.Election.FindAsync(id);
            if (election == null)
            {
                return NotFound();
            }
            ViewData["ElectionType"] = await _context.ElectionType.FindAsync(election.ElectionTypeFk);
            return View(election);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,PollStartDate,PollEndDate,ElectorateListClosingDate")] Election election)
        {
            if (id != election.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var e = await _context.Election.FindAsync(election.Id);
                    e.Name = election.Name;
                    e.Description = election.Description;
                    e.PollStartDate = election.PollStartDate;
                    e.PollEndDate = election.PollEndDate;
                    e.ElectorateListClosingDate = election.ElectorateListClosingDate;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElectionExists(election.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ElectionType"] = await _context.ElectionType.FindAsync(election.ElectionTypeFk);
            return View(election);
        }

        [HttpGet]
        public async Task<IActionResult> EditPollingStations(Guid id)
        {
            var pollingStation = await _context.PollingStationCommission.Where(c => c.ElectionFk == id).Include("PollingStationCommissioner.PersonFkNavigation").Include("RelPollingStationSystemPollingStationCommission").ToListAsync();
            ViewData["ElectionName"] = (await _context.Election.FindAsync(id)).Name;
            ViewData["ElectionId"] = id;
            return View(pollingStation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPSCommission(PollingStationCommissionUI comm, Guid? presidentFK)
        {
            var c = new PollingStationCommission();
            c.Id = Guid.NewGuid();
            comm.UpdatePollingStationCommission(c);
            _context.PollingStationCommission.Add(c);

            await _context.SaveChangesAsync();

            if (presidentFK.HasValue) { 
                var memb = new PollingStationCommissioner();
                memb.Id = Guid.NewGuid();
                memb.PersonFk = presidentFK.Value;
                memb.PollingStationCommissionFk = c.Id;

                c.PresidentFk = memb.Id;
                _context.PollingStationCommissioner.Add(memb);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("EditPollingStations", new { id = comm.ElectionFk });
        }

        [Authorize(Roles = EligereRoles.Admin)]
        [Authorize(Roles = EligereRoles.ElectionOfficer)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPSCommissioner(Guid electionFK, Guid commissionFK, Guid memberFK)
        {
            if (!(from pc in _context.PollingStationCommissioner where commissionFK == pc.PollingStationCommissionFk && memberFK == pc.PersonFk select pc).Any())
            {
                var c = new PollingStationCommissioner();
                c.Id = Guid.NewGuid();
                c.PollingStationCommissionFk = commissionFK;
                c.PersonFk = memberFK;

                _context.PollingStationCommissioner.Add(c);

                await _context.SaveChangesAsync();

            }

            return RedirectToAction("EditPollingStations", new { id = electionFK });
        }

        [HttpGet]
        public async Task<IActionResult> RemovePSCommissioner(Guid commissionerId)
        {
            var c = await _context.PollingStationCommissioner.FindAsync(commissionerId);
            var pc = await _context.PollingStationCommission.FindAsync(c.PollingStationCommissionFk);

            if (pc.PresidentFk == c.Id)
            {
                pc.PresidentFk = null;
            }
            _context.PollingStationCommissioner.Remove(c);

            await _context.SaveChangesAsync();

            return RedirectToAction("EditPollingStations", new { id = pc.ElectionFk });
        }

        [HttpGet("Voters/{id}")]
        public async Task<IActionResult> Voters(Guid id, string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["electionId"] = id;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["LastNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "lastName_desc" : "";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var people = from p in _context.Person join v in _context.Voter on p.Id equals v.PersonFk where v.ElectionFk == id select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                people = people.Where(p => p.LastName.Contains(searchString) || p.FirstName.Contains(searchString) || p.PublicId.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "lastName_desc":
                    people = people.OrderByDescending(p => p.LastName);
                    break;
                default:
                    people = people.OrderBy(p => p.LastName);
                    break;
            }
            int pageSize = 25;
            return View(await PaginatedList<Models.DB.Person>.CreateAsync(people.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpPost("VotersBulkUpload/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VotersBulkUpload(Guid id, List<IFormFile> file)
        {
            if (file.Count != 1)
            {
                return NotFound(); // FixMe should not be not found
            }
            var result = new List<(Person person, (string firstName, string lastName, string publicId, DateTime birthDate, string birthPlace, string role)? data, string reason)>();
            using (var csv = new CsvReader(new System.IO.StreamReader(file[0].OpenReadStream()), CultureInfo.CurrentUICulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (await csv.ReadAsync())
                {
                    var firstName = csv.GetField<string>(0).Trim(' ', '\t');
                    var lastName = csv.GetField<string>(1).Trim(' ', '\t');
                    var companyId = csv.GetField<string>(2).Trim(' ', '\t');
                    var publicId = csv.GetField<string>(3).Trim(' ', '\t').ToUpperInvariant();
                    var birthPlace = csv.GetField<string>(4).Trim(' ', '\t');
                    var birthDate = csv.GetField<DateTime>(5);
                    var role = csv.GetField<string>(6).Trim(' ', '\t');

                    var q = from p in _context.Person join v in _context.Voter on p.Id equals v.PersonFk where v.ElectionFk == id select p;
                    // For the moment is a weak check before insert
                    if (await q.CountAsync(p => p.PublicId == publicId) > 0)
                    {
                        result.Add(
                            (await _context.Person.FirstAsync(p => p.PublicId == publicId),
                            (firstName, lastName, publicId, birthDate, birthPlace, role),
                            "Person already present"));
                        // Add secure log entry here!
                    }
                    else
                    {
                        var p = await _context.Person.FirstOrDefaultAsync(per => per.PublicId == publicId);
                        if (p == null)
                        {
                            result.Add(
                                (await _context.Person.FirstAsync(p => p.PublicId == publicId),
                                (firstName, lastName, publicId, birthDate, birthPlace, role),
                                "Person missing from database"));
                            // Add secure log entry here!
                        }
                        else
                        {
                            var toadd = new Voter()
                            {
                                Id = Guid.NewGuid(),
                                ElectionFk = id,
                                PersonFk = p.Id
                            };
                            _context.Voter.Add(toadd);
                            result.Add((p, null, null));
                            // Add secure log entry here!
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return View(result);
        }


        [HttpGet("Candidates/{id}")]
        public async Task<IActionResult> Candidates(Guid id, string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["electionId"] = id;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["LastNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "lastName_desc" : "";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var people = from p in _context.Person 
                         join c in _context.EligibleCandidate on p.Id equals c.PersonFk 
                         join bn in _context.BallotName on c.BallotNameFk equals bn.Id 
                         where bn.ElectionFk == id select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                people = people.Where(p => p.LastName.Contains(searchString) || p.FirstName.Contains(searchString) || p.PublicId.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "lastName_desc":
                    people = people.OrderByDescending(p => p.LastName);
                    break;
                default:
                    people = people.OrderBy(p => p.LastName);
                    break;
            }
            int pageSize = 25;
            return View(await PaginatedList<Models.DB.Person>.CreateAsync(people.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpPost("CandidatesBulkUpload/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CandidatesBulkUpload(Guid id, List<IFormFile> file)
        {
            if (file.Count != 1)
            {
                return NotFound(); // FixMe should not be not found
            }
            var result = new List<(Person person, (string firstName, string lastName, string publicId)? data, string reason)>();
            using (var csv = new CsvReader(new System.IO.StreamReader(file[0].OpenReadStream()), CultureInfo.CurrentUICulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (await csv.ReadAsync())
                {
                    var firstName = csv.GetField<string>(0);
                    var lastName = csv.GetField<string>(1);
                    var companyId = csv.GetField<string>(2);
                    var publicId = csv.GetField<string>(3);

                    var q = from p in _context.Person 
                            join c in _context.EligibleCandidate on p.Id equals c.PersonFk 
                            join bn in _context.BallotName on c.BallotNameFk equals bn.Id
                            where bn.ElectionFk == id select p;
                    // For the moment is a weak check before insert
                    if (await q.CountAsync(p => p.PublicId == publicId) > 0)
                    {
                        result.Add(
                            (await _context.Person.FirstAsync(p => p.PublicId == publicId),
                            (firstName, lastName, publicId),
                            "Person already present"));
                        // Add secure log entry here!
                    }
                    else
                    {
                        var p = await _context.Person.FirstOrDefaultAsync(per => per.PublicId == publicId);
                        if (p == null)
                        {
                            result.Add(
                                (await _context.Person.FirstAsync(p => p.PublicId == publicId),
                                (firstName, lastName, publicId),
                                "Person missing from database"));
                            // Add secure log entry here!
                        }
                        else
                        {
                            var ballotname = new BallotName()
                            {
                                Id = Guid.NewGuid(),
                                BallotNameLabel = $"{p.LastName} {p.FirstName}",
                                ElectionFk = id
                            };
                            var toadd = new EligibleCandidate()
                            {
                                Id = Guid.NewGuid(),
                                BallotNameFk = ballotname.Id,
                                PersonFk = p.Id
                            };
                            _context.BallotName.Add(ballotname);
                            _context.EligibleCandidate.Add(toadd);
                            result.Add((p, null, null));
                            // Add secure log entry here!
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return View(result);
        }

        [HttpGet("RemoveCandidate")]
        public async Task<IActionResult> RemoveCandidate(Guid electionid, Guid personid)
        {
            var cand = await (from c in _context.EligibleCandidate
                              join bn in _context.BallotName on c.BallotNameFk equals bn.Id
                              where bn.ElectionFk == electionid && c.PersonFk == personid
                              select new { Candidate = c, Ballot = bn }).FirstOrDefaultAsync();

            if (cand == null) throw new Exception("Candidate not found");

            _context.EligibleCandidate.Remove(cand.Candidate);
            _context.BallotName.Remove(cand.Ballot);
            await _context.SaveChangesAsync();
            return RedirectToAction("Candidates", new { id = electionid });
        }

        [HttpGet("RemoveVoter")]
        public async Task<IActionResult> RemoveVoter(Guid electionid, Guid personid)
        {
            var v = await (from c in _context.Voter
                              where c.ElectionFk == electionid && c.PersonFk == personid
                              select c).FirstOrDefaultAsync();

            if (v == null) throw new Exception("Voter not found");

            _context.Voter.Remove(v);
            await _context.SaveChangesAsync();
            return RedirectToAction("Voters", new { id = electionid });
        }

        [HttpGet("ClonePSCommissions")]
        public async Task<IActionResult> ClonePSCommissions(Guid id)
        {
            var el = (await GetUpcomingElections()).Where(e => e.Id != id).OrderBy(e => e.Name);
            var del = await _context.Election.FindAsync(id);
            var list = new SelectList(el, "Id", "Name");
            ViewData["dst"] = del;
            return View(list);
        }

        [HttpPost("ClonePSCommissions")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClonePSCommissions(Guid src, Guid dst)
        {
            var srce = await _context.Election.FindAsync(src);
            var dste = await _context.Election.FindAsync(dst);

            var updatedgid = false;

            var presidents = new Dictionary<PollingStationCommission, PollingStationCommissioner>();

            if (srce.PollStartDate >= DateTime.Today)
            {
                if (srce.PollingStationGroupId.HasValue)
                {
                    dste.PollingStationGroupId = srce.PollingStationGroupId;
                }
                else
                {
                    var newgid = Guid.NewGuid();
                    srce.PollingStationGroupId = newgid;
                    dste.PollingStationGroupId = newgid;
                    updatedgid = true;
                }
            }

            var commissions = await (from c in _context.PollingStationCommission where c.ElectionFk == src select c).ToListAsync();

            foreach (var com in commissions)
            {
                if (updatedgid) 
                    com.PollingStationGroupId = srce.PollingStationGroupId;

                var ncom = new PollingStationCommission()
                {
                    Id = Guid.NewGuid(),
                    Location = com.Location,
                    DigitalLocation = com.DigitalLocation,
                    Description = com.Description,
                    ElectionFk = dste.Id,
                    PollingStationGroupId = com.PollingStationGroupId
                };

                _context.PollingStationCommission.Add(ncom);

                var members = await (from m in _context.PollingStationCommissioner where m.PollingStationCommissionFk == com.Id select m).ToListAsync();

                foreach (var mem in members)
                {
                    var nmem = new PollingStationCommissioner()
                    {
                        Id = Guid.NewGuid(),
                        PersonFk = mem.PersonFk,
                        AvailableForRemoteRecognition = mem.AvailableForRemoteRecognition,
                        PollingStationCommissionFk = ncom.Id,
                        VirtualRoom = mem.VirtualRoom
                    };
                    if (com.PresidentFk.HasValue && com.PresidentFk == mem.Id)
                        presidents.Add(ncom, nmem);
                    _context.PollingStationCommissioner.Add(nmem);
                }

                var remids = await (from m in _context.RemoteIdentificationCommissioner where m.PollingStationCommissionFk == com.Id select m).ToListAsync();

                foreach (var remid in remids)
                {
                    var nremid = new RemoteIdentificationCommissioner()
                    {
                        Id = Guid.NewGuid(),
                        PersonFk = remid.PersonFk,
                        AvailableForRemoteRecognition = remid.AvailableForRemoteRecognition,
                        PollingStationCommissionFk = ncom.Id,
                        VirtualRoom = remid.VirtualRoom
                    };
                    _context.RemoteIdentificationCommissioner.Add(nremid);
                }
            }

            await _context.SaveChangesAsync();

            // Needed to avoid circular dependencies
            foreach (var c in presidents.Keys)
            {
                c.PresidentFk = presidents[c].Id;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("EditPollingStations", new { id = dste.Id });
        }

        [AuthorizeRoles(EligereRoles.ElectionOfficer)]
        [HttpGet("Counters")]
        public async Task<IActionResult> Counters()
        {
            var electionsq = from e in _context.Election
                             where (e.Active)
                             join v in _context.Voter on e.Id equals v.ElectionFk
                             //group new { Election = e, Vote(e, v) } by v.Id into g 
                             where v.Vote.HasValue
                             select new { ElectionID = e.Id, ElectionName = e.Name, ElectionConfiguration=e.Configuration, Vote = v.Vote };

            var counters = new Dictionary<Guid, int>();
            foreach (var v in await electionsq.ToListAsync())
            {
                if (!counters.ContainsKey(v.ElectionID))
                    counters.Add(v.ElectionID, 0);
                counters[v.ElectionID] = counters[v.ElectionID] + 1;
            }

            var names = (await _context.Election.Where(e => e.Active).ToListAsync());
            var elnames = new Dictionary<Guid, string>();
            foreach (var e in names) { elnames.Add(e.Id, e.Name); }

            return View((counters, elnames));
        }

        private async Task<List<Election>> GetUpcomingElections()
        {
            var q = from e in _context.Election
                    where e.PollStartDate >= DateTime.Today
                    select e;

            return await q.ToListAsync();
        }

        private bool ElectionExists(Guid id)
        {
            return _context.Election.Any(e => e.Id == id);
        }
    }
}
