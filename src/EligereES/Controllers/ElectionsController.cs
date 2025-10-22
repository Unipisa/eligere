using CsvHelper;
using EligereES.Models;
using EligereES.Models.Client;
using EligereES.Models.DB;
using EligereES.Models.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        private async Task<Dictionary<K, V>> ToDict<G, K, V>(IQueryable<G> q, Func<G, K> keysel, Func<G, V> valsel)
        {
            var ret = new Dictionary<K, V>();
            foreach (var g in (await q.ToListAsync()))
            {
                ret[keysel(g)] = valsel(g);
            }
            return ret;
        }

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
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
                             where !b.IsCandidate.HasValue || b.IsCandidate == true
                             group b by b.ElectionFk into bg
                             select new { ElectionFk = bg.Key, Count = bg.Count() }, g => g.ElectionFk, g => g.Count);
            var ballotParties =
                await ToDict(from b in _context.BallotName
                             where b.IsCandidate == false
                             group b by b.ElectionFk into bg
                             select new { ElectionFk = bg.Key, Count = bg.Count() }, g => g.ElectionFk, g => g.Count);
            var elections = await _context.Election.ToListAsync();


            return View((elections, commissions, voters, ballots, ballotParties));
        }

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
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

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        public IActionResult Create()
        {
            ViewData["ElectionTypeFk"] = new SelectList(_context.ElectionType, "Id", "Name");
            return View();
        }

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,PollStartDate,PollEndDate,ElectorateListClosingDate,ElectionTypeFk")] Election election)
        {
            if (ModelState.IsValid)
            {
                election.Id = Guid.NewGuid();
                election.PollingStationGroupId = Guid.NewGuid();
                election.Configuration = _context.ElectionType.Single(q => q.Id == election.ElectionTypeFk).DefaultConfiguration;
                _context.Add(election);
                await _context.SaveChangesAsync();
                return RedirectToAction("Config", new { id = election.Id });
            }
            ViewData["ElectionTypeFk"] = new SelectList(_context.ElectionType, "Id", "Name", election.ElectionTypeFk);
            return View(election);
        }

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
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
            ViewBag.ElectionType = await _context.ElectionType.FindAsync(election.ElectionTypeFk);
            return View(election);
        }

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        public async Task<IActionResult> Delete(Guid? id)
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
            return View(election);
        }

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var election = await _context.Election.FindAsync(id);
            if (election == null)
            {
                return NotFound();
            }

            if ((from v in _context.Voter where v.ElectionFk == id && v.Vote.HasValue select v).Any())
            {
                return BadRequest("Election has casted votes");
            }

            var com = _context.PollingStationCommission.FirstOrDefault(c => c.ElectionFk == id);
            if (com != null)
            {
                com.PresidentFk = null;
                await _context.SaveChangesAsync();
                _context.PollingStationCommissioner.RemoveRange(from pc in _context.PollingStationCommissioner where pc.PollingStationCommissionFk == com.Id select pc);
                _context.RemoteIdentificationCommissioner.RemoveRange(from c in _context.RemoteIdentificationCommissioner where c.PollingStationCommissionFk == com.Id select c);
            }
            _context.Voter.RemoveRange(from v in _context.Voter where v.ElectionFk == id select v);
            _context.EligibleCandidate.RemoveRange(from c in _context.EligibleCandidate join bn in _context.BallotName on c.BallotNameFk equals bn.Id where bn.ElectionFk == id select c);
            _context.BallotName.RemoveRange(from bn in _context.BallotName where bn.ElectionFk == id select bn);
            _context.PollingStationCommission.RemoveRange(from c in _context.PollingStationCommission where c.ElectionFk == id select c);
            _context.Election.Remove(election);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
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
            ViewBag.ElectionType = await _context.ElectionType.FindAsync(election.ElectionTypeFk);
            return View(election);
        }

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        public async Task<IActionResult> Config(Guid? id)
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
            ElectionUI ui = new ElectionUI(election);
            return View(ui);
        }


        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Config(Guid? id, int NumPreferences, int NumPartyPreferences, int CandidatesType, int IdentificationType, double SamplingRate, int? NoNullVote, int? ActiveForStronglyAuthenticatedUsers)
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

            ElectionConfiguration conf = ElectionConfiguration.FromJson(election.Configuration);
            try
            {
                conf.NumPreferences = NumPreferences;
                conf.NumPartyPreferences = NumPartyPreferences;
                conf.CandidatesType = (CandidatesType)CandidatesType;
                conf.HasCandidates = (conf.CandidatesType != Models.Extensions.CandidatesType.Implicit);
                conf.IdentificationType = (IdentificationType)IdentificationType;
                conf.SamplingRate = SamplingRate;
                conf.NoNullVote = NoNullVote == 1;
                conf.ActiveForStronglyAuthenticatedUsers = ActiveForStronglyAuthenticatedUsers == 1;
            }
            catch (Exception)
            {
                return View("CustomError", ("Parametri di configurazione elezione", "Non è stato possibile codificare i parametri di configurazione, verificare il tipo di valori inseriti"));
            }

            election.Configuration = conf.ToJson();
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return View("CustomError", ("Salvataggio configurazione elezione", "Non è stato possibile salvare i parametri di configurazione"));
            }

            return RedirectToAction(nameof(Index));
        }



        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> EditPollingStations(Guid id)
        {
            var pollingStation = await _context.PollingStationCommission.Where(c => c.ElectionFk == id).Include("PollingStationCommissioner.PersonFkNavigation").Include("RelPollingStationSystemPollingStationCommission").ToListAsync();
            ViewData["ElectionName"] = (await _context.Election.FindAsync(id)).Name;
            ViewData["ElectionId"] = id;
            return View(pollingStation);
        }

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPSCommission(PollingStationCommissionUI comm, Guid? presidentFK)
        {
            var c = new PollingStationCommission();
            c.Id = Guid.NewGuid();
            // d.b. 22/10/2025 - PSCommission inherits PSGroupId from Election
            Election e = await _context.Election.FindAsync(comm.ElectionFk);
            comm.UpdatePollingStationCommission(c);
            c.PollingStationGroupId = e.PollingStationGroupId;
            _context.PollingStationCommission.Add(c);

            await _context.SaveChangesAsync();

            if (presidentFK.HasValue)
            {
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

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> DeletePSCommission(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            PollingStationCommission ps = await _context.PollingStationCommission.FindAsync(id);
            if (ps == null)
            {
                return NotFound();
            }

            Guid ElectionId = ps.ElectionFk;
            Election e = await _context.Election.FindAsync(ElectionId);
            if (e == null)
            {
                return NotFound();
            }

            if(e.Active)
            {
                return View("CustomError", ("Cancellazione commissione","Impossibile cancellare la commissione di una elezione attiva"));
            }

            ps.PresidentFk = null;
            await _context.SaveChangesAsync();

            _context.RelPollingStationSystemPollingStationCommission.RemoveRange(from x in _context.RelPollingStationSystemPollingStationCommission where x.PollingStationCommissionFk == id select x);
            _context.PollingStationCommissioner.RemoveRange(from x in _context.PollingStationCommissioner where x.PollingStationCommissionFk == id select x);
            _context.PollingStationCommission.Remove(ps);

            await _context.SaveChangesAsync();

            return RedirectToAction("EditPollingStations", new { id = ElectionId });
        }

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
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

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
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
        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> SetAsPresident(Guid commissionerId)
        {
            var c = await _context.PollingStationCommissioner.FindAsync(commissionerId);
            var pc = await _context.PollingStationCommission.FindAsync(c.PollingStationCommissionFk);

            pc.PresidentFk = commissionerId;
            await _context.SaveChangesAsync();

            return RedirectToAction("EditPollingStations", new { id = pc.ElectionFk });
        }

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]

        [HttpGet("Voters/{id}")]
        public async Task<IActionResult> Voters(Guid id, string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["electionId"] = id;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["LastNameSortParam"] = System.String.IsNullOrEmpty(sortOrder) ? "lastName_desc" : "";
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
            if (!System.String.IsNullOrEmpty(searchString))
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
            return View((await PaginatedList<Models.DB.Person>.CreateAsync(people.AsNoTracking(), pageNumber ?? 1, pageSize), await _context.Election.FindAsync(id)));
        }

        [HttpPost("VotersBulkUpload/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VotersBulkUpload(Guid id, List<IFormFile> file)
        {
            if (file.Count != 1)
            {
                return NotFound(); // FixMe should not be not found
            }
            var result = new List<(Person person, CsvPerson data, string reason)>();
            var conf = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.CurrentUICulture)
            {
                Delimiter = ";"
            };
            var lines = new List<CsvPerson>();
            using (var csv = new CsvReader(new System.IO.StreamReader(file[0].OpenReadStream()), conf))
            {
                lines = CsvUtils.ParsePeople(csv);

                var dups = from l in lines group l by l.PublicId into grp where grp.Count() > 1 select grp;

                foreach (var d in dups)
                {
                    foreach (var dl in d.Skip(1))
                    {
                        result.Add((null, dl, "Duplicate entry"));
                        lines.Remove(dl);
                    }
                }

                foreach (var l in lines.Where(l => l.Status != null))
                {
                    result.Add((null, l, l.Status));
                }

                var q = new List<Person>();
                var pids = lines.Select(l => l.PublicId).ToArray();
                for (var i = 0; i < pids.Length; i += 1000)
                {
                    var toadd = pids.Skip(i).Take(Math.Min(1000, pids.Length - i)).ToArray();
                    q.AddRange(from p in _context.Person where toadd.Contains(p.PublicId) select p);
                }
                var existingpids = q.Select(p => p.PublicId);

                foreach (var a in (from l in lines where l.Status == null && !existingpids.Contains(l.PublicId) select l))
                {
                    result.Add((null, a, "Person missing"));
                }

                var voters = (from v in _context.Voter where v.ElectionFk == id select v.PersonFk).ToArray();

                foreach (var p in (from pp in q where voters.Contains(pp.Id) select pp))
                {
                    result.Add((p, null, "Person already present"));
                }

                foreach (var p in (from pp in q where !voters.Contains(pp.Id) select pp))
                {
                    var toadd = new Voter()
                    {
                        Id = Guid.NewGuid(),
                        ElectionFk = id,
                        PersonFk = p.Id
                    };
                    _context.Voter.Add(toadd);
                    result.Add((p, null, null));
                }
                await _context.SaveChangesAsync();
            }

            return View(result);
        }


        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        [HttpGet("Candidates/{id}")]
        public async Task<IActionResult> Candidates(Guid id, string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["electionId"] = id;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["LastNameSortParam"] = System.String.IsNullOrEmpty(sortOrder) ? "lastName_desc" : "";
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
                         where bn.ElectionFk == id
                         select new EligibleCandidateBallotNameViewModel { Person = p, BallotName = bn };
            if (!System.String.IsNullOrEmpty(searchString))
            {
                people = people.Where(p => p.Person.LastName.Contains(searchString) || p.Person.FirstName.Contains(searchString) || p.Person.PublicId.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "lastName_desc":
                    people = people.OrderByDescending(p => p.Person.LastName);
                    break;
                default:
                    people = people.OrderBy(p => p.Person.LastName);
                    break;
            }
            int pageSize = 25;
            return View((await PaginatedList<EligibleCandidateBallotNameViewModel>.CreateAsync(people.AsNoTracking(), pageNumber ?? 1, pageSize), await _context.Election.FindAsync(id), _context));
        }

        [HttpPost("CandidatesBulkUpload/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CandidatesBulkUpload(Guid id, List<IFormFile> file)
        {
            if (file.Count != 1)
            {
                return NotFound(); // FixMe should not be not found
            }
            var result = new List<(Person person, CsvPerson data, string reason)>();
            var conf = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.CurrentUICulture)
            {
                Delimiter = ";"
            };
            var lines = new List<CsvPerson>();
            using (var csv = new CsvReader(new System.IO.StreamReader(file[0].OpenReadStream()), conf))
            {
                lines = CsvUtils.ParsePeople(csv);

                var dups = from l in lines group l by l.PublicId into grp where grp.Count() > 1 select grp;

                foreach (var d in dups)
                {
                    foreach (var dl in d.Skip(1))
                    {
                        result.Add((null, dl, "Duplicate entry"));
                        lines.Remove(dl);
                    }
                }

                foreach (var l in lines.Where(l => l.Status != null))
                {
                    result.Add((null, l, l.Status));
                }

                var q = new List<Person>();
                var pids = lines.Select(l => l.PublicId).ToArray();
                for (var i = 0; i < pids.Length; i += 1000)
                {
                    var toadd = pids.Skip(i).Take(Math.Min(1000, pids.Length - i)).ToArray();
                    q.AddRange(from p in _context.Person where toadd.Contains(p.PublicId) select p);
                }
                var existingpids = q.Select(p => p.PublicId);

                foreach (var a in (from l in lines where l.Status == null && !existingpids.Contains(l.PublicId) select l))
                {
                    result.Add((null, a, "Person missing"));
                }

                foreach (var p in q)
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
                }
                await _context.SaveChangesAsync();
            }

            return View(result);
        }

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
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

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
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

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        [HttpGet("ClonePSCommissions")]
        public async Task<IActionResult> ClonePSCommissions(Guid id)
        {
            var el = (await GetUpcomingElections()).Where(e => e.Id != id).OrderBy(e => e.Name);
            var del = await _context.Election.FindAsync(id);
            var list = new SelectList(el, "Id", "Name");
            ViewData["dst"] = del;
            return View(list);
        }

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
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

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        [HttpGet("Counters")]
        public async Task<IActionResult> Counters()
        {
            var activeelections = from e in _context.Election where (e.Configuring || e.Active) select e;
            var electionsq = from e in activeelections
                             join v in _context.Voter on e.Id equals v.ElectionFk
                             //group new { Election = e, Vote(e, v) } by v.Id into g 
                             where v.Vote.HasValue
                             select new { ElectionID = e.Id, ElectionName = e.Name, ElectionConfiguration = e.Configuration, Vote = v.Vote };

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

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        public async Task<IActionResult> Registry(Guid? id, string searchString, int? pageNumber = 1)
        {
            if (id == null)
            {
                return NotFound();
            }
            Election election = await _context.Election.FindAsync(id);
            if (election == null)
            {
                return NotFound();
            }

            ViewData["CurrentFilter"] = searchString;

            IQueryable<RegistryVoter> voters = ElectionMgmt.GetVoters(id.Value, _context);
            int count = await voters.CountAsync();
            if (!System.String.IsNullOrEmpty(searchString))
            {
                voters = voters.Where(v => v.LastName.Contains(searchString) || v.PublicID.Contains(searchString));
            }
            return View((await PaginatedList<RegistryVoter>.CreateAsync(voters, (int)pageNumber, 50), election.Description, count));
        }


        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        public async Task<IActionResult> RegistryDownload(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Election election = await _context.Election.FindAsync(id);
            if (election == null)
            {
                return NotFound();
            }

            IQueryable<RegistryVoter> voters = ElectionMgmt.GetVoters(id.Value, _context);
            var csvBytes = CsvUtils.ExportToCsv(voters.ToList());
            return File(csvBytes, "text/csv", "Votanti per " + election.Description.Trim() + ".csv");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetVRLink(Guid? ElectionId, Guid? commissionerid, string vr)
        {
            if (ElectionId == null || commissionerid == null)
            {
                return NotFound();
            }
            Election election = await _context.Election.FindAsync(ElectionId);
            if (election == null)
            {
                return NotFound();
            }
            PollingStationCommissioner c = await _context.PollingStationCommissioner.FindAsync(commissionerid);
            if (c == null)
            {
                return NotFound();
            }
            if (System.String.IsNullOrEmpty(vr))
            {
                return BadRequest();
            }

            c.VirtualRoom = vr;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return View("CustomError", ("Impossibile registrare l'aula virtuale", e.Message));
            }
            return RedirectToAction("EditPollingStations", new { id = ElectionId });

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
