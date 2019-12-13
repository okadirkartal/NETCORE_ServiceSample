using System;

namespace DataSource.Entities
{
    public class Weather
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);

        public string Summary { get; set; }

        public City City { get; set; }
    }
}