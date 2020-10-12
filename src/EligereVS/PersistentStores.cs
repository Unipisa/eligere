using RocksDbSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EligereVS
{
    public class PersistentStores
    {
        internal const string ConfigurationLocation = "Data/Configuration";
        internal const string TicketsLocation = "Data/Tickets";
        internal const string SecureBallotLocation = "Data/SecureBallot";

        private string contentRootPath;

        private RocksDb configuration = null;
        private RocksDb tickets = null;
        private RocksDb secureBallot = null;

        public void SetContentRootPath(string path)
        {
            contentRootPath = path;
        }

        private RocksDb InitDb(string path)
        {
            var opt = new DbOptions().SetCreateIfMissing(true);
            return RocksDb.Open(opt, path);
        }

        public RocksDb Configuration { 
            get { 
                if (configuration == null)
                {
                    configuration = InitDb(Path.Combine(contentRootPath, ConfigurationLocation));
                }
                return configuration; 
            } 
        }

        public RocksDb Tickets { 
            get {
                if (tickets == null)
                {
                    tickets = InitDb(Path.Combine(contentRootPath, TicketsLocation));
                }
                return tickets; 
            }
        }

        public RocksDb SecureBallot
        {
            get
            {
                if (secureBallot == null)
                {
                    secureBallot = InitDb(Path.Combine(contentRootPath, SecureBallotLocation));
                }
                return secureBallot;
            }
        }
    }
}
