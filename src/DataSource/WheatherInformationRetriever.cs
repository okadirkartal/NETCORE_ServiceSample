using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Configuration;
using DataSource.Entities;
using Microsoft.Extensions.Options;

namespace DataSource
{
    public class WheatherInformationRetriever : IDataRetriever<WeatherInformation>
    {
        private IOptions<DatasourceConfiguration> _configuration;

        public WheatherInformationRetriever(IOptions<DatasourceConfiguration> configuration)
        {
            _configuration = configuration;
        }
        
        public List<WeatherInformation> GetData(string parameter)
        {
            var list = JsonSerializer.Deserialize<List<WeatherInformation>>(Path.Combine(Directory.GetCurrentDirectory(), _configuration.Value.WeatherInformationDataFilePath));
            return list;
        }
    }
}