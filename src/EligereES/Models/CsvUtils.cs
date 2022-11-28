using CsvHelper;
using System;
using System.Collections.Generic;

namespace EligereES.Models
{
    public record CsvPerson(string FirstName, string LastName, string CompanyId, string PublicId, string BirthPlace, DateTime BirthDate, string Role, string Status);

    public class CsvUtils
    {
        public static List<CsvPerson> ParsePeople(CsvReader csv)
        {
            var lines = new List<CsvPerson>();
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var firstName = csv.GetField<string>(0).Trim(' ', '\t');
                var lastName = csv.GetField<string>(1).Trim(' ', '\t');
                var companyId = csv.GetField<string>(2).Trim(' ', '\t');
                var publicId = csv.GetField<string>(3).Trim(' ', '\t').ToUpper().Replace(" ", "");
                var birthPlace = csv.GetField<string>(4).Trim(' ', '\t');
                DateTime birthDate;
                var role = csv.GetField<string>(6).Trim(' ', '\t');
                string errmsg = null;
                if (!csv.TryGetField<DateTime>(5, out birthDate))
                    errmsg = "Malformed data";

                if (firstName == "" && lastName == "") // skip blank lines, perhaps a better definition is needed...
                    continue;

                lines.Add(new CsvPerson(firstName, lastName, companyId, publicId, birthPlace, birthDate, role, errmsg));
            }
            return lines;
        }
    }
}
