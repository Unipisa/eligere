using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EligereES.Models;
using EligereES.Models.DB;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace EligereES.Controllers
{
    public enum UserLoginRequestResult
    {
        Ok,
        MissingPerson,
        AssociationAlreadyRequested,
        LoginAlreadyAssociated
    }

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ESDB _context;
        private readonly IConfiguration _config;
        private string contentRootPath;
        private string defaultProvider;

        public HomeController(ILogger<HomeController> logger, ESDB context, IWebHostEnvironment env, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _config = configuration;
            contentRootPath = env.ContentRootPath;
            defaultProvider = _config.GetValue(typeof(string), "DefaultAuthProvider") as string;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var dir = new DirectoryInfo(Path.Combine(contentRootPath, "Data/EVSKey/"));
            if (dir.GetFiles("*.xml").Length == 0)
            {
                return RedirectToAction("Index", "Setup");
            }

            // FIXME: this should be improved, roles are computed upon login so if they are changed during execution should be recomputed
            // in particular when assigned roles are revoked currently login should be forced through server restart.
            // This check is only to ensure that enrolled voters being still acknowledged.
            if (User.Identity.IsAuthenticated && await EligereRoles.InconsistentRoles(User, _context,  defaultProvider, User.Identity.Name))
            {
                return RedirectToAction("SignOut", "Account", new { Area = "MicrosoftIdentity" });
            }
            var u = User.Identity.Name;
            var pendingUserLoginRequest = false;
            if (User.IsInRole(EligereRoles.AuthenticatedUser) && !User.IsInRole(EligereRoles.AuthenticatedPerson))
            {
                if (_context.UserLoginRequest.Where(l => l.UserId == u).Count() > 0)
                    pendingUserLoginRequest = true;
            }
            ViewData["PendingUserLoginRequest"] = pendingUserLoginRequest;
            return View();
        }

        [AllowAnonymous]
        public IActionResult PollingStation()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [AuthorizeRoles(EligereRoles.AuthenticatedUser, EligereRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestUserLoginAssociation(string cf)
        {
            cf = cf.Trim(' ', '\t').ToUpperInvariant();

            var u = User.Identity.Name;

            var req = _context.UserLoginRequest.Where(l => l.UserId == cf).FirstOrDefault();

            if (req != null)
                return View(UserLoginRequestResult.AssociationAlreadyRequested);

            var person = _context.Person.Where(p => p.PublicId == cf).FirstOrDefault();

            if (person == null)
                return View(UserLoginRequestResult.MissingPerson);

            var userlogin = _context.UserLogin.Where(l => l.UserId == u).FirstOrDefault();

            if (userlogin != null)
                return View(UserLoginRequestResult.LoginAlreadyAssociated);

            var r = new UserLoginRequest()
            {
                Id = System.Guid.NewGuid(),
                Provider = defaultProvider,
                UserId = User.Identity.Name,
                PersonFk = person.Id
            };
            _context.UserLoginRequest.Add(r);
            await _context.SaveChangesAsync();

            return View(UserLoginRequestResult.Ok);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
