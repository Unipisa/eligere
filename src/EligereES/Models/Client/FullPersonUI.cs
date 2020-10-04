using EligereES.Models.DB;
using System;
using System.Collections.Generic;

namespace EligereES.Models.Client
{
    public partial class FullPersonUI
    {
        public FullPersonUI()
        {
        }

        public FullPersonUI(Person p)
        {
            this.Id = p.Id;
            this.FirstName = p.FirstName;
            this.LastName = p.LastName;
            this.PublicId = p.PublicId;
            this.BirthDate = p.BirthDate;
            this.BirthPlace = p.BirthPlace;
            this.Attributes = p.Attributes;
        }

        public void UpdatePerson(Person p)
        {
            p.FirstName = this.FirstName;
            p.LastName = this.LastName;
            p.PublicId = this.PublicId;
            p.BirthDate = this.BirthDate;
            p.BirthPlace = this.BirthPlace;
            p.Attributes = this.Attributes;
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PublicId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string Attributes { get; set; }
    }
}
