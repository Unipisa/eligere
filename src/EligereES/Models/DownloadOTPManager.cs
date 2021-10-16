using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EligereES.Models
{
    public class DownloadOTPManager
    {
        private string _OTP = null;
        private DateTime expiration = DateTime.Now;

        public bool HasOTP { 
            get {
                if (DateTime.Now > expiration) ResetOTP();
                return _OTP != null;
            } 
        }

        public string OTP { 
            get {
                if (DateTime.Now > expiration) ResetOTP();
                return _OTP; 
            } 
        }

        public DateTime? Expiration {
            get { 
                return this.HasOTP ? expiration : null;
            }
        }

        public void GenerateOTP()
        {
            expiration = DateTime.Now + TimeSpan.FromMinutes(5);
            _OTP = OTPSender.GenerateOTP(8);
        }

        public void ResetOTP()
        {
            _OTP = null;
        }
    }
}
