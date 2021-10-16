using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EligereVS.Models;
using RocksDbSharp;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebUtilities;

namespace EligereVS.Controllers
{
    public class HomeController : Controller
    {
        public const string IsAuthenticated = "IsSessionAuthenticated";
        public const string MasterPasswordKey = "VSPasswordHash";
        public const string APIConfigurationKey = "VSAPIConfiguration";
        public const string ESElectionConfigurationKey = "ESElectionConfiguration";
        public const string VotingForTallyClosedKey = "FirstTallyExecuted";
        public const string GuardiansKeyGenerated = "GuardiansKeyGenerated";

        private static ElectionGuard.GuardianClient GuardianApi = null;
        private static ElectionGuard.MediatorClient MediatorApi = null;

        private readonly ILogger<HomeController> _logger;
        private string contentRootPath;
        private RocksDb _conf;
        private RocksDb secureBallot;
        private RocksDb egSecureBallot;
        private TicketsQueue ticketQueue;

        private IDataProtectionProvider dataProtector;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env, PersistentStores stores, IDataProtectionProvider provider, TicketsQueue tickets)
        {
            _logger = logger;
            contentRootPath = env.ContentRootPath;
            stores.SetContentRootPath(env.ContentRootPath);
            _conf = stores.Configuration;
            secureBallot = stores.SecureBallot;
            egSecureBallot = stores.EGSecureBallot;
            dataProtector = provider;
            ticketQueue = tickets;

            var confAPI = new VotingSystemConfiguration();
            lock (_conf)
            {
                var v = _conf.Get(APIConfigurationKey);
                if (v != null)
                {
                    confAPI = VotingSystemConfiguration.FromJson(v);
                }
            }
            if (confAPI.GuardianAPI != null && confAPI.MediatorAPI != null) {
                GuardianApi = new ElectionGuard.GuardianClient(confAPI.GuardianAPI);
                MediatorApi = new ElectionGuard.MediatorClient(confAPI.MediatorAPI);
            }
        }

        public IActionResult Index()
        {
            if (!HttpContext.Session.Keys.Contains(IsAuthenticated))
                return RedirectToAction("Signin");

            var electionDataAvailable = false;
            var guardiansKeysGenerated = false;

            var confAPI = new VotingSystemConfiguration();
            lock (_conf)
            {
                electionDataAvailable = _conf.Get(ESElectionConfigurationKey) != null;

                var v = _conf.Get(APIConfigurationKey);
                if (v != null)
                {
                    confAPI = VotingSystemConfiguration.FromJson(v);
                }
                if (_conf.Get(GuardiansKeyGenerated) != null)
                    guardiansKeysGenerated = true;
            }

            ViewData["ESElectionConfigured"] = electionDataAvailable;
            ViewData["ESApiEndPointConfigured"] = confAPI.ElectionSystemAPI != null;
            ViewData["GuardiansKeysGenerated"] = guardiansKeysGenerated;
            ViewData["HangingTickets"] = ticketQueue.Count;

            return View(confAPI);
        }

        public IActionResult SendHangingTickets()
        {
            if (!HttpContext.Session.Keys.Contains(IsAuthenticated))
                return RedirectToAction("Signin");

            var confAPI = new VotingSystemConfiguration();
            lock (_conf)
            {
                var v = _conf.Get(APIConfigurationKey);
                if (v != null)
                {
                    confAPI = VotingSystemConfiguration.FromJson(v);
                }
            }

            ticketQueue.NotifyTickets(contentRootPath, confAPI.ElectionSystemAPI.TrimEnd('/'));

            return RedirectToAction("Index");
        }

        public IActionResult TestEligereESConnection()
        {
            if (!HttpContext.Session.Keys.Contains(IsAuthenticated))
                return RedirectToAction("Signin");

            var confAPI = new VotingSystemConfiguration();
            lock (_conf)
            {
                var v = _conf.Get(APIConfigurationKey);
                if (v != null)
                {
                    confAPI = VotingSystemConfiguration.FromJson(v);
                }
            }

            var challenge = Convert.ToBase64String(SHA256.Create().ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(DateTime.Now.ToString())));
            try
            {
                var urlBuilder = new System.Text.StringBuilder();
                urlBuilder.Append(confAPI.ElectionSystemAPI.TrimEnd('/')).Append("/TestEligereESConnection?test=").Append(challenge);
                var req = WebRequest.Create(urlBuilder.ToString());
                var resp = (HttpWebResponse)req.GetResponse();
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    var text = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                    var dp = dataProtector.CreateProtector("EligereTest");
                    var data = dp.Unprotect(text);
                    if (data == challenge)
                    {
                        ViewData["Result"] = "Test successful";
                    } 
                    else
                    {
                        ViewData["Result"] = "Error when decoding the challenge";
                    }
                }
                else
                {
                    ViewData["Result"] = $"HTTP response status: {resp.StatusCode}";
                }

                resp.Close();
            }
            catch (Exception e)
            {
                ViewData["Result"] = $"Exception when invoking API: {e.ToString()}";
            }

            return View();
        }

        public bool JoinKeys(List<string> keys)
        {
            var d = new DirectoryInfo(Path.Combine(contentRootPath, "Data/EVSKey"));
            if (!d.Exists) throw new Exception("Missing EVSKey directory");
            if (d.GetFiles().Length != 1) throw new Exception("Too many files in EVSKey directory");
            var content = System.IO.File.ReadAllText(d.GetFiles()[0].FullName);

            var protector = dataProtector.CreateProtector("GuardianKeys");
            var decslices = new string[5];
            foreach (var s in keys)
            {
                var cs = protector.Unprotect(s);
                decslices[(int)(cs[0] - '0')] = cs.Substring(1);
            }

            var deccont = String.Join("", decslices);
            return content == deccont;
        }

        public List<string> GenKeys()
        {
            var d = new DirectoryInfo(Path.Combine(contentRootPath, "Data/EVSKey"));
            if (!d.Exists) throw new Exception("Missing EVSKey directory");
            if (d.GetFiles().Length != 1) throw new Exception("Too many files in EVSKey directory");
            var content = System.IO.File.ReadAllText(d.GetFiles()[0].FullName);
            var stride = content.Length / 5;
            var protector = dataProtector.CreateProtector("GuardianKeys");
            var slices = new List<string>();
            for (var i = 0; i < 4; i++)
            {
                var s = protector.Protect(i.ToString() + content.Substring(i * stride, stride));
                slices.Add(s);
            }
            slices.Add(protector.Protect("4" + content.Substring(4 * stride)));
            return slices;
        }

        public string[] ElectionGuardGenGuardians(int numGuardians, int quorum)
        {
            // ElectionGuard generation
            ElectionGuard.ElectionDescription eldesc;
            lock (_conf)
            {
                if (_conf.Get(ESElectionConfigurationKey) == null)
                    throw new Exception("ESConfiguration missing");
                eldesc = JsonSerializer.Deserialize<ElectionGuard.ElectionDescription>(_conf.Get(ESElectionConfigurationKey));
            }

            var guardianIds = new string[numGuardians];
            for (var i = 0; i < guardianIds.Length; i++) guardianIds[i] = $"Guardian-{i}";
            var guardians = new List<ElectionGuard.Guardian>();
            for (var i = 0; i < numGuardians; i++)
                guardians.Add(GuardianApi.Guardian(null, null, guardianIds[i], numGuardians, quorum, i));

            var guardianPubKeys = guardians.ConvertAll(g => g.election_key_pair.public_key).ToArray();
            var electionJointKey = MediatorApi.ElectionCombine(guardianPubKeys);
            var electionContext = MediatorApi.ElectionContext(eldesc, electionJointKey.joint_key, numGuardians, quorum);

            // Persists only the public part
            lock(egSecureBallot)
            {
                egSecureBallot.Put("GuardianPubKeys", JsonSerializer.Serialize(guardianPubKeys));
                egSecureBallot.Put("ElectionJointKey", JsonSerializer.Serialize(electionJointKey));
                egSecureBallot.Put("ElectionContext", JsonSerializer.Serialize(electionContext));
                egSecureBallot.Put("Nonce", "110191403412906482859082647039385408757148325819889522238592336039604240167009");
                egSecureBallot.Put("SeedHash", "110191403412906482859082647039385908787142225838889522238592336039604240167009");
            }

            return guardians.ConvertAll(g => JsonSerializer.Serialize(g)).ToArray();
        }

        public IActionResult GuardianKeyGen()
        {
            if (!HttpContext.Session.Keys.Contains(IsAuthenticated))
                return RedirectToAction("Signin");

            lock(_conf)
            {
                if (_conf.Get(GuardiansKeyGenerated) != null)
                    return RedirectToAction("Index");
            }
            //var guardiansKeys = ElectionGuardGenGuardians(5, 3);

            var keys = GenKeys();
            var fnames = new List<string>();

            for (var i = 0; i < keys.Count; i++)
            {
                var fn = Path.Combine(contentRootPath, $"wwwroot/temp/key-{i}.txt");
                //var gfn = Path.Combine(contentRootPath, $"wwwroot/temp/gkey-{i}.txt");
                System.IO.File.WriteAllText(fn, keys[i]);
                //System.IO.File.WriteAllText(gfn, guardiansKeys[i]);
                fnames.Add(Url.Content("~") + $"../temp/key-{i}.txt");
                //fnames.Add(Url.Content("~") + $"../temp/gkey-{i}.txt");
            }

            lock (_conf)
            {
                _conf.Put(GuardiansKeyGenerated, DateTime.Now.ToString());
            }

            return View(fnames);
        }

        public IActionResult ClearKeys()
        {
            if (!HttpContext.Session.Keys.Contains(IsAuthenticated))
                return RedirectToAction("Signin");
            for (var i = 0; i < 5; i++)
            {
                var fn = Path.Combine(contentRootPath, $"wwwroot/temp/key-{i}.txt");
                //var gfn = Path.Combine(contentRootPath, $"wwwroot/temp/gkey-{i}.txt");
                System.IO.File.Delete(fn);
                //System.IO.File.Delete(gfn);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Tally(string msg)
        {
            return View("KeyCerimony");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Tally(List<IFormFile> files)
        {
            if (!HttpContext.Session.Keys.Contains(IsAuthenticated))
                return RedirectToAction("Signin");

            if (files.Count != 5)
                return RedirectToAction("Tally", new { msg = "Missing keys" });

            var kk = new List<string>();
            foreach (var f in files)
            {
                kk.Add((new StreamReader(f.OpenReadStream())).ReadToEnd());
            }

            if (!JoinKeys(kk))
                return RedirectToAction("Tally", new { msg = "Fail to obtain master key" });

            ElectionGuard.ElectionDescription eldesc;
            lock (_conf)
            {
                if (_conf.Get(ESElectionConfigurationKey) == null)
                        throw new Exception("ESConfiguration missing");
                eldesc = JsonSerializer.Deserialize<ElectionGuard.ElectionDescription>(_conf.Get(ESElectionConfigurationKey));
            }

            var protector = dataProtector.CreateProtector("SecureBallot");
            var ballots = new List<BallotContent>();
            lock (secureBallot)
            {
                if (secureBallot.Get(VotingForTallyClosedKey) == null)
                {
                    secureBallot.Put(VotingForTallyClosedKey, DateTime.Now.ToString());
                }
                using (var it = secureBallot.NewIterator())
                {
                    it.SeekToFirst();
                    while (it.Valid())
                    {
                        // This is the only exception and it is the seal of the Ballot
                        if (it.StringKey() != HomeController.VotingForTallyClosedKey)
                          ballots.Add(BallotContent.FromJson(it.StringValue()));
                        it.Next();
                    }
                }
            }

            var clearBallots = ballots.ConvertAll(b => (b.ElectionId, protector.Unprotect(b.SecureVote)));

            var result = new Dictionary<string, Dictionary<string, int>>();
            foreach (var ballot in clearBallots.GroupBy(b => b.ElectionId))
            {
                var count = ballot.GroupBy(b => b.Item2);
                foreach (var c in count)
                {
                    if (!result.ContainsKey(ballot.Key))
                        result.Add(ballot.Key, new Dictionary<string, int>());

                    result[ballot.Key].Add(c.Key, c.Count());
                }
            }
            var elections = eldesc.contests.ToDictionary(g => g.object_id);

            return View((result, elections));
        }

        public IActionResult LoadESElectionConfiguration()
        {
            if (!HttpContext.Session.Keys.Contains(IsAuthenticated))
                return RedirectToAction("Signin");

            var confAPI = new VotingSystemConfiguration();
            lock (_conf)
            {
                if (_conf.Get(ESElectionConfigurationKey) != null)
                    return new ForbidResult();

                var v = _conf.Get(APIConfigurationKey);
                if (v != null)
                {
                    confAPI = VotingSystemConfiguration.FromJson(v);
                }
            }

            var urlBuilder = new System.Text.StringBuilder();
            urlBuilder.Append(confAPI.ElectionSystemAPI.TrimEnd('/')).Append("/RunningElections");
            var req = WebRequest.Create(urlBuilder.ToString());

            var resp = req.GetResponse();
            var pdata = (new StreamReader(resp.GetResponseStream())).ReadToEnd();
            var dp = dataProtector.CreateProtector("EligereMetadataExchange");
            var data = dp.Unprotect(pdata);
            var fn = Path.Combine(contentRootPath, $"wwwroot/temp/log.txt");
            System.IO.File.WriteAllText(fn, data);
            var eldesc = JsonSerializer.Deserialize<ElectionGuard.ElectionDescription>(data);

            lock(_conf)
            {
                _conf.Put(ESElectionConfigurationKey, JsonSerializer.Serialize<ElectionGuard.ElectionDescription>(eldesc));
            }

            return View("ShowESElectionConfiguration", eldesc);
        }

        public IActionResult ShowESElectionConfiguration()
        {
            if (!HttpContext.Session.Keys.Contains(IsAuthenticated))
                return RedirectToAction("Signin");

            var confAPI = new VotingSystemConfiguration();
            lock (_conf)
            {
                if (_conf.Get(ESElectionConfigurationKey) == null)
                    throw new Exception("ESConfiguration missing");
                var eldesc = JsonSerializer.Deserialize<ElectionGuard.ElectionDescription>(_conf.Get(ESElectionConfigurationKey)); 

                return View(eldesc);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveAPI(string eligereesapi, string mediatorapi, string guardianapi)
        {
            if (!HttpContext.Session.Keys.Contains(IsAuthenticated))
                return RedirectToAction("Signin");

            var confAPI = new VotingSystemConfiguration()
            {
                ElectionSystemAPI = eligereesapi != null && eligereesapi.Trim() == String.Empty ? null : eligereesapi,
                MediatorAPI = mediatorapi != null && mediatorapi.Trim() == String.Empty ? null : mediatorapi,
                GuardianAPI = guardianapi != null && guardianapi.Trim() == String.Empty ? null : guardianapi
            };

            lock (_conf) {                
                _conf.Put(APIConfigurationKey, confAPI.ToJson());
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DownloadEVSKey(string otp)
        {
            if (!HttpContext.Session.Keys.Contains(IsAuthenticated))
                return RedirectToAction("Signin");

            var confAPI = new VotingSystemConfiguration();
            lock (_conf)
            {
                if (_conf.Get(ESElectionConfigurationKey) != null)
                    return new ForbidResult();

                var v = _conf.Get(APIConfigurationKey);
                if (v != null)
                {
                    confAPI = VotingSystemConfiguration.FromJson(v);
                }
            }

            var urlBuilder = new System.Text.StringBuilder();
            urlBuilder.Append(confAPI.ElectionSystemAPI.TrimEnd('/')).Append($"/GetEncryptionKey?otp={otp}");
            var req = WebRequest.Create(urlBuilder.ToString());

            var resp = req.GetResponse();
            var pdata = (new StreamReader(resp.GetResponseStream())).ReadToEnd();
            if (pdata == "KO")
            {
                ViewData["Msg"] = "Failed to get key from EligereES";
                return View();
            }

            var dir = new DirectoryInfo(Path.Combine(contentRootPath, "Data/EVSKey/"));
            foreach (var f in dir.GetFiles())
                f.Delete();

            var data = pdata.Split("::");
            System.IO.File.WriteAllText(Path.Combine(contentRootPath, $"Data/EVSKey/{data[0]}"), data[1]);

            ViewData["Msg"] = "Successfully saved the EligereES Key";

            return View();
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

        [HttpGet]
        public IActionResult Signin()
        {
            var electionDataAvailable = false;

            lock (_conf)
            {
                electionDataAvailable = _conf.Get(ESElectionConfigurationKey) != null;

                var hash = _conf.Get(MasterPasswordKey);
                if (hash == null)
                {
                    return RedirectToAction("SetupMasterPassword");
                }
            }
            ViewData["ESElectionConfigured"] = electionDataAvailable;
            return View();
        }

        [HttpPost]
        public IActionResult Signin(string masterpassword)
        {
            lock (_conf)
            {
                var hash = _conf.Get(MasterPasswordKey);
                if (hash == null)
                {
                    return RedirectToAction("SetupMasterPassword");
                }
                if (hash == ComputeSha256Hash(masterpassword))
                {
                    HttpContext.Session.SetInt32(IsAuthenticated, 1);
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Signin");
        }

        [HttpGet]
        public IActionResult SetupMasterPassword()
        {
            lock (_conf)
            {
                if (_conf.Get(MasterPasswordKey) != null)
                {
                    return RedirectToAction("Signin");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult SetupCheckList()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetupMasterPassword(string masterpassword)
        {
            lock (_conf)
            {
                if (_conf.Get(MasterPasswordKey) != null)
                {
                    return RedirectToAction("Signin");
                }
                _conf.Put(MasterPasswordKey, ComputeSha256Hash(masterpassword));
            }
            return RedirectToAction("Signin");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
