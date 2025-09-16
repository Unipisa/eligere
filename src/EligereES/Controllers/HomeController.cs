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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Cryptography;

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
            if (User.Identity.IsAuthenticated)
            {
                _logger.LogWarning($"User {User.Identity.Name} logged from {Request.HttpContext.Connection.RemoteIpAddress?.ToString()}");
            }

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
            ViewData["SpidEnabled"] = _config.GetValue<bool?>("Spid:Enabled") ?? false;
            ViewData["SAML2Enabled"] = _config.GetValue<bool?>("SAML2:Enabled") ?? false;
            return View();
        }

        public IActionResult SAMLLogin()
        {

            /// sure?

            string name = User.Claims.Where(c => c.Type == "Name").First().Value;
            string familyName = User.Claims.Where(c => c.Type == "familyName").First().Value;
            string fiscalNumber = User.Claims.Where(c => c.Type == "fiscalNumber").First().Value;
            string dateOfBirth = User.Claims.Where(c => c.Type == "dateOfBirth").First().Value;

            User.Identities.FirstOrDefault().AddClaim(new Claim(ClaimTypes.GivenName, name, ClaimValueTypes.String, "SAML2"));
            User.Identities.FirstOrDefault().AddClaim(new Claim(ClaimTypes.Surname, familyName, ClaimValueTypes.String, "SAML2"));
            User.Identities.FirstOrDefault().AddClaim(new Claim(ClaimTypes.Name, $"{name} {familyName}", ClaimValueTypes.String, "SAML2"));
            User.Identities.FirstOrDefault().AddClaim(new Claim(ClaimTypes.NameIdentifier, fiscalNumber, ClaimValueTypes.String, "SAML2"));
            User.Identities.FirstOrDefault().AddClaim(new Claim(ClaimTypes.Email, "", ClaimValueTypes.String, "SAML2"));
            User.Identities.FirstOrDefault().AddClaim(new Claim(ClaimTypes.AuthorizationDecision, "Verify", ClaimValueTypes.String, "SAML2"));

            return RedirectToAction("Index", "Home");
            /*
             * debug only: inspect all claims returned by the IdP
             * 
            IDictionary<string, string?> Attributes = new Dictionary<string, string?>();
            foreach (var c in User.Claims)
            {
                Attributes[c.Type] = c.Value;
            }
            return View("SAMLLogin", Attributes);
            */
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

        [AllowAnonymous]
        public IActionResult ExternalLogin()
        {
            return Challenge(new AuthenticationProperties() { RedirectUri=Url.Action("Index", "Home") },  "Spid");
        }

        [Route("spid-logout")]
        public async Task<IActionResult> SpidLogout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
