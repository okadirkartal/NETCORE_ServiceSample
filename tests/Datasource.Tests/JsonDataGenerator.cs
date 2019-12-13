using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using DataSource.Entities;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Datasource.Tests
{
    public class JsonDataGenerator
    {
        [Test]
        public void GenerateCities()
        {
            var cities = new List<City>
            {
                    new City(){  Code = "1", Name = "Tartu"},
                    new City(){  Code = "2", Name = "Narva"},
                    new City(){  Code = "3", Name = "Pärnu"},
                    new City(){  Code = "4", Name = "Kohtla-Järve"},
                    new City(){  Code = "5", Name = "Viljandi"},
                    new City(){  Code = "6", Name = "Viljandi"},
                    new City(){  Code = "7", Name = "Maardu"},
                    new City(){  Code = "8", Name = "Rakvere"},
                    new City(){  Code = "9", Name = "Kuressaare"}
            };
            
            WriteToFile<List<City>>("cities.json",cities);
        }

        public void WriteToFile<T>(string filePath,T model)
        {
           var _serviceDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.Parent?.FullName;
           var jsonData = JsonConvert.SerializeObject(model,Formatting.Indented);
            using (var writer = new StreamWriter(Path.Combine(_serviceDirectory, $@"src{Path.DirectorySeparatorChar}Services{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}",filePath), append: false))
            {
                writer.WriteLine(jsonData);
            }
        }
    }
}