using System;
using System.Collections.Generic;
using EligereES.Models.DB;

namespace EligereES.Models.Client
{
    public partial class PollingStationSystemUI
    {
        public PollingStationSystemUI()
        {
        }

        public PollingStationSystemUI(PollingStationSystem ps)
        {
            this.Id = ps.Id;
            this.Ipaddress = ps.Ipaddress;
            this.DigitalFootprint = ps.DigitalFootprint;
        }

        public void UpdatePollingStationSystem(PollingStationSystem ps)
        {
            ps.Ipaddress = this.Ipaddress;
            ps.DigitalFootprint = this.DigitalFootprint;
        }

        public Guid Id { get; set; }
        public string Ipaddress { get; set; }
        public string DigitalFootprint { get; set; }
    }
}
