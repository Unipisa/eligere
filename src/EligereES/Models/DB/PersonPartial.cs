using EligereES.Models.Extensions;
using System;
using System.Collections.Generic;

namespace EligereES.Models.DB
{
    public partial class Person
    {
        public PersonAttributes GetPersonAttributes() { return PersonAttributes.FromJson(Attributes); }
        public void SetPersonAttributes(PersonAttributes value) { Attributes = value.ToJson(); }
    }
}
