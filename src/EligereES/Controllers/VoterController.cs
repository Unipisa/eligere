using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EligereES.Models;
using EligereES.Models.Client;
using EligereES.Models.DB;
using EligereES.Models.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EligereES.Controllers
{
    [Route("Voter")]
    [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin, EligereRoles.Voter)]
    public class VoterController : Controller
    {
        private static Random rnd = new Random();
        private ESDB _context;
        private string contentRootPath;
        private IDataProtectionProvider dataprotection;

        public VoterController(ESDB ctxt, IWebHostEnvironment env, IDataProtectionProvider provider)
        {
            _context = ctxt;
            contentRootPath = env.ContentRootPath;
            dataprotection = provider;
        }

        private async Task<List<(Voter, Election, List<PollingStationCommission>, bool, DateTime?)>> GetElections(Person person)
        {
            var q = from v in _context.Voter
                    join e in _context.Election on v.ElectionFk equals e.Id
                    where v.PersonFk == person.Id
                    select new { Voter = v, Election = e, Past = e.PollEndDate < DateTime.Today, HasVoted = v.Vote };

            var qd = await q.ToListAsync();

            var el = qd.Where(v => !v.Past).ToList().ConvertAll(v => v.Election.Id);

            var psq = from s in _context.PollingStationCommission
                      where el.Contains(s.ElectionFk)
                      select s;

            var psqdata = (await psq.ToListAsync()).GroupBy(v => v.ElectionFk);

            var d = from v in qd
                    join s in psqdata on v.Election.Id equals s.Key into comm
                    from sc in comm.DefaultIfEmpty()
                    select new { Voter = v.Voter, Election = v.Election, Past = v.Past, HasVoted = v.HasVoted, Commission = sc?.ToList() ?? new List<PollingStationCommission>() };

            var data = d.ToList().ConvertAll(v => (v.Voter, v.Election, v.Commission, v.Past, v.HasVoted));

            var activeel = data.Where(d => !d.Past).ToList();

            // integrity check for multiple elections and commissions
            if (activeel.Count > 1)
            {
                // integrity check for multiple elections and commissions
                var eg = activeel[0].Election.PollingStationGroupId;
                for (var i = 1; i < activeel.Count; i++)
                    if (activeel[i].Election.PollingStationGroupId != eg) 
                        throw new Exception("Internal error: multiple election groups for voter " + activeel[i].Voter.PersonFk);

                foreach (var vd in activeel)
                {
                    foreach (var pc in vd.Commission)
                    {
                        if (pc.PollingStationGroupId != eg)
                            throw new Exception("Internal error: PollingStationGroupId differ from election for voter" + vd.Voter.PersonFk);
                    }
                }
            }

            return data;
        }

        private ESConfiguration ReadElectionConf()
        {
            var fn = System.IO.Path.Combine(contentRootPath, "Data/apiconf.js");

            if (!System.IO.File.Exists(fn))
            {
                throw new Exception("System configuration error: API Endpoint configuration is missing!");
            }
            return ESConfiguration.FromJson(System.IO.File.ReadAllText(fn));
        }

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        [HttpGet("Impersonate/{id}")]
        public async Task<IActionResult> Impersonate(string id)
        {
            var person = await _context.Person.FirstAsync(p => p.PublicId == id);

            return View("Index", (ReadElectionConf(), person, await GetElections(person)));
        }

        [HttpGet("IdentificationLink")]
        public async Task<IActionResult> IdentificationLink()
        {
            var pq = from p in _context.Person
                     join u in _context.UserLogin on p.Id equals u.PersonFk
                     where u.Provider == "AzureAD" && u.UserId == this.User.Identity.Name
                     select p;

            if (await pq.CountAsync() != 1)
                throw new Exception("Internal error! Too many persons associated with login " + this.User.Identity.Name);


            var person = await pq.FirstAsync();

            var comm = new List<Guid>();
            var elections = new List<Guid>();

            foreach (var (voter, election, commissions, past, voted) in await GetElections(person))
            {
                if (election.Active && !voted.HasValue)
                {
                    elections.Add(election.Id);
                    foreach (var c in commissions)
                    {
                        comm.Add(c.Id);
                    }
                }
            }

            // 30 out of 100 are sent to some commissioner. 70 if there is an affine commissioner will be preferred
            if (rnd.Next(100) > 30)
            {
                var affineCommissioners = await (from ar in _context.IdentificationCommissionerAffinityRel
                                                 where elections.Contains(ar.ElectionFk)
                                                 select ar.PersonFk).Distinct().ToListAsync();

                var affineIds = await (from psc in _context.PollingStationCommissioner
                                       where comm.Contains(psc.PollingStationCommissionFk) && affineCommissioners.Contains(psc.PersonFk) && psc.AvailableForRemoteRecognition
                                       select psc.VirtualRoom).Distinct().ToListAsync();

                var affineRids = await (from psc in _context.RemoteIdentificationCommissioner
                                        where comm.Contains(psc.PollingStationCommissionFk) && affineCommissioners.Contains(psc.PersonFk) && psc.AvailableForRemoteRecognition
                                        select psc.VirtualRoom).Distinct().ToListAsync();

                affineIds.AddRange(affineRids);

                if (affineIds.Count > 0)
                    return Redirect(affineIds[rnd.Next(affineIds.Count)]);
            }

            var ids = await (from psc in _context.PollingStationCommissioner
                              where comm.Contains(psc.PollingStationCommissionFk) && psc.AvailableForRemoteRecognition
                              select psc.VirtualRoom).Distinct().ToListAsync();
            var rids = await (from psc in _context.RemoteIdentificationCommissioner
                              where comm.Contains(psc.PollingStationCommissionFk) && psc.AvailableForRemoteRecognition
                              select psc.VirtualRoom).Distinct().ToListAsync();

            ids.AddRange(rids);

            return Redirect(ids[rnd.Next(ids.Count)]);
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

            return View((ReadElectionConf(), person, await GetElections(person)));
        }

        private static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


        [HttpGet("GenerateTicket/{otp}")]
        [AuthorizeRoles(EligereRoles.Voter)]
        public async Task<IActionResult> GenerateTicket(string otp)
        {
            var pq = from p in _context.Person
                     join u in _context.UserLogin on p.Id equals u.PersonFk
                     where u.Provider == "AzureAD" && u.UserId == this.User.Identity.Name
                     select p;

            if (await pq.CountAsync() != 1)
                throw new Exception("Internal error! Too many persons associated with login " + this.User.Identity.Name);

            var person = await pq.FirstAsync();

            var votes = await GetElections(person);

            var availableVotes = votes.Where(v => !v.Item4 && !v.Item5.HasValue).Select(v => v.Item1.Id).ToList();

            if (availableVotes.Count == 0)
                return Forbid("No ballot to cast");

            var ckvotes = await (from v in _context.Voter
                                 join r in _context.Recognition on v.RecognitionFk equals r.Id
                                 join e in _context.Election on v.ElectionFk equals e.Id
                                 where availableVotes.Contains(v.Id) && !v.Vote.HasValue // ensure that the vote has not been casted
                                 select new { Voter = v, Recognition = r, Election = e }).ToListAsync();

            var otps = ckvotes.GroupBy(v => v.Recognition.Otp);
            if (otps.Count() != 1)
                throw new Exception("Internal error: too many Otps for ticket generation");

            if (otp != otps.First().Key)
                return Forbid("OTP non valida");

            // Ticket generation
            var tickets = new List<VoteTicket>();
            foreach (var v in ckvotes)
            {
                var ticketIdClearText = v.Voter.Id + ":" + v.Voter.PersonFk;
                var sha256 = SHA256.Create();
                var ticket = new VoteTicket();
                ticket.HashId = ComputeSha256Hash(ticketIdClearText);
                ticket.Issued = DateTime.Now;
                ticket.Expiration = DateTime.Now + TimeSpan.FromMinutes(30);
                ticket.ElectionId = v.Voter.ElectionFk.ToString();
                ticket.ElectionName = v.Election.Name;
                tickets.Add(ticket);
                v.Recognition.State = 1;

                var dbt = new VotingTicket()
                {
                    Id = Guid.NewGuid(),
                    Hash = ticket.HashId,
                    Content = ticket.ToJson(),
                    VoterFk = v.Voter.Id
                };

                v.Voter.VotingTicketFk = dbt.Id;

                _context.VotingTicket.Add(dbt);
            }

            await _context.SaveChangesAsync();

            var sertickets = System.Text.Json.JsonSerializer.Serialize(tickets);

            var protector = dataprotection.CreateProtector("EVSKeyExchange");
            var secureTickets = protector.Protect(Encoding.UTF8.GetBytes(sertickets));

            return Json(Convert.ToBase64String(secureTickets));
        }
    }
}
