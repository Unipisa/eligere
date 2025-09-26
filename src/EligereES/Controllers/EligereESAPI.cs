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
using Microsoft.AspNetCore.Authorization;
using EligereES.Models;
using System.Security.Permissions;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace EligereES.Controllers
{
    [Authorize]
    [Route("api/v1.0")]
    [ApiController]
    public class EligereESAPI : ControllerBase
    {
        public const int DefaultMinutesBeforeElectionStarts = 15;
        public const int DefaultMinutesAfterElectionEnds = 15;
        public const int DefaultOTPLifetime = 30;
        public const int DefaultTicketLifetime = 30;
        public const int DefaultVoteIdentificationSpan = 3;

        private ESDB _context;
        private string contentRootPath;
        private IDataProtectionProvider dataProtector;
        private DownloadOTPManager _dotpmgr;
        private string defaultProvider;
        private int _minutesBeforeElectionStarts = DefaultMinutesBeforeElectionStarts;
        private int _minutesAfterElectionEnds = DefaultMinutesAfterElectionEnds;

        public EligereESAPI(ESDB ctxt, IWebHostEnvironment env, IDataProtectionProvider provider, IConfiguration configuration, DownloadOTPManager dotpmgr)
        {
            _context = ctxt;
            contentRootPath = env.ContentRootPath;
            dataProtector = provider;
            _dotpmgr = dotpmgr;
            defaultProvider = configuration.GetValue(typeof(string), "DefaultAuthProvider") as string;
            _minutesBeforeElectionStarts = configuration.GetValue(typeof(int), "MinutesBeforeElectionStarts") as int? ?? DefaultMinutesBeforeElectionStarts;
            _minutesAfterElectionEnds = configuration.GetValue(typeof(int), "MinutesAfterElectionEnds") as int? ?? DefaultMinutesAfterElectionEnds;
        }

        [HttpGet("Election")]
        public async Task<List<ElectionUI>> ListElections(bool? valid)
        {
            var onlyvalid = valid ?? true;
            var q = from e in _context.Election where (!onlyvalid || (e.Active && e.Started.HasValue && !e.Closed.HasValue)) select e;
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
        [AuthorizeRoles(EligereRoles.ElectionOfficer)]
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
        [AuthorizeRoles(EligereRoles.ElectionOfficer)]
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
        [AuthorizeRoles(EligereRoles.ElectionOfficer)]
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
        [AuthorizeRoles(EligereRoles.ElectionOfficer)]
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
        [AuthorizeRoles(EligereRoles.ElectionOfficer)]
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
        [AuthorizeRoles(EligereRoles.ElectionOfficer)]
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
        [AuthorizeRoles(EligereRoles.ElectionOfficer)]
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
            var q = from p in _context.Person where search == null || (p.FirstName + " " + p.LastName).Contains(search) || p.PublicId.Contains(search) select p;
            return (await q.ToListAsync()).ConvertAll(p => new PersonUI(p));
        }

        [HttpPost("People")]
        [AuthorizeRoles(EligereRoles.ElectionOfficer)]
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

        [AllowAnonymous]
        [HttpGet("GetEncryptionKey")]
        public string GetEncryptionKey(string otp)
        {
            if (_dotpmgr.HasOTP)
            {
                if (otp == _dotpmgr.OTP)
                {
                    var k = new System.IO.DirectoryInfo(System.IO.Path.Combine(contentRootPath, "Data/EVSKey/"));
                    var f = k.GetFiles("*.xml");
                    if (f.Length > 1) throw new Exception("Keyring must contain a single Key");
                    var fn = f[0].Name;
                    var kf = System.IO.File.ReadAllText(f[0].FullName);
                    _dotpmgr.ResetOTP(); // One Time is One Time :)
                    return $"{fn}::{kf}";
                } else {
                    return "KO";
                }
            } else {
                return "KO";
            }
        }

        [AllowAnonymous]
        [HttpGet("TestEligereESConnection")]
        public string TestEligereESConnection(string test)
        {
            var dp = dataProtector.CreateProtector("EligereTest");
            var data = dp.Protect(test);
            return data;
        }

        [AllowAnonymous]
        [HttpPut("StartElections")]
        public async Task<ActionResult> StartElections([FromForm] string data)
        {
            var dp = dataProtector.CreateProtector("EligereMetadataExchange");
            var plainData = dp.Unprotect(data);
            var electionIdsData = JsonSerializer.Deserialize<List<string>>(plainData);
            var electionIds = electionIdsData.ConvertAll(e => Guid.Parse(e));
            var elections = await (from e in _context.Election where electionIds.Contains(e.Id) select e).ToListAsync();
            foreach (var e in elections)
            {
                e.Started = DateTime.Now;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("CloseElections")]
        public async Task<ActionResult> CloseElections([FromForm] string data)
        {
            var dp = dataProtector.CreateProtector("EligereMetadataExchange");
            var plainData = dp.Unprotect(data);
            var electionIdsData = JsonSerializer.Deserialize<List<string>>(plainData);
            var electionIds = electionIdsData.ConvertAll(e => Guid.Parse(e));
            var elections = await (from e in _context.Election where electionIds.Contains(e.Id) select e).ToListAsync();
            foreach (var e in elections)
            {
                e.Closed = DateTime.Now;
                e.Configuring = false;
                e.Active = false;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("RunningElections")]
        public async Task<string> RunningElections()
        {
            var reqip = Request.HttpContext.Connection.RemoteIpAddress;
            var conf = SetupController.GetESConfiguration(contentRootPath);
            var acl = new List<IPAddress>();

            if (conf.VotingSystemTicketAPI != null)
            {
                var host = new Uri(conf.VotingSystemTicketAPI).Host;
                foreach (var a in Dns.GetHostEntry(host).AddressList)
                {
                    acl.Add(a);
                }
            }

            var check = acl.FirstOrDefault(a => a.Equals(reqip));
            if (check == default(IPAddress)) {
                Response.StatusCode = 403;
                return null;
            }

            var time4open = DateTime.Now + TimeSpan.FromMinutes(_minutesBeforeElectionStarts); // Early comers
            var time4close = DateTime.Now - TimeSpan.FromMinutes(_minutesAfterElectionEnds); //Late comers
            var electionsq = from e in _context.Election
                            where e.Configuring && (e.PollStartDate <= time4open) && (e.PollEndDate >= time4close)
                            select e;

            var elections = await electionsq.ToListAsync();
            var electionids = elections.ConvertAll(e => e.Id);

            // FixMe: this should be changed when multiple VS systems will be introduced
            if (elections.Count > 1 && elections.GroupBy(e => e.PollingStationGroupId).Count() > 1)
                throw new Exception("Multiple elections with different PollingStationGroupIds");

            var groupid = elections[0].PollingStationGroupId == null ? Guid.NewGuid() : elections[0].PollingStationGroupId;

            // In general this would be empty, but in a scenario with multiple days of voting if one or more elections
            // start after the first day they will be missing in the voting system. Therefore we must ensure that all the
            // elections in the same group are present in the voting system
            var missingElectionIds = await (from e in _context.Election
                                      where !electionids.Contains(e.Id) && e.PollingStationGroupId == groupid
                                      select e.Id).ToListAsync();
            var missingElections = await (from e in _context.Election
                                          where missingElectionIds.Contains(e.Id)
                                          select e).ToListAsync();

            if (missingElectionIds.Count > 0)
            {
                electionids.AddRange(missingElectionIds);
                elections.AddRange(missingElections);
            }


            // Ignoring name, election_scope_id, start_date, end_date, type, geopolitical_units
            var electionDescription = new ElectionGuard.ElectionDescription();

            electionDescription.ballot_styles = 
                new ElectionGuard.BallotStyle[] { 
                    new ElectionGuard.BallotStyle() { 
                        object_id = groupid.ToString() 
                    } 
                };

            var partiesq = from p in _context.Party
                           join b in _context.BallotName on p.Id equals b.PartyFk
                           where electionids.Contains(b.ElectionFk)
                           select p;

            var parties = await partiesq.ToListAsync();

            if (parties.Count > 0)
            {
                var partydescs = parties.ConvertAll(p => new ElectionGuard.PartyDescription()
                {
                    object_id = p.Id.ToString(),
                    name = new ElectionGuard.ListOfLocalizedText()
                    {
                        text = new ElectionGuard.LocalizedText[] {
                            new ElectionGuard.LocalizedText() { language = "it", value = p.Name }
                        }
                    },
                    logo_uri = p.LogoUri
                });
                electionDescription.parties = partydescs.ToArray();
            }


            var ballotnamesq = from b in _context.BallotName
                           where electionids.Contains(b.ElectionFk)
                           select b;

            var ballotnames = await ballotnamesq.ToListAsync();

            var ballotids = new Dictionary<Guid, string>();

            electionDescription.candidates = 
                ballotnames.ConvertAll(b => {
                    ballotids.Add(b.Id, (b.IsCandidate.HasValue && b.IsCandidate.Value ? "*" : "") + b.Id.ToString());
                    return new ElectionGuard.Candidate()
                    {
                        object_id = ballotids[b.Id],
                        ballot_name = new ElectionGuard.BallotName()
                        {
                            party_id = b.PartyFk.ToString(),
                            text = new ElectionGuard.LocalizedText[] {
                            new ElectionGuard.LocalizedText() { language = "it", value = b.BallotNameLabel }
                        }
                        }
                    };
                }).ToArray();

            var contests = new List<ElectionGuard.Contest>();
            var i = 0;
            foreach (var e in elections)
            {
                var contest = new ElectionGuard.Contest();
                contest.object_id = e.Id.ToString();
                contest.name = e.Name;
                contest.sequence_order = i++;
                var config = ElectionConfiguration.FromJson(e.Configuration);
                contest.votes_allowed = config.NumPreferences;
                contest.number_elected = config.EligibleSeats;
                contest.electoral_district_id = "https://eligere.unipi.it"; // To be added to configuration
                contest.extensions = new Dictionary<string, string>();
                contest.extensions["HasCandidates"] = config.HasCandidates.ToString();
                contest.extensions["CandidatesType"] = config.CandidatesType.ToString();
                contest.extensions["IdentificationType"] = config.IdentificationType.ToString();
                contest.extensions["PollStartDate"] = e.PollStartDate.ToString(CultureInfo.GetCultureInfo("it-it"));
                contest.extensions["PollEndDate"] = e.PollEndDate.ToString(CultureInfo.GetCultureInfo("it-it"));
                contest.extensions["NoNullVote"] = config.NoNullVote.ToString();
                contest.extensions["VoterCount"] = _context.Voter.Where(v => v.ElectionFk == e.Id).Count().ToString();
                var ob = ballotnames.Where(b => b.ElectionFk == e.Id && b.SequenceOrder.HasValue).OrderBy(b => b.SequenceOrder).ThenBy(b => b.BallotNameLabel).ToList();
                var ub = ballotnames.Where(b => b.ElectionFk == e.Id && !b.SequenceOrder.HasValue).OrderBy(b => b.BallotNameLabel).ToList();
                ob.AddRange(ub);
                var ballotselections = new List<ElectionGuard.BallotSelection>();
                var j = 0;
                foreach (var b in ob)
                {
                    var bs = new ElectionGuard.BallotSelection();
                    bs.object_id = ballotids[b.Id]; // Should be ok
                    bs.candidate_id = ballotids[b.Id];
                    bs.sequence_order = j++;
                    ballotselections.Add(bs);
                }
                contest.ballot_selections = ballotselections.ToArray();
                contests.Add(contest);
            }
            electionDescription.contests = contests.ToArray();
            var sret = JsonSerializer.Serialize<ElectionGuard.ElectionDescription>(electionDescription);
            var dp = dataProtector.CreateProtector("EligereMetadataExchange");
            return dp.Protect(sret);
        }

        //Todo: for security reasons would be better to use post
        [AllowAnonymous]
        [HttpGet("TicketUsed/{hash}")]
        public async Task<bool> TicketUsed(string hash)
        {
            var reqip = Request.HttpContext.Connection.RemoteIpAddress;
            var conf = SetupController.GetESConfiguration(contentRootPath);
            //var acl = new List<IPAddress>();

            //if (conf.VotingSystemTicketAPI != null)
            //{
            //    var host = new Uri(conf.VotingSystemTicketAPI).Host;
            //    foreach (var a in Dns.GetHostEntry(host).AddressList)
            //    {
            //        acl.Add(a);
            //    }
            //}

            //var check = acl.FirstOrDefault(a => a.Equals(reqip));
            //if (check == default(IPAddress))
            //{
            //    Response.StatusCode = 403;
            //    return false;
            //}

            var dp = dataProtector.CreateProtector("EligereMetadataExchange");
            var plainHash = dp.Unprotect(hash);

            var ticketq = from t in _context.VotingTicket
                          join v in _context.Voter on t.Id equals v.VotingTicketFk
                          where t.Hash == plainHash
                          select t;

            var ticket = await ticketq.FirstOrDefaultAsync();
            if (ticket == null)
                throw new Exception($"Invalid ticket hash {plainHash}");

            var voter = await _context.Voter.Include(v => v.ElectionFkNavigation).FirstOrDefaultAsync(v => v.VotingTicketFk == ticket.Id);
            if (ticket == null)
                throw new Exception($"Internal error, inconsistent DB on ticket");

            var rnd = new Random();
            voter.Vote = DateTime.Now + TimeSpan.FromMinutes(rnd.Next(10) - 5);
            await _context.SaveChangesAsync();

            return true;
        }

        [AuthorizeRoles(EligereRoles.PollingStationStaff)]
        [HttpPut("PSCommission/StaffAvailability")]
        public async Task<bool> StaffAvailability(bool available)
        {
            var pfk = EligereRoles.PersonFK(this.User); // Authorized roles imply pfk non null
            var person = await _context.Person.FindAsync(pfk);

            var now = DateTime.Now + TimeSpan.FromMinutes(_minutesBeforeElectionStarts);
            var today = DateTime.Today;
            var psc = await (from pc in _context.PollingStationCommissioner
                             join c in _context.PollingStationCommission on pc.PollingStationCommissionFk equals c.Id
                             join e in _context.Election on c.ElectionFk equals e.Id
                             where e.PollStartDate <= now && e.PollEndDate > today && pc.PersonFk == person.Id
                             select pc).ToListAsync();

            foreach (var p in psc)
            {
                p.AvailableForRemoteRecognition = available;
            }

            await _context.SaveChangesAsync();

            return available;
        }

        [AuthorizeRoles(EligereRoles.PollingStationStaff)]
        [HttpGet("PSCommission/StaffAvailability")]
        public async Task<bool> StaffAvailability()
        {
            var pfk = EligereRoles.PersonFK(this.User); // Authorized roles imply pfk non null
            var person = await _context.Person.FindAsync(pfk);

            var now = DateTime.Now + TimeSpan.FromMinutes(_minutesBeforeElectionStarts);
            var today = DateTime.Today;
            return await (from pc in _context.PollingStationCommissioner
                             join c in _context.PollingStationCommission on pc.PollingStationCommissionFk equals c.Id
                             join e in _context.Election on c.ElectionFk equals e.Id
                             where e.PollStartDate <= now && e.PollEndDate > today && pc.PersonFk == person.Id && pc.AvailableForRemoteRecognition
                             select pc).AnyAsync();
        }

        [AuthorizeRoles(EligereRoles.RemoteIdentificationOfficer)]
        [HttpGet("PSCommission/RemoteIdAvailability")]
        public async Task<bool> RemoteIdAvailability()
        {
            var pfk = EligereRoles.PersonFK(this.User); // Authorized roles imply pfk non null
            var person = await _context.Person.FindAsync(pfk);

            var now = DateTime.Now + TimeSpan.FromMinutes(_minutesBeforeElectionStarts);
            var today = DateTime.Today;
            return await (from pc in _context.RemoteIdentificationCommissioner
                             join c in _context.PollingStationCommission on pc.PollingStationCommissionFk equals c.Id
                             join e in _context.Election on c.ElectionFk equals e.Id
                             where e.PollStartDate <= now && e.PollEndDate > today && pc.PersonFk == person.Id && pc.AvailableForRemoteRecognition
                             select pc).AnyAsync();
        }

        [AuthorizeRoles(EligereRoles.RemoteIdentificationOfficer)]
        [HttpPut("PSCommission/RemoteIdAvailability")]
        public async Task<bool> RemoteIdAvailability(bool available)
        {
            var pfk = EligereRoles.PersonFK(this.User); // Authorized roles imply pfk non null
            var person = await _context.Person.FindAsync(pfk);

            var now = DateTime.Now + TimeSpan.FromMinutes(_minutesBeforeElectionStarts);
            var today = DateTime.Today;
            var psc = await (from pc in _context.RemoteIdentificationCommissioner
                             join c in _context.PollingStationCommission on pc.PollingStationCommissionFk equals c.Id
                             join e in _context.Election on c.ElectionFk equals e.Id
                             where e.PollStartDate <= now && e.PollEndDate > today && pc.PersonFk == person.Id
                             select pc).ToListAsync();

            foreach (var p in psc)
            {
                p.AvailableForRemoteRecognition = available;
            }

            await _context.SaveChangesAsync();

            return available;
        }

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        [HttpGet("Elections/ApproveUserLoginRequest/{id}")]
        public async Task<bool> ApproveUserLoginRequest(Guid id)
        {
            var pfk = EligereRoles.PersonFK(this.User); // Authorized roles imply pfk non null
            var person = await _context.Person.FindAsync(pfk);

            var r = await _context.UserLoginRequest.FindAsync(id);
            if (r == null || r.Approval.HasValue) return false;
            var nr = new UserLogin() { 
                Id=Guid.NewGuid(),
                Provider = r.Provider, 
                UserId =r.UserId,
                PersonFk=r.PersonFk
            };
            r.Approved = true;
            r.Approval = DateTime.Now;
            r.Approver = person.Id;
            _context.UserLogin.Add(nr);
            await _context.SaveChangesAsync();
            return true;
        }

        //[AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        [HttpGet("Elections/ApproveUserLoginRequests")]
        public async Task<bool> ApproveUserLoginRequest()
        {
            var pfk = EligereRoles.PersonFK(this.User); // Authorized roles imply pfk non null
            var person = await _context.Person.FindAsync(pfk);

            var reqs = await _context.UserLoginRequest.Where(r => !r.Approval.HasValue).ToListAsync();
            foreach (var r in reqs)
            {
                if (r == null || r.Approval.HasValue) return false;
                var nr = new UserLogin()
                {
                    Id = Guid.NewGuid(),
                    Provider = r.Provider,
                    UserId = r.UserId,
                    PersonFk = r.PersonFk
                };
                r.Approved = true;
                r.Approval = DateTime.Now;
                r.Approver = person.Id;
                _context.UserLogin.Add(nr);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        [HttpGet("Elections/RejectUserLoginRequest/{id}")]
        public async Task<bool> RejectUserLoginRequest(Guid id)
        {
            var pfk = EligereRoles.PersonFK(this.User); // Authorized roles imply pfk non null
            var person = await _context.Person.FindAsync(pfk);

            var r = await _context.UserLoginRequest.FindAsync(id);
            if (r == null || r.Approval.HasValue) return false;
            r.Approved = false;
            r.Approval = DateTime.Now;
            r.Approver = person.Id;
            await _context.SaveChangesAsync();
            return true;
        }
        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin)]
        [HttpGet("Test/SendEmailOTP/{id}")]
        public bool SendEmailOTP(string id)
        {
            OTPSender.SendMail(null, "123", id);
            return true;
        }

    }
}
