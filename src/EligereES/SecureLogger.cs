using EligereES.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EligereES
{
    public class SecureLogger
    {
        // ToDo: Add digital signature, add priority to the entry and notification to guardians
        public static async void Log(ESDB ctxt, Models.DB.Person p, String AccountProvider, String UserId, String text)
        {
            var log = new Models.DB.Log() { 
                Id = Guid.NewGuid(),
                AccountProvider = AccountProvider,
                PersonFk = p.Id,
                UserId = UserId, 
                TimeStamp = DateTime.Now,
                LogEntry = text 
            };
            await ctxt.Log.AddAsync(log);
            await ctxt.SaveChangesAsync();
        }
    }
}
