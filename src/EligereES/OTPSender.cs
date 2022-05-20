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
        public static string GenerateOTP(int otpLength = 3)
        {
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var rnd = new Random();
            var ret = new System.Text.StringBuilder();
            for (var i = 0; i < otpLength; i++)
            {
                ret.Append(alphabet[rnd.Next(alphabet.Length)]);
            }
            return ret.ToString();
        }

        public static void SendMail(IConfiguration conf, string otp, string dest)
        {
            var host = conf.GetValue<string>("Smtp:Host");
            var port = conf.GetValue<int>("Smtp:Port");
            var enableSSL = conf.GetValue<bool>("Smtp:Encryption");
            var useAuth = conf.GetValue<bool>("Smtp:UseAuthentication");
            var user = conf.GetValue<string>("Smtp:User");
            var password = conf.GetValue<string>("Smtp:Password");
            var sender = conf.GetValue<string>("Smtp:Sender");

            if (host == null)
            {
                return;
            }
            var smtpconf = new SmtpConfiguration { Host = host, Port = port, Encryption = enableSSL, UseAuthentication = useAuth, User = user, Password = password, Sender = sender };
            if (smtpconf == null)
            {
                return;
            }
            var smtp = new System.Net.Mail.SmtpClient(smtpconf.Host, smtpconf.Port);
            smtp.EnableSsl = smtpconf.Encryption;
            smtp.UseDefaultCredentials = !smtpconf.UseAuthentication;
            if (smtpconf.UseAuthentication)
                smtp.Credentials = new System.Net.NetworkCredential(smtpconf.User, smtpconf.Password);
            smtp.Send(smtpconf.Sender, dest, "Codice OTP per Eligere", $"Il codice OTP per procedere al voto è: {otp}");
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
