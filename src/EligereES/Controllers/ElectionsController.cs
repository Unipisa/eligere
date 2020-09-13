using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EligereES.Models.DB;

namespace EligereES.Controllers
{
    public class ElectionsController : Controller
    {
        private readonly ESDB _context;

        public ElectionsController(ESDB context)
        {
            _context = context;
        }

        // GET: Elections
        public async Task<IActionResult> Index()
        {
            var eSDB = _context.Election.Include(e => e.ElectionTypeFkNavigation);
            return View(await eSDB.ToListAsync());
        }

        // GET: Elections/Details/5
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

        // GET: Elections/Create
        public IActionResult Create()
        {
            ViewData["ElectionTypeFk"] = new SelectList(_context.ElectionType, "Id", "Name");
            return View();
        }

        // POST: Elections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Elections/Edit/5
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

        // POST: Elections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        private bool ElectionExists(Guid id)
        {
            return _context.Election.Any(e => e.Id == id);
        }
    }
}
