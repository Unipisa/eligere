using System;
using System.Collections.Generic;
using EligereES.Models.DB;

namespace EligereES.Models.Client
{
    public partial class PersonUI
    {
        public PersonUI()
        {
        }

        public PersonUI(Person p)
        {
            this.Id = p.Id;
            this.FirstName = p.FirstName;
            this.LastName = p.LastName;
            this.PublicId = p.PublicId;
            this.Attributes = p.Attributes;
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PublicId { get; set; }
        public string Attributes { get; set; }
    }
}
