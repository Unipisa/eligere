﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EligereES.Models;
using EligereES.Models.DB;
using Microsoft.AspNetCore.Http;
using CsvHelper;
using System.Globalization;
using System.Data;
using EligereES.Models.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace EligereES.Controllers
{
    [Authorize]
    public class PeopleController : Controller
    {
        private readonly ESDB _context;

        public PeopleController(ESDB context)
        {
            _context = context;
        }

        // GET: People
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["LastNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "lastName_desc" : "";
            ViewData["BirthDateSortParam"] = sortOrder == "birthDate" ? "birthDate_desc" : "birthDate";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageNumber = 1;
            } else
            {
                searchString = currentFilter;
            }

            var people = from p in _context.Person select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                people = people.Where(p => p.LastName.Contains(searchString) || p.FirstName.Contains(searchString) || p.PublicId.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "lastName_desc":
                    people = people.OrderByDescending(p => p.LastName);
                    break;
                case "birthDate":
                    people = people.OrderBy(p => p.BirthDate);
                    break;
                case "birthDate_desc":
                    people = people.OrderByDescending(p => p.BirthDate);
                    break;
                default:
                    people = people.OrderBy(p => p.LastName);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Models.DB.Person>.CreateAsync(people.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,PublicId,BirthDate,BirthPlace,Attributes")] Person person)
        {
            if (ModelState.IsValid)
            {
                person.Id = Guid.NewGuid();
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FirstName,LastName,PublicId,BirthDate,BirthPlace,Attributes")] Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
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
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var person = await _context.Person.FindAsync(id);
            _context.Person.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkUpload(List<IFormFile> file)
        {
            if (file.Count != 1)
            {
                return NotFound(); // FixMe should not be not found
            }
            var result = new List<(Person person, CsvPerson data, string reason)>();
            var conf = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.CurrentUICulture) { 
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

                foreach (var l in lines.Where(l=> l.Status != null))
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
                    var toadd = new Person()
                    {
                        Id = Guid.NewGuid(),
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        PublicId = a.PublicId,
                        BirthDate = a.BirthDate,
                        BirthPlace = a.BirthPlace,
                        Attributes = (new PersonAttributes() { Role = a.Role, CompanyId = a.CompanyId }).ToJson()
                    };
                    _context.Person.Add(toadd);
                    result.Add((toadd, null, null));
                }

                foreach (var a in (from l in lines where l.Status == null && existingpids.Contains(l.PublicId) select l))
                {
                        result.Add(
                            (q.Where(p => p.PublicId == a.PublicId).First(),
                            a,
                            "Person already exists"));
                }

                await _context.SaveChangesAsync();
            }
            return View(result);
        }

        private bool PersonExists(Guid id)
        {
            return _context.Person.Any(e => e.Id == id);
        }
    }
}
