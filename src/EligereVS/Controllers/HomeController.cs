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

        private readonly ILogger<HomeController> _logger;
        private string contentRootPath;
        private RocksDb _conf;
        private RocksDb secureBallot;

        private IDataProtectionProvider dataProtector;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env, PersistentStores stores, IDataProtectionProvider provider)
        {
            _logger = logger;
            contentRootPath = env.ContentRootPath;
            stores.SetContentRootPath(env.ContentRootPath);
            _conf = stores.Configuration;
            secureBallot = stores.SecureBallot;
            dataProtector = provider;
        }

        public IActionResult Index()
        {
            if (!HttpContext.Session.Keys.Contains(IsAuthenticated))
                return RedirectToAction("Signin");

            var electionDataAvailable = false;

            var confAPI = new VotingSystemConfiguration();
            lock (_conf)
            {
                electionDataAvailable = _conf.Get(ESElectionConfigurationKey) != null;

                var v = _conf.Get(APIConfigurationKey);
                if (v != null)
                {
                    confAPI = VotingSystemConfiguration.FromJson(v);
                }
            }

            ViewData["ESElectionConfigured"] = electionDataAvailable;
            ViewData["ESApiEndPointConfigured"] = confAPI.ElectionSystemAPI != null;

            return View(confAPI);
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

        public IActionResult GuardianKeyGen()
        {
            if (!HttpContext.Session.Keys.Contains(IsAuthenticated))
                return RedirectToAction("Signin");
            //var guardianApi = new GuardianClient("http://localhost:8001");
            //var mediatorApi = new MediatorClient("http://localhost:8000");
            var keys = GenKeys();
            var fnames = new List<string>();

            for (var i = 0; i < keys.Count; i++)
            {
                var fn = Path.Combine(contentRootPath, $"wwwroot/temp/key-{i}.txt");
                System.IO.File.WriteAllText(fn, keys[i]);
                fnames.Add(Url.Content("~") + $"temp/key-{i}.txt");
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
                System.IO.File.Delete(fn);
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

            List<ElectionDescription> eldesc;
            lock (_conf)
            {
                if (_conf.Get(ESElectionConfigurationKey) == null)
                        throw new Exception("ESConfiguration missing");
                eldesc = JsonSerializer.Deserialize<List<ElectionDescription>>(_conf.Get(ESElectionConfigurationKey));
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
            var elections = eldesc.ToDictionary(g => g.ElectionId);

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
            var eldesc = JsonSerializer.Deserialize<List<ElectionDescription>>(data);

            lock(_conf)
            {
                _conf.Put(ESElectionConfigurationKey, JsonSerializer.Serialize<List<ElectionDescription>>(eldesc));
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
                var eldesc = JsonSerializer.Deserialize<List<ElectionDescription>>(_conf.Get(ESElectionConfigurationKey)); 

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
        public IActionResult UploadEVSKey(List<IFormFile> file)
        {
            if (!HttpContext.Session.Keys.Contains(IsAuthenticated))
                return RedirectToAction("Signin");

            if (file.Count != 1)
            {
                throw new Exception("Invalid number of uploaded files");
            }

            var dir = new DirectoryInfo(Path.Combine(contentRootPath, "Data/EVSKey/"));
            foreach (var f in dir.GetFiles())
                f.Delete();

            var fn = Path.Combine(contentRootPath, "Data/EVSKey/" + file[0].FileName);
            var k = new StreamReader(file[0].OpenReadStream()).ReadToEnd();
            System.IO.File.WriteAllText(fn, k);

            return RedirectToAction("Index");
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
