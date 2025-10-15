using EligereES.Models.DB;
using EligereES.Models.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace EligereES.Models.Client
{
    public partial class ElectionUI
    {
        public ElectionUI() { }

        public ElectionUI(Election e)
        {
            this.Id = e.Id;
            this.Name = e.Name;
            this.Description = e.Description;
            this.Configuration = e.Configuration;
            this.PollStartDate = e.PollStartDate;
            this.PollEndDate = e.PollEndDate;
            this.Active = e.Active;
            this.ElectorateListClosingDate = e.ElectorateListClosingDate;
            this.ElectionTypeFk = e.ElectionTypeFk;
        }

        public void UpdateElection(Election e)
        {
            e.Name = this.Name;
            e.Description = this.Description;
            e.Configuration = this.Configuration;
            e.PollStartDate = this.PollStartDate;
            e.PollEndDate = this.PollEndDate;
            e.Active = this.Active;
            e.ElectorateListClosingDate = this.ElectorateListClosingDate;
            e.ElectionTypeFk = this.ElectionTypeFk;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Configuration { get; set; }
        public ElectionConfiguration ElectionConfiguration
        {
            get { return ElectionConfiguration.FromJson(Configuration); }
        }
        public List<SelectListItem> CandidatesTypeOptions
        {
            get { return ElectionConfiguration.CandidatesTypeOptions(); }
        }
        public List<SelectListItem> IdentificationTypeOptions
        {
            get { return ElectionConfiguration.IdentificationTypeOptions(); }
        }

        public DateTime PollStartDate { get; set; }
        public DateTime PollEndDate { get; set; }
        public bool Active { get; set; }
        public DateTime? ElectorateListClosingDate { get; set; }
        public Guid? ElectionTypeFk { get; set; }

    }
}
