using System;
using System.Collections.Generic;
using System.Threading;
using EligereES.Models.DB;

namespace EligereES.Models.Client
{
    public partial class PollingStationCommissionUI
    {
        public PollingStationCommissionUI()
        {
        }

        public PollingStationCommissionUI(PollingStationCommission c)
        {
            this.Id = c.Id;
            this.ElectionFk = c.ElectionFk;
            this.Location = c.Location;
            this.DigitalLocation = c.DigitalLocation;
            this.Description = c.Description;
            this.ElectionFk = c.ElectionFk;
            this.President = new PersonUI(c.PresidentFkNavigation.PersonFkNavigation);
        }

        public void UpdatePollingStationCommission(PollingStationCommission c)
        {
            c.Location = this.Location;
            c.DigitalLocation = this.DigitalLocation;
            c.Description = this.Description;
            c.ElectionFk = this.ElectionFk;
            if (this.President != null)
                c.PresidentFk = this.President.Id;
        }

        public Guid Id { get; set; }
        public Guid ElectionFk { get; set; }
        public string Location { get; set; }
        public string DigitalLocation { get; set; }
        public string Description { get; set; }
        public PersonUI President { get; set; }
    }
}
