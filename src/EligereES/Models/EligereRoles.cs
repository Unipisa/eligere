using EligereES.Models.DB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EligereES.Models
{
    public static class EligereRoles
    {
        public const string Admin = "Administrator";
        public const string ElectionOfficer = "Election Officer";
        public const string PollingStationStaff = "Polling Station Member";
        public const string PollingStationPresident = "Polling Station President";
        public const string Candidate = "Candidate";
        public const string Voter = "Voter";
        public const string AuthenticatedUser = "Authenticated User";
        public const string AuthenticatedPerson = "Authenticated Person";
        public const string RemoteIdentificationOfficer = "Remote Identification Officer";

        internal async static Task<List<string>> ComputeRoles(ESDB esdb, string provider, string username)
        {
            var roles = new List<string>();
            roles.Add(AuthenticatedUser);

            var u = from l in esdb.UserLogin where provider == l.Provider && username == l.UserId select l;

            if (await u.CountAsync() == 1) // Should be either 0 or 1
            {
                roles.Add(AuthenticatedPerson);
                var user = u.First();
                var q = from s in esdb.ElectionStaff
                        join r in esdb.ElectionRole on s.ElectionRoleFk equals r.Id
                        where s.PersonFk == user.PersonFk
                        select r.Label;

                await q.ForEachAsync(r => roles.Add(r)); // Assumption: the role in the DB table match constants in this class

                var elections = from e in esdb.Election where e.PollEndDate > DateTime.Today select e;
                var isCandidate = await (from e in elections join bn in esdb.BallotName on e.Id equals bn.ElectionFk join c in esdb.EligibleCandidate on bn.Id equals c.BallotNameFk where c.PersonFk == user.PersonFk select c).AnyAsync();
                var isVoter = await (from e in elections join v in esdb.Voter on e.Id equals v.ElectionFk where v.PersonFk == user.PersonFk select v).AnyAsync();
                var isRemoteIdentificationOfficer = await (from e in elections join psc in esdb.PollingStationCommission on e.Id equals psc.ElectionFk join ro in esdb.RemoteIdentificationCommissioner on psc.Id equals ro.PollingStationCommissionFk where ro.PersonFk == user.PersonFk select ro).AnyAsync();
                var isPresident = await (from e in elections join c in esdb.PollingStationCommission on e.Id equals c.ElectionFk join com in esdb.PollingStationCommissioner on c.PresidentFk equals com.Id where com.PersonFk == user.PersonFk select c).AnyAsync();
                var isMember = await (from e in elections join c in esdb.PollingStationCommission on e.Id equals c.ElectionFk join com in esdb.PollingStationCommissioner on c.Id equals com.PollingStationCommissionFk where com.PersonFk == user.PersonFk select c).AnyAsync();

                if (isCandidate) roles.Add(Candidate);
                if (isPresident) roles.Add(PollingStationPresident);
                if (isVoter) roles.Add(Voter);
                if (isMember) roles.Add(PollingStationStaff);
                if (isRemoteIdentificationOfficer) roles.Add(RemoteIdentificationOfficer);
            }

            return roles;
        }

        internal async static Task<bool> InconsistentRoles(ClaimsPrincipal user, ESDB esdb, string provider, string username)
        {
            var roles = await ComputeRoles(esdb, provider, username);
            foreach (var r in roles)
            {
                if (!user.IsInRole(r)) return true;
            }
            return false;
        }

        internal async static Task<ClaimsPrincipal> UpdateRoles(ClaimsPrincipal user, ESDB esdb, string provider, string username)
        {
            var roles = await ComputeRoles(esdb, provider, username);
            ClaimsIdentity claimsIdentity = user.Identities.Where(c => c.AuthenticationType == "EligereIdentity").Any() ? user.Identities.Where(c => c.AuthenticationType == "EligereIdentity").First() : null;
            var claims = new List<Claim>();
            roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
            if (claimsIdentity != null)
            {
                foreach (var c in claimsIdentity.Claims.ToArray())
                {
                    claimsIdentity.RemoveClaim(c);
                }
                claimsIdentity.AddClaims(claims);
            }
            else
            {
                claimsIdentity = new ClaimsIdentity(claims, "EligereIdentity");
                user.AddIdentity(claimsIdentity);
            }

            return user;
        }

        // Method designed to harmonize the way different auth providers maps the Identity.Name property
        public static string UserId(ClaimsPrincipal principal)
        {
            if (principal == null) return "";

            return principal.Identity.Name;
        }
    }

    public class EligereClaimsTransformation : IClaimsTransformation
    {
        private IConfiguration _configuration;
        private string defaultProvider;

        public EligereClaimsTransformation(IConfiguration configuration)
        {
            _configuration = configuration;
            defaultProvider = configuration.GetValue(typeof(string), "DefaultAuthProvider") as string;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var opt = new DbContextOptionsBuilder<ESDB>();

            using (var esdb = new ESDB(opt.UseSqlServer(_configuration.GetConnectionString("ESDB")).Options))
            {
                await EligereRoles.UpdateRoles(principal, esdb, defaultProvider, EligereRoles.UserId(principal));
            }

            return principal;
        }
    }
}
