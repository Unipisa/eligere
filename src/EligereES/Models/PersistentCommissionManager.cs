using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EligereES.Models
{
    public class PersistentCommissionManager
    {
        private Random rnd = new Random();
        private List<(Guid, DateTime)> BusyRemoteCommissioners = new List<(Guid, DateTime)>();

        public TimeSpan Expiration { get; set; }

        public Random Rnd { get { return rnd; } }

        public void CollectExpiredItems()
        {
            lock (BusyRemoteCommissioners)
            {
                BusyRemoteCommissioners = BusyRemoteCommissioners.Where(p => p.Item2 >= DateTime.Now).ToList();
            }
        }

        public List<Guid> GetBusyCommissioners()
        {
            return BusyRemoteCommissioners.ConvertAll(p => p.Item1);
        }

        public void AddBusyCommissioner(Guid id)
        {
            lock(BusyRemoteCommissioners)
            {
                BusyRemoteCommissioners.Add((id, DateTime.Now + Expiration));
            }
        }
    }
}
