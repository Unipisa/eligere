using EligereES.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace EligereES
{
    public static class OTPSender
    {
        public static string GenerateOTP()
        {
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var rnd = new Random();
            var ret = new System.Text.StringBuilder();
            for (var i = 0; i < 3; i++)
            {
                ret.Append(alphabet[rnd.Next(alphabet.Length)]);
            }
            return ret.ToString();
        }

        public static void SendMail(IConfiguration conf, string otp, string dest)
        {
            var smtpconf = conf.GetValue<SmtpConfiguration>("Smtp");
            if (smtpconf == null)
            {
                return;
            }
            var smtp = new System.Net.Mail.SmtpClient(smtpconf.Host, smtpconf.Port);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(smtpconf.User, smtpconf.Password);
            smtp.Send("no-reply@unipi.it", dest, "Codice OTP per Eligere", $"Il codice OTP per procedere al voto è: {otp}");
        }

        public static void SendSMS(IConfiguration conf, string otp, string mobile)
        {
            var m = HttpUtility.UrlEncode(mobile);
            var endpoint = conf.GetValue<string>("SendSMSEndPoint");
            var req = WebRequest.Create(endpoint + "?msg=" + otp + "%20Codice%20di%20voto%20di%20Eligere&num=" + m);
            var resp = req.GetResponse();
        }
    }
}
