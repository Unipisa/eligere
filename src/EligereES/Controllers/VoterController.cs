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
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EligereES.Controllers
{
    [Route("Voter")]
    public class VoterController : Controller
    {
        private ESDB _context;
        private string contentRootPath;
        private IDataProtectionProvider dataprotection;
        private IConfiguration Configuration;
        private PersistentCommissionManager _manager;
        private string defaultProvider;

        public VoterController(ESDB ctxt, IWebHostEnvironment env, PersistentCommissionManager manager, IDataProtectionProvider provider, IConfiguration configuration)
        {
            _context = ctxt;
            contentRootPath = env.ContentRootPath;
            _manager = manager;
            _manager.Expiration = TimeSpan.FromMinutes(3); // Should be added to configuration
            dataprotection = provider;
            Configuration = configuration;
            defaultProvider = configuration.GetValue(typeof(string), "DefaultAuthProvider") as string;
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

            var activeel = data.Where(d => !d.Past && d.Election.Active).ToList();

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

        public static bool isDesktopOS(string ua)
        {
            var uap = UAParser.Parser.GetDefault();
            var info = uap.Parse(ua);
            return info.OS.Family == "Windows" || info.OS.Family == "Linux" || info.OS.Family == "Mac OS X";
        }

        private string selectVirtualRoom(string virtualRoom)
        {
            var ua = Request.Headers["User-Agent"];

            var desktop = virtualRoom;
            var nodesktop = virtualRoom.Replace("l/call/", "l/chat/").Replace("&withvideo=true", "");

            return  isDesktopOS(ua) ? desktop : nodesktop;
        }

        [HttpGet("Test")]
        public string Test()
        {
            return selectVirtualRoom("https://teams.microsoft.com/l/call/0/0?users=a010223@unipi.it&withvideo=true");
        }

        [AuthorizeRoles(EligereRoles.ElectionOfficer, EligereRoles.Admin, EligereRoles.Voter)]
        [HttpGet("IdentificationLink")]
        public async Task<IActionResult> IdentificationLink()
        {
            var userid = EligereRoles.UserId(this.User);
            var pfk = EligereRoles.PersonFK(this.User);
            if (!pfk.HasValue)
                throw new Exception("Internal error! Cannot associate PublicID with current user");

            var person = await _context.Person.FindAsync(pfk);

            var comm = new List<Guid>();
            var elections = new List<Guid>();
            var samplingrate = 0.0;
            IdentificationType? idtype = default;

            foreach (var (voter, election, commissions, past, voted) in await GetElections(person))
            {
                if (election.Active && !voted.HasValue)
                {
                    if (!idtype.HasValue)
                    {
                        idtype = election.ElectionConfiguration.IdentificationType;
                        if (idtype == IdentificationType.Sampling)
                            samplingrate = election.ElectionConfiguration.SamplingRate;
                    } else
                    {
                        if (idtype != election.ElectionConfiguration.IdentificationType)
                            throw new Exception("Identification type should be the same for all elections associated with a voter");
                        if (idtype == IdentificationType.Sampling && samplingrate != election.ElectionConfiguration.SamplingRate)
                            throw new Exception("For sampled identification you should use the same rate");
                    }
                    elections.Add(election.Id);
                    foreach (var c in commissions)
                    {
                        comm.Add(c.Id);
                    }
                }
            }

            List<Guid> busycomm = new List<Guid>();

            // FIXME: Introduce memoization of voter identification for a period of time (i.e. 30min) to avoid mischievous behavior
            _manager.CollectExpiredItems();
            busycomm = _manager.GetBusyCommissioners();

            // 30 out of 100 are sent to some commissioner. 70 if there is an affine commissioner will be preferred
            // Disabled for student general elections
            if (false && _manager.Rnd.Next(100) > 30)
            {
                var affineCommissioners = await (from ar in _context.IdentificationCommissionerAffinityRel
                                                 where elections.Contains(ar.ElectionFk)
                                                 select ar.PersonFk).Distinct().ToListAsync();

                var affineIds = await (from psc in _context.PollingStationCommissioner
                                       where !busycomm.Contains(psc.PersonFk) && comm.Contains(psc.PollingStationCommissionFk) && affineCommissioners.Contains(psc.PersonFk) && psc.AvailableForRemoteRecognition
                                       select new { Id = psc.Id, PersonFk = psc.PersonFk, VirtualRoom = psc.VirtualRoom }).Distinct().ToListAsync();

                var affineRids = await (from psc in _context.RemoteIdentificationCommissioner
                                        where !busycomm.Contains(psc.PersonFk) && comm.Contains(psc.PollingStationCommissionFk) && affineCommissioners.Contains(psc.PersonFk) && psc.AvailableForRemoteRecognition
                                        select new { Id = psc.Id, PersonFk = psc.PersonFk, VirtualRoom = psc.VirtualRoom }).Distinct().ToListAsync();

                affineIds.AddRange(affineRids);

                if (affineIds.Count > 0)
                {
                    var selecteda = affineIds[_manager.Rnd.Next(affineIds.Count)];
                    _manager.AddBusyCommissioner(selecteda.PersonFk);
                    return Redirect(selectVirtualRoom(selecteda.VirtualRoom));
                }
            }

            var ids = await (from psc in _context.PollingStationCommissioner
                              where !busycomm.Contains(psc.PersonFk) && comm.Contains(psc.PollingStationCommissionFk) && psc.AvailableForRemoteRecognition
                              select new { Id = psc.Id, PersonFk = psc.PersonFk, VirtualRoom = psc.VirtualRoom }).Distinct().ToListAsync();
            var rids = await (from psc in _context.RemoteIdentificationCommissioner
                              where !busycomm.Contains(psc.PersonFk) && comm.Contains(psc.PollingStationCommissionFk) && psc.AvailableForRemoteRecognition
                              select new { Id = psc.Id, PersonFk = psc.PersonFk, VirtualRoom = psc.VirtualRoom }).Distinct().ToListAsync();

            ids.AddRange(rids);

            if (ids.Count > 0)
            {
                var selected = ids[_manager.Rnd.Next(ids.Count)];
                _manager.AddBusyCommissioner(selected.PersonFk);

                return Redirect(selectVirtualRoom(selected.VirtualRoom));
            } else
            {
                if (idtype == IdentificationType.Sampling)
                {
                    var otp = OTPSender.GenerateOTP();

                    var votes = await _context.Voter.Where(v => elections.Contains(v.ElectionFk) && v.PersonFk == person.Id).Include(v => v.RecognitionFkNavigation).ToListAsync();

                    foreach (var v in votes)
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

                        recognition.Idtype = "_SampledIdentification";
                        recognition.UserId = userid;
                        recognition.AccountProvider = defaultProvider;
                        recognition.Otp = otp;
                        recognition.State = 0;
                        recognition.Validity = DateTime.Now + TimeSpan.FromMinutes(30);
                        recognition.RemoteIdentification = true;
                    }

                    await _context.SaveChangesAsync();

                    try
                    {
                        OTPSender.SendMail(Configuration, otp, userid);
                        ViewData["OTPResult"] = "Success";
                    }
                    catch
                    {
                        ViewData["OTPResult"] = "Error";
                    }

                    return View("CheckForOTP");
                } else
                {
                    return View("WaitForCommissioner");
                }
            }
        }

        [AuthorizeRoles(EligereRoles.AuthenticatedPerson)]
        public async Task<IActionResult> Index()
        {
            var pfk = EligereRoles.PersonFK(this.User); // If AuthenticatedPerson person should be populated
            var person = await _context.Person.FindAsync(pfk);

            return View((ReadElectionConf(), person, await GetElections(person), isDesktopOS(Request.Headers["User-Agent"])));
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
            var pfk = EligereRoles.PersonFK(this.User); // Voter role implies AuthenticatedPerson and thus pfk should be populated
            var person = await _context.Person.FindAsync(pfk);

            var votes = await GetElections(person);

            var availableVotes = votes.Where(v => (v.Item2.Active || (EligereRoles.Provider(this.User,defaultProvider) == "Spid" && v.Item2.PollStartDate < DateTime.Now && v.Item2.PollEndDate > DateTime.Now)) && !v.Item4 && !v.Item5.HasValue).Select(v => v.Item1.Id).ToList();

            if (availableVotes.Count == 0)
                return Forbid("No ballot to cast");

            var now = DateTime.Now;

            // If strong Authentication recognition is performed automatically
            if (EligereRoles.Provider(this.User, defaultProvider) == "Spid" && votes.Where(v => ElectionConfiguration.FromJson(v.Item2.Configuration).IdentificationType == IdentificationType.IndividualAndSPID).Any())
            {
                var recognitions = await (from v in _context.Voter join r in _context.Recognition on v.RecognitionFk equals r.Id where availableVotes.Contains(v.Id) && !v.Vote.HasValue select r).ToListAsync();
                var ckrecognition = await (from v in _context.Voter where availableVotes.Contains(v.Id) && !v.Vote.HasValue && !v.RecognitionFk.HasValue select v).ToArrayAsync();
                foreach (var v in ckrecognition)
                {
                    var recognition = new Recognition()
                    {
                        Id = Guid.NewGuid(),
                    };
                    v.RecognitionFk = recognition.Id;
                    _context.Recognition.Add(recognition);
                    recognitions.Add(recognition);
                }
                foreach (var recognition in recognitions)
                {
                    recognition.Idtype = "SpidAuth";
                    recognition.UserId = EligereRoles.UserId(this.User);
                    recognition.AccountProvider = "Spid";
                    recognition.Otp = otp;
                    recognition.State = 0;
                    recognition.Validity = DateTime.Now + TimeSpan.FromMinutes(30);
                }
                await _context.SaveChangesAsync();
            }

            var ckvotes = await (from v in _context.Voter
                                 join r in _context.Recognition on v.RecognitionFk equals r.Id
                                 join e in _context.Election on v.ElectionFk equals e.Id
                                 where availableVotes.Contains(v.Id) && !v.Vote.HasValue // ensure that the vote has not been casted
                                 select new { Voter = v, Recognition = r, Election = e }).ToListAsync();

            if (ckvotes.Count() == 0)
                return Forbid("OTP non valida");

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
