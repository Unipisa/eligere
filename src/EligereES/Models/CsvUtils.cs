using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

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

        public static byte[] ExportToCsv<T>(IEnumerable<T> records, char separator = ';')
        {
            if (records == null || !records.Any())
                return Array.Empty<byte>();

            var sb = new StringBuilder();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Header
            sb.AppendLine(string.Join(separator, properties.Select(p => p.Name)));

            // Rows
            foreach (var record in records)
            {
                var values = properties.Select(p =>
                {
                    var val = p.GetValue(record, null)?.ToString() ?? "";
                    // Gestione escaping se ci sono separatori o virgolette
                    if (val.Contains(separator) || val.Contains("\""))
                    {
                        val = $"\"{val.Replace("\"", "\"\"")}\"";
                    }
                    return val;
                });

                sb.AppendLine(string.Join(separator, values));
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

    }
}
