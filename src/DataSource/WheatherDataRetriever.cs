using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Configuration;
using DataSource.Entities;
using Microsoft.Extensions.Options;

namespace DataSource
{
    public class WheatherDataRetriever : IDataRetriever<Weather>
    {
        private IOptions<DatasourceConfiguration> _configuration;

        public WheatherDataRetriever(IOptions<DatasourceConfiguration> configuration)
        {
            _configuration = configuration;
        }
        
        public List<Weather> GetData(string cityCode)
        {
            var list = JsonSerializer.Deserialize<List<Weather>>(File.ReadAllText(_configuration.Value.WeatherDataFilePath));
            if (!string.IsNullOrWhiteSpace(cityCode))
                list = list.Where(x => x.City.Code==cityCode).ToList();
            return list;
        }
    }
}