using System;
using System.Collections.Generic;
using System.IO;
using DataSource.Entities;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Datasource.Tests
{
    [Order(1)]
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

        [Test]
        public void GenerateGeoLocationData()
        {
            var geoLocations = new List<GeoLocation>
            {
                new GeoLocation
                {
                    zip_code = "60610", lat = 41_906762, lng = 087_632174, city = "Chicago",
                    state = "IL", timezone = new Timezone
                    {
                        timezone_identifier = "America/Chicago", timezone_abbr = "CST",
                        utc_offset_sec = -21600, is_dst = "F"
                    },
                    acceptable_city_names = new List<AcceptableCityName>
                    {
                        new AcceptableCityName() {city = "Sample Street 11", state = "IL"},
                        new AcceptableCityName() {city = "Sample Street 12", state = "IL"}
                    },
                    area_codes = new List<int> {312}
                },

                new GeoLocation
                {
                    zip_code = "60611", lat = 41_906762, lng = 087_632174, city = "Seattle",
                    state = "IR", timezone = new Timezone
                    {
                        timezone_identifier = "America/Seattle", timezone_abbr = "CST",
                        utc_offset_sec = -21601, is_dst = "F"
                    },
                    acceptable_city_names = new List<AcceptableCityName>
                    {
                        new AcceptableCityName() {city = "Sample1 Street", state = "IR"},
                        new AcceptableCityName() {city = "Sample2 Street", state = "IR"}
                    },
                    area_codes = new List<int> {313}
                },

                new GeoLocation
                {
                    zip_code = "60612", lat = 41_906762, lng = 087_632174, city = "Newyork",
                    state = "IK", timezone = new Timezone
                    {
                        timezone_identifier = "America/Newyork", timezone_abbr = "CST",
                        utc_offset_sec = -21600, is_dst = "F"
                    },
                    acceptable_city_names = new List<AcceptableCityName>
                    {
                        new AcceptableCityName() {city = "Sample3 Street", state = "IK"},
                        new AcceptableCityName() {city = "Sample3 Street", state = "IK"}
                    },
                    area_codes = new List<int> {314}
                },
                new GeoLocation
                {
                    zip_code = "60613", lat = 41_906762, lng = 087_632174, city = "Boston",
                    state = "IK", timezone = new Timezone
                    {
                        timezone_identifier = "America/Boston", timezone_abbr = "CST",
                        utc_offset_sec = -21604, is_dst = "F"
                    },
                    acceptable_city_names = new List<AcceptableCityName>
                    {
                        new AcceptableCityName() {city = "Sample4 Street", state = "IT"},
                        new AcceptableCityName() {city = "Sample5 Street", state = "IT"}
                    },
                    area_codes = new List<int> {315}
                },
                new GeoLocation
                {
                    zip_code = "60614", lat = 41_906762, lng = 087_632174, city = "Manhattan",
                    state = "IM", timezone = new Timezone
                    {
                        timezone_identifier = "America/Manhattan", timezone_abbr = "CST",
                        utc_offset_sec = -21605, is_dst = "F"
                    },
                    acceptable_city_names = new List<AcceptableCityName>
                    {
                        new AcceptableCityName() {city = "Sample5 Street", state = "IM"},
                        new AcceptableCityName() {city = "Sample6 Street", state = "IM"}
                    },
                    area_codes = new List<int> {316}
                }
            };

            WriteToFile<List<GeoLocation>>("geoLocations.json", geoLocations);
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