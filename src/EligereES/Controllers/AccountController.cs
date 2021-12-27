using EligereES.Models;
using EligereES.Models.DB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EligereES.Controllers
{

    [AllowAnonymous, Route("Account")]
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AccountController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _config = configuration;
            _webHostEnvironment = env;
        }

        [Route("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [Route("google-logout")]
        public async Task<IActionResult> GoogleLogout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var opt = new DbContextOptionsBuilder<ESDB>();
            using (var esdb = new ESDB(opt.UseSqlServer(_config.GetConnectionString("ESDB")).Options))
            {
                var roles = await EligereRoles.ComputeRoles(esdb, "AzureAD", result.Principal.Identity.Name);
                var lclaims = new List<Claim>();
                roles.ForEach(r => lclaims.Add(new Claim(ClaimTypes.Role, r)));
                var appIdentity = new ClaimsIdentity(lclaims, "EligereIdentity");
                result.Principal.AddIdentity(appIdentity);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
