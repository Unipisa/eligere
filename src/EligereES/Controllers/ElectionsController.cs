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

        public async Task<IActionResult> Index()
        {
            var eSDB = _context.Election.Include(e => e.ElectionTypeFkNavigation).Include(e => e.PollingStationCommission).Include(e => e.Voter).Include(e => e.EligibleCandidate);
            return View(await eSDB.ToListAsync());
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
        public async Task<IActionResult> AddPSCommission(PollingStationCommissionUI comm, Guid presidentFK)
        {
            var c = new PollingStationCommission();
            c.Id = Guid.NewGuid();
            comm.UpdatePollingStationCommission(c);
            _context.PollingStationCommission.Add(c);

            await _context.SaveChangesAsync();

            var memb = new PollingStationCommissioner();
            memb.Id = Guid.NewGuid();
            memb.PersonFk = presidentFK;
            memb.PollingStationCommissionFk = c.Id;

            c.PresidentFk = memb.Id;
            _context.PollingStationCommissioner.Add(memb);
            await _context.SaveChangesAsync();
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
                    var firstName = csv.GetField<string>(0);
                    var lastName = csv.GetField<string>(1);
                    var companyId = csv.GetField<string>(2);
                    var publicId = csv.GetField<string>(3);
                    var birthPlace = csv.GetField<string>(4);
                    var birthDate = csv.GetField<DateTime>(5);
                    var role = csv.GetField<string>(6);

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

            var people = from p in _context.Person join c in _context.EligibleCandidate on p.Id equals c.PersonFk where c.ElectionFk == id select p;
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

                    var q = from p in _context.Person join c in _context.EligibleCandidate on p.Id equals c.PersonFk where c.ElectionFk == id select p;
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
                            var toadd = new EligibleCandidate()
                            {
                                Id = Guid.NewGuid(),
                                ElectionFk = id,
                                PersonFk = p.Id
                            };
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
                       where c.ElectionFk == electionid && c.PersonFk == personid
                       select c).FirstOrDefaultAsync();

            if (cand == null) throw new Exception("Candidate not found");

            _context.EligibleCandidate.Remove(cand);
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

        private bool ElectionExists(Guid id)
        {
            return _context.Election.Any(e => e.Id == id);
        }
    }
}
