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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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
        private string contentRootPath;

        public HomeController(ILogger<HomeController> logger, ESDB context, IWebHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            contentRootPath = env.ContentRootPath;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var dir = new DirectoryInfo(Path.Combine(contentRootPath, "Data/EVSKey/"));
            if (dir.GetFiles("*.xml").Length == 0)
            {
                return RedirectToAction("Index", "Setup");
            }

            var u = EligereRoles.UserId(User);
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

            var u = EligereRoles.UserId(User);

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
                UserId = u,
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
