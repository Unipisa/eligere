using EligereES.Models.DB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EligereES.Models
{
    public class RegistryVoter
    {
        public string PublicID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Election { get; set; }
    }
    public static class ElectionMgmt
    {
        public static IQueryable<RegistryVoter> GetVoters(Guid electionFK, ESDB esdb)
        {
            var q = from ev in esdb.Voter
                    join p in esdb.Person on ev.PersonFk equals p.Id
                    join e in esdb.Election on ev.ElectionFk equals e.Id    
                    where ev.ElectionFk == electionFK
                    orderby p.LastName, p.FirstName
                    select new RegistryVoter
                    {
                        PublicID = p.PublicId,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        Election = e.Description
                    };
            return q;
        }
    }


}
