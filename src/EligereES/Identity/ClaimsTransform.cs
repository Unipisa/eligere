using EligereES.Models;
using EligereES.Models.DB;
using ITfoxtec.Identity.Saml2.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

// From ITfoxtec WebCore example
namespace EligereES.Saml.Identity
{
    public static class ClaimsTransform
    {
        public static ClaimsPrincipal Transform(IConfiguration conf, ClaimsPrincipal incomingPrincipal)
        {
            if (!incomingPrincipal.Identity.IsAuthenticated)
            {
                return incomingPrincipal;
            }

            return CreateClaimsPrincipal(conf, incomingPrincipal);
        }

        private static ClaimsPrincipal CreateClaimsPrincipal(IConfiguration conf, ClaimsPrincipal incomingPrincipal)
        {
            var claims = new List<Claim>();

            // All claims
            claims.AddRange(incomingPrincipal.Claims);
            var opt = new DbContextOptionsBuilder<ESDB>();

            using (var esdb = new ESDB(opt.UseSqlServer(conf.GetConnectionString("ESDB")).Options))
            {
                // Associate application roles
                var rw = EligereRoles.ComputeRoles(esdb, "SAML2", incomingPrincipal.Identity.Name);
                rw.Wait();
                var roles = rw.Result;
                roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
                var appIdentity = new ClaimsIdentity(claims, "EligereIdentity");
                incomingPrincipal.AddIdentity(appIdentity);

                // Trust the PublicID from the provider to associate UserLogin information
                var cf = claims.Where(c => c.Type == "codice_fiscale").First().Value;
                var p = esdb.Person.Where(p => p.PublicId == cf).FirstOrDefault();
                if (p != null)
                {
                    if (!esdb.UserLogin.Where(u => u.UserId == incomingPrincipal.Identity.Name).Any())
                    {
                        var u = new UserLogin()
                        {
                            Id = System.Guid.NewGuid(),
                            PersonFk = p.Id,
                            Provider = "SAML2",
                            UserId = incomingPrincipal.Identity.Name
                        };
                        esdb.UserLogin.Add(u);
                        esdb.SaveChanges();
                    }
                }
            }

            return new ClaimsPrincipal(new ClaimsIdentity(claims, incomingPrincipal.Identity.AuthenticationType, ClaimTypes.NameIdentifier, ClaimTypes.Role)
            {
                BootstrapContext = ((ClaimsIdentity)incomingPrincipal.Identity).BootstrapContext
            });
        }

        private static IEnumerable<Claim> GetSaml2LogoutClaims(ClaimsPrincipal principal)
        {
            yield return GetClaim(principal, Saml2ClaimTypes.NameId);
            yield return GetClaim(principal, Saml2ClaimTypes.NameIdFormat);
            yield return GetClaim(principal, Saml2ClaimTypes.SessionIndex);
        }

        private static Claim GetClaim(ClaimsPrincipal principal, string claimType)
        {
            return ((ClaimsIdentity)principal.Identity).Claims.Where(c => c.Type == claimType).FirstOrDefault();
        }

        private static string GetClaimValue(ClaimsPrincipal principal, string claimType)
        {
            var claim = GetClaim(principal, claimType);
            return claim != null ? claim.Value : null;
        }
    }
}
