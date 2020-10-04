using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EligereES.Models.DB;
using EligereES.Models.Client;

using SQLitePCL;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp;
using CsvHelper;
using System.Globalization;
using EligereES.Models.Extensions;

namespace EligereES.Controllers
{
    [Route("api/v1.0")]
    [ApiController]
    public class EligereESAPI : ControllerBase
    {
        private ESDB _context;

        public EligereESAPI(ESDB ctxt)
        {
            _context = ctxt;
        }

        [HttpGet("Election")]
        public async Task<List<ElectionUI>> ListElections(bool? valid)
        {
            var onlyvalid = valid ?? true;
            var q = from e in _context.Election where (!onlyvalid || (e.Active ?? false)) select e;
            var d = await q.ToListAsync();
            return d.ConvertAll(e => new ElectionUI(e));
        }

        [HttpGet("Election/{id}")]
        public async Task<ElectionUI> GetElection(Guid id)
        {
            var e = await _context.Election.FindAsync(id);
            return new ElectionUI(e);
        }

        [HttpPatch("Election/{id}")]
        public async Task<IActionResult> UpdateElection(Guid id, ElectionUI election)
        {
            var e = await _context.Election.FindAsync(id);
            election.UpdateElection(e);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("Election/{id}/PSCommission")]
        public async Task<List<PollingStationCommissionUI>> ListPSCommissions(Guid id)
        {
            var q = from c in _context.PollingStationCommission where c.ElectionFk == id select c;

            return (await q.ToListAsync()).ConvertAll(c => new PollingStationCommissionUI(c));
        }

        [HttpPut("Election/{id}/PSCommission")]
        public async Task<List<PollingStationCommissionUI>> NewPSCommissions(Guid id, PollingStationCommissionUI commission)
        {
            var com = new PollingStationCommission();
            com.Id = Guid.NewGuid();
            commission.Id = com.Id;
            commission.UpdatePollingStationCommission(com);
            _context.PollingStationCommission.Add(com);
            await _context.SaveChangesAsync();

            var q = from c in _context.PollingStationCommission where c.ElectionFk == id select c;

            return (await q.ToListAsync()).ConvertAll(c => new PollingStationCommissionUI(c));
        }

        [HttpGet("Election/{eid}/PSCommission/{id}")]
        public async Task<PollingStationCommissionUI> GetPSCommissions(Guid eid, Guid id)
        {
            var q = from c in _context.PollingStationCommission where c.ElectionFk == eid && c.Id == id select c;

            return new PollingStationCommissionUI(await q.FirstAsync());
        }

        [HttpPatch("Election/{eid}/PSCommission/{id}")]
        public async Task<IActionResult> UpdatePSCommissions(Guid eid, Guid id, PollingStationCommissionUI commission)
        {
            var q = from c in _context.PollingStationCommission where c.ElectionFk == eid && c.Id == id select c;
            var pc = await q.FirstAsync();
            commission.UpdatePollingStationCommission(pc);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("Election/{eid}/PSCommission/{id}/Staff")]
        public async Task<List<PersonUI>> GetPSCommissionStaff(Guid eid, Guid id)
        {
            var q = from c in _context.PollingStationCommission where c.ElectionFk == eid && c.Id == id select c;
            var l = (await q.FirstAsync()).PollingStationCommissioner.ToList().ConvertAll(c => new PersonUI(c.PersonFkNavigation));
            return l;
        }

        [HttpPut("Election/{eid}/PSCommission/{id}/Staff")]
        public async Task<List<PersonUI>> AddPSCommissionStaff(Guid eid, Guid id, PersonUI person)
        {
            var q = from c in _context.PollingStationCommission where c.ElectionFk == eid && c.Id == id select c;
            var comm = new PollingStationCommissioner();
            comm.Id = Guid.NewGuid();
            comm.PersonFk = person.Id;
            comm.PollingStationCommissionFk = id;
            _context.PollingStationCommissioner.Add(comm);
            await _context.SaveChangesAsync();
            var l = (await q.FirstAsync()).PollingStationCommissioner.ToList().ConvertAll(c => new PersonUI(c.PersonFkNavigation));
            return l;
        }

        [HttpDelete("Election/{eid}/PSCommission/{id}/Staff")]
        public async Task<List<PersonUI>> DeletePSCommissionStaff(Guid eid, Guid id, PersonUI person)
        {
            var q = from c in _context.PollingStationCommission 
                    join sc in _context.PollingStationCommissioner on c.Id equals sc.PollingStationCommissionFk 
                    where c.ElectionFk == eid && c.Id == id && person.Id == sc.PersonFk 
                    select sc;
            _context.PollingStationCommissioner.Remove(await q.FirstAsync());
            await _context.SaveChangesAsync();
            var staff = from c in _context.PollingStationCommission where c.ElectionFk == eid && c.Id == id select c;
            var l = (await staff.FirstAsync()).PollingStationCommissioner.ToList().ConvertAll(c => new PersonUI(c.PersonFkNavigation));
            return l;
        }

        [HttpGet("Election/{eid}/PSCommission/{id}/PollingStation")]
        public async Task<List<PollingStationSystemUI>> GetPSCommissionSystems(Guid eid, Guid id)
        {
            var q = from c in _context.PollingStationCommission where c.ElectionFk == eid && c.Id == id select c;
            var l = (await q.FirstAsync()).RelPollingStationSystemPollingStationCommission.ToList().ConvertAll(c => new PollingStationSystemUI(c.PollingStationSystemFkNavigation));
            return l;
        }

        [HttpPut("Election/{eid}/PSCommission/{id}/PollingStation")]
        public async Task<List<PollingStationSystemUI>> AddPSCommissionSystems(Guid eid, Guid id, PollingStationSystemUI system)
        {
            var ps = new PollingStationSystem();
            ps.Id = Guid.NewGuid();
            ps.Ipaddress = system.Ipaddress;
            ps.DigitalFootprint = system.DigitalFootprint;

            var rel = new RelPollingStationSystemPollingStationCommission();
            rel.Id = Guid.NewGuid();
            rel.PollingStationCommissionFk = id;
            rel.PollingStationSystemFk = ps.Id;

            _context.PollingStationSystem.Add(ps);
            _context.RelPollingStationSystemPollingStationCommission.Add(rel);

            await _context.SaveChangesAsync();

            var q = from c in _context.PollingStationCommission where c.ElectionFk == eid && c.Id == id select c;
            var l = (await q.FirstAsync()).RelPollingStationSystemPollingStationCommission.ToList().ConvertAll(c => new PollingStationSystemUI(c.PollingStationSystemFkNavigation));
            return l;
        }

        [HttpDelete("Election/{eid}/PSCommission/{id}/PollingStation")]
        public async Task<List<PollingStationSystemUI>> GetPSCommissionSystems(Guid eid, Guid id, PollingStationSystemUI system)
        {
            var q = from c in _context.PollingStationCommission
                    join rel in _context.RelPollingStationSystemPollingStationCommission on c.Id equals rel.PollingStationCommissionFk 
                    where c.ElectionFk == eid && c.Id == id && rel.PollingStationSystemFk == system.Id select rel;

            var r = await q.FirstAsync();

            _context.RelPollingStationSystemPollingStationCommission.Remove(r);

            if (!await _context.RelPollingStationSystemPollingStationCommission.Where(s => s.PollingStationSystemFk == system.Id).AnyAsync())
            {
                var s = await _context.PollingStationSystem.FindAsync(system.Id);
                _context.PollingStationSystem.Remove(s);
            }

            await _context.SaveChangesAsync();

            var qr = from c in _context.PollingStationCommission where c.ElectionFk == eid && c.Id == id select c;
            var l = (await qr.FirstAsync()).RelPollingStationSystemPollingStationCommission.ToList().ConvertAll(c => new PollingStationSystemUI(c.PollingStationSystemFkNavigation));
            return l;
        }

        [HttpGet("People")]
        public async Task<List<PersonUI>> FindPeople(string search)
        {
            var q = from p in _context.Person where search == null || p.FirstName.Contains(search) || p.LastName.Contains(search) || p.PublicId.Contains(search) select p;
            return (await q.ToListAsync()).ConvertAll(p => new PersonUI(p));
        }

        [HttpPost("People")]
        public async Task<List<(FullPersonUI person, FullPersonUI inputPerson, string reason)>> BulkUpload(List<IFormFile> file)
        {
            if (file.Count != 1)
            {
                throw new Exception("Invalid invocation");
            }
            var result = new List<(FullPersonUI person, FullPersonUI inputPerson, string reason)>();
            using (var csv = new CsvReader(new System.IO.StreamReader(file[0].OpenReadStream()), CultureInfo.CurrentUICulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (await csv.ReadAsync())
                {
                    var toAdd = new FullPersonUI();
                    toAdd.FirstName = csv.GetField<string>(0);
                    toAdd.LastName = csv.GetField<string>(1);
                    toAdd.PublicId = csv.GetField<string>(2);
                    toAdd.BirthDate = csv.GetField<DateTime>(3);
                    toAdd.BirthPlace = csv.GetField<string>(4);
                    toAdd.Attributes = (new PersonAttributes() { Role = csv.GetField<string>(5) }).ToJson();

                    // For the moment is a weak check before insert
                    if (await _context.Person.CountAsync(p => p.PublicId == toAdd.PublicId) > 0)
                    {
                        result.Add(
                            (new FullPersonUI(await _context.Person.FirstAsync(p => p.PublicId == toAdd.PublicId)),
                             toAdd, "Person already exists"));
                        // Add secure log entry here!
                    }
                    else
                    {
                        var np = new Person()
                        {
                            Id = Guid.NewGuid()
                        };
                        toAdd.UpdatePerson(np);
                        toAdd.Id = np.Id;
                        _context.Person.Add(np);
                        result.Add((toAdd, null, null));
                        // Add secure log entry here!
                    }
                }
                await _context.SaveChangesAsync();
            }
            return result;
        }

    }
}
