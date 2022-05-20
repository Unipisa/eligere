using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EligereES.Models
{
    public class SmtpConfiguration
    {
        public SmtpConfiguration()
        {
            Port = 25;
            Encryption = false;
            UseAuthentication = false;
        }
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool Encryption { get; set; }
        public bool UseAuthentication { get; set; }
        public string Sender { get; set; }
    }
}
