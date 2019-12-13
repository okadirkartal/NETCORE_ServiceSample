using System;
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
                new City() {Code = "1", Name = "Tallinn"},
                new City() {Code = "2", Name = "Tartu"},
                new City() {Code = "3", Name = "Narva"},
                new City() {Code = "4", Name = "Pärnu"},
                new City() {Code = "5", Name = "Kohtla-Järve"},
                new City() {Code = "6", Name = "Viljandi"},
                new City() {Code = "7", Name = "Maardu"},
                new City() {Code = "8", Name = "Rakvere"},
                new City() {Code = "9", Name = "Kuressaare"}
            };

            WriteToFile<List<City>>("cities.json", cities);
        }

        [Test]
        public void GenerateWheatherData()
        {
            var wheatherDatas = new List<Weather>
            {
                new Weather()
                {
                    City = new City() {Code = "1", Name = "Tallin"}, Summary = "Sunny", TemperatureC = 15,
                    Date = DateTime.UtcNow
                },
                new Weather()
                {
                    City = new City() {Code = "2", Name = "Tartu"}, Summary = "Cloudy", TemperatureC = 3,
                    Date = DateTime.UtcNow
                },
                new Weather()
                {
                    City = new City() {Code = "3", Name = "Narva"}, Summary = "Sunny", TemperatureC = 16,
                    Date = DateTime.UtcNow
                },
                new Weather()
                {
                    City = new City() {Code = "4", Name = "Pärnu"}, Summary = "Rainy", TemperatureC = 4,
                    Date = DateTime.UtcNow
                },
                new Weather()
                {
                    City = new City() {Code = "5", Name = "Kohtla-Järve"}, Summary = "Windy", TemperatureC = 2,
                    Date = DateTime.UtcNow
                },
                new Weather()
                {
                    City = new City() {Code = "6", Name = "Viljandi"}, Summary = "Snowy", TemperatureC = -3,
                    Date = DateTime.UtcNow
                },
                new Weather()
                {
                    City = new City() {Code = "7", Name = "Maardu"}, Summary = "Sunny", TemperatureC = 12,
                    Date = DateTime.UtcNow
                },
                new Weather()
                {
                    City = new City() {Code = "8", Name = "Rakvere"}, Summary = "Cloudy", TemperatureC = 6,
                    Date = DateTime.UtcNow
                },
                new Weather()
                {
                    City = new City() {Code = "9", Name = "Kuressaare"}, Summary = "Raimy", TemperatureC = 4,
                    Date = DateTime.UtcNow
                },
            };
            
            WriteToFile<List<Weather>>("wheathers.json", wheatherDatas);
        }

        public void WriteToFile<T>(string filePath, T model)
        {
            var _serviceDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.Parent
                ?.FullName;
            var jsonData = JsonConvert.SerializeObject(model, Formatting.Indented);
            using (var writer =
                new StreamWriter(
                    Path.Combine(_serviceDirectory,
                        $@"src{Path.DirectorySeparatorChar}Services{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}",
                        filePath), append: false))
            {
                writer.WriteLine(jsonData);
            }
        }
    }
}