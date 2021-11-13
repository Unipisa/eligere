using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EligereVS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RocksDbSharp;

namespace EligereVS.Controllers
{
    [AllowAnonymous]
    public class VoteController : Controller
    {
        private RocksDb ticketsDb;
        private RocksDb configuration;
        private RocksDb secureBallot;
        private IDataProtectionProvider dataprotection;
        private string contentRoot;
        private TicketsQueue ticketSender;

        public VoteController(IWebHostEnvironment env, PersistentStores stores, IDataProtectionProvider provider, TicketsQueue tickets)
        {
            contentRoot = env.ContentRootPath;
            stores.SetContentRootPath(env.ContentRootPath);
            ticketsDb = stores.Tickets;
            configuration = stores.Configuration;
            secureBallot = stores.SecureBallot;
            dataprotection = provider;
            ticketSender = tickets;
        }

        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult ElectionCard(string tickets)
        {
            var encTickets = Convert.FromBase64String(tickets);
            var protector = dataprotection.CreateProtector("EVSKeyExchange");
            var plainSerTickets = Encoding.UTF8.GetString(protector.Unprotect(encTickets));
            var ticketList = JsonSerializer.Deserialize<List<VoteTicket>>(plainSerTickets);

            string d;
            lock (configuration)
            {
                d = configuration.Get(HomeController.ESElectionConfigurationKey);
            }

            var electionDescription = JsonSerializer.Deserialize<ElectionGuard.ElectionDescription>(d);
            var electionmap = electionDescription.contests.ToDictionary(v => v.object_id);
            var elections2Send = new List<string>();
            var usedTickets = new List<VoteTicket>();
            var availableTickets = new List<VoteTicket>();

            // Check used tickets
            foreach (var ticket in ticketList)
            {
                if (!electionmap.ContainsKey(ticket.ElectionId))
                    throw new Exception("Invalid state in ElectionCard generation (ticket referring to non-existing election)");

                elections2Send.Add(ticket.ElectionId);
                if (ticketsDb.Get(ticket.HashId) != null)
                {
                    usedTickets.Add(ticket);
                } else
                {
                    availableTickets.Add(ticket);
                }
            }
            electionDescription.contests = electionDescription.contests.Where(c => elections2Send.Contains(c.object_id)).ToArray();
            var cardData = new VoteInformation()
            {
                ElectionDescription = electionDescription,
                AvailableTickets = availableTickets,
                UsedTickets = usedTickets
            };

            return View((tickets, cardData));
        }

        private void CastVote(VoteTicket ticket, string voteContent)
        {
            var protector = dataprotection.CreateProtector("SecureBallot");
            var secureVote = protector.Protect(voteContent);
            var voteId = Guid.NewGuid().ToString();

            lock (ticketsDb)
            {
                    ticketsDb.Put(ticket.HashId, ticket.ToJson());
            }

            lock (secureBallot)
            {
                var ballot = new BallotContent() { ElectionId = ticket.ElectionId, SecureVote = secureVote };
                secureBallot.Put(voteId, ballot.ToJson());
            }
        }

        [HttpPost]
        public IActionResult CastBallot(string tickets, string election, string ballotType, string preferences)
        {
            var encTickets = Convert.FromBase64String(tickets);
            var protector = dataprotection.CreateProtector("EVSKeyExchange");
            var plainSerTickets = Encoding.UTF8.GetString(protector.Unprotect(encTickets));
            var ticketList = JsonSerializer.Deserialize<List<VoteTicket>>(plainSerTickets);

            string d;

            lock (secureBallot)
            {
                if (secureBallot.Get(HomeController.VotingForTallyClosedKey) != null)
                    return NotFound("Election Voting System is closed after tally");
            }

            lock (configuration)
            {
                d = configuration.Get(HomeController.ESElectionConfigurationKey);
            }

            var confAPI = new VotingSystemConfiguration();
            lock (configuration)
            {
                var v = configuration.Get(HomeController.APIConfigurationKey);
                if (v != null)
                {
                    confAPI = VotingSystemConfiguration.FromJson(v);
                }
            }

            var electionDescription = JsonSerializer.Deserialize<ElectionGuard.ElectionDescription>(d);
            var contests = electionDescription.contests.ToDictionary(v => v.object_id);

            if (ticketList.Where(t => t.ElectionId == election).Count() > 1)
            {
                return Json(new CastBallotResult()
                {
                    Status = 500,
                    Message = "Internal error: more tickets for a single election"
                });
            }

            var ticket = ticketList.Where(t => t.ElectionId == election).FirstOrDefault();
            if (ticket == null)
            {
                return Json(new CastBallotResult()
                {
                    Status = 403,
                    Message = "No ticket for election"
                });
            }

            // Ensure atomicity locking ticketsDB
            lock (ticketsDb)
            {
                if (ticketsDb.Get(ticket.HashId) != null)
                    return Json(new CastBallotResult()
                    {
                        Status = 403,
                        Message = "Ticket already used"
                    });

                if (contests.ContainsKey(election))
                {
                    var el = contests[election];
                    var candidates = electionDescription.candidates.ToDictionary(v => v.object_id);
                    switch (ballotType)
                    {
                        case "emptyBallot":
                            CastVote(ticket, "$blank$");
                            break;
                        case "spoiledBallot":
                            CastVote(ticket, "$spoil$");
                            break;
                        default:
                            var prefs = JsonSerializer.Deserialize<string[]>(preferences);
                            var partyel = (el.extensions["CandidatesType"] == "Party");

                            if (partyel)
                            {
                                var partycount = 0;
                                foreach (var pref in prefs)
                                {
                                    var cand = el.ballot_selections.Where(c => candidates[c.candidate_id].ballot_name.text[0].value == pref).FirstOrDefault();
                                    if (cand.object_id[0] != '*')
                                    {
                                        partycount++;
                                    }
                                }
                                // Check can be improved
                                if (partycount > 1)
                                    return Json(new CastBallotResult()
                                    {
                                        Status = 403,
                                        Message = "Too many parties selected (only 1 is allowed)"
                                    });
                            }

                            var extracount = partyel ? 1 : 0; // If party election one is for the party
                            if (prefs.Length > el.votes_allowed + extracount)
                            {
                                return Json(new CastBallotResult()
                                {
                                    Status = 403,
                                    Message = $"Too many votes expressed ({prefs.Length}) with respect to the maximum ({el.votes_allowed})"
                                });

                            }
                            foreach (var pref in prefs)
                            {
                                var cand = el.ballot_selections.Where(c => candidates[c.candidate_id].ballot_name.text[0].value == pref).FirstOrDefault();
                                if (cand == null)
                                {
                                    return Json(new CastBallotResult()
                                    {
                                        Status = 403,
                                        Message = "Invalid candidate"
                                    });
                                }
                            }
                            foreach (var pref in prefs)
                            {
                                CastVote(ticket, pref);
                            }
                            break;
                    }

                }
                else
                {
                    return Json(new CastBallotResult()
                    {
                        Status = 500,
                        Message = "Invalid election id"
                    });
                }
            }

            var dp = dataprotection.CreateProtector("EligereMetadataExchange");
            var secretHash = dp.Protect(ticket.HashId);

            // Notification may fail, so we bookkeep failed notifications in a queue and retry over time
            ticketSender.AddTicket(secretHash);
            ticketSender.NotifyTickets(contentRoot, confAPI.ElectionSystemAPI.TrimEnd('/'));

            var ret = new CastBallotResult()
            {
                Status = 200,
                Message = "Vote casted"
            };

            return Json(ret);
        }

        [HttpGet]
        public IActionResult VoteConclusion()
        {
            return View();
        }
    }
}
