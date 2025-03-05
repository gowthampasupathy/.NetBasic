using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;
using System.Collections.Generic;

namespace WebApplication1.Controllers
{
    public class CsvImple
    {
        public Dictionary<string, string> LoadConstants(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File not found: {path}");
            }

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var records = csv.GetRecords<CsvModel>().ToDictionary(c => c.Id, c => c.Role);
                return records;
            }
        }
    }

    public class CsvModel
    {
        public string?Id { get; set; }
        public string?Role { get; set; }
    }
}
