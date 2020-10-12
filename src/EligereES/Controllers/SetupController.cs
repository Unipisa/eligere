using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EligereES.Models.DB;
using EligereES.Models.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using EligereES.Models;
using Microsoft.AspNetCore.Http;
using EligereES.Models.Client;
using System.IO;

namespace EligereES.Controllers
{
    [Authorize]
    public class SetupController : Controller
    {
        private readonly ESDB _context;
        private string contentRootPath;

        public SetupController(ESDB ctxt, IWebHostEnvironment env)
        {
            _context = ctxt;
            contentRootPath = env.ContentRootPath;
        }

        public static ESConfiguration GetESConfiguration(string contentRootPath)
        {
            var confAPI = new ESConfiguration();
            var fn = Path.Combine(contentRootPath, "Data/apiconf.js");

            if (System.IO.File.Exists(fn))
            {
                confAPI = ESConfiguration.FromJson(System.IO.File.ReadAllText(fn));
            }
            return confAPI;
        }

        [AuthorizeRoles(EligereRoles.Admin)]
        public IActionResult StartDailyElections()
        {
            var e = _context.Election.ToList();
            var today = DateTime.Today;
            foreach (var el in e)
            {
                if (el.PollStartDate - TimeSpan.FromDays(1) <= today && el.PollEndDate >= today)
                {
                    el.Active = true;
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // This should be reviewed as a grant
        [AuthorizeRoles(EligereRoles.Admin)]
        public IActionResult Index()
        {
            return View(GetESConfiguration(contentRootPath));
        }

        [AuthorizeRoles(EligereRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveAPI(string eligerevsapi)
        {
            var confAPI = new ESConfiguration()
            {
                VotingSystemTicketAPI = eligerevsapi != null && eligerevsapi.Trim() == String.Empty ? null : eligerevsapi,
            };

            System.IO.File.WriteAllText(Path.Combine(contentRootPath, "Data/apiconf.js"), confAPI.ToJson());

            return RedirectToAction("Index");
        }

        [AuthorizeRoles(EligereRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UploadEVSKey(List<IFormFile> file)
        {
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


    }
}
