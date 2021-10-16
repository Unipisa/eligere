using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace EligereVS
{
    public class TicketsQueue
    {
        private List<string> _queue = new List<string>();

        public int Count { get { return _queue.Count; } }

        public string[] TicketsList 
        {
            get
            {
                return _queue.ToArray();
            }
        }

        public void AddTicket(string ticket)
        {
            _queue.Add(ticket);
        }

        private void LogFailedNotification(string contentRoot, string ticket, string additionalInfo)
        {
            var fn = Path.Combine(contentRoot, $"wwwroot/temp/FailedNotifications.log");
            File.AppendAllLines(fn, new string[] { $"{DateTime.Now} - {ticket}", additionalInfo });
        }

        public void NotifyTickets(string contentRoot, string baseUrl)
        {
            foreach (var t in _queue.ToArray()) // copies the list so that I can modify during enumeration
            {
                try
                {
                    var urlBuilder = new System.Text.StringBuilder();
                    urlBuilder.Append(baseUrl).Append("/TicketUsed/").Append(t);
                    var req = WebRequest.Create(urlBuilder.ToString());
                    var resp = (HttpWebResponse)req.GetResponse();
                    if (resp.StatusCode == HttpStatusCode.OK) {
                        var text = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                        _queue.Remove(t);
                    } else
                    {
                        // Nothing to do, ticket stays in queue
                        LogFailedNotification(contentRoot, t, "");
                    }

                    resp.Close();
                } catch (Exception e)
                {
                    // Nothing to do, ticket stays in queue
                    LogFailedNotification(contentRoot, t, e.ToString());
                }
            }
        }
    }
}
