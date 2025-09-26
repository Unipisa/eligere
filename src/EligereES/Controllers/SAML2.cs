using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sustainsys.Saml2;
using Sustainsys.Saml2.AspNetCore2;
using Sustainsys.Saml2.Saml2P;

namespace EligereES.Controllers
{
    public class SAML2 : Controller
    {
        public IActionResult Logout()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToAction("Index", "Home");
            }

            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index","Home")
            };

            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            
            return SignOut(properties, CookieAuthenticationDefaults.AuthenticationScheme, Saml2Defaults.Scheme);
        }

        public IActionResult Exit() { 
            return View("Exit",(Url.Action("Index","Home")));
        }
    }
}
