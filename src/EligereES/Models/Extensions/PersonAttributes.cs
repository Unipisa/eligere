using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using EligereES.Models.DB;

namespace EligereES.Models.Extensions
{
    public class PersonAttributes
    {
        public string ToJson()
        {
            return JsonSerializer.Serialize<PersonAttributes>(this);
        }

        public static PersonAttributes FromJson(string data)
        {
            return JsonSerializer.Deserialize<PersonAttributes>(data);
        }

        public PersonAttributes() { }

        public PersonAttributes(Person p)
        {
            var a = FromJson(p.Attributes);
            Role = a.Role;
            CompanyId = a.CompanyId;
            Login = a.Login;
        }

        public bool IsProfessor()  { return Role == "PO" || Role == "PA"; }

        public string Role { get; set; }
        public string CompanyId { get; set; }
        public string Login { get; set; }
        public string Mobile { get; set; }
    }
}
