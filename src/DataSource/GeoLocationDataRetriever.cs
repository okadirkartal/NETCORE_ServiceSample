using System.Collections.Generic;
using System.Text.Json;
using Configuration;
using DataSource.Entities;
using Microsoft.Extensions.Options;

namespace DataSource
{
    public class GeoLocationDataRetriever: IDataRetriever<GeoLocation>
    {
        private IOptions<DatasourceConfiguration> _configuration;

        public GeoLocationDataRetriever(IOptions<DatasourceConfiguration> configuration)
        {
            _configuration = configuration;
        }
        
        public List<GeoLocation> GetData(string parameter)
        {
            var list = JsonSerializer.Deserialize<List<GeoLocation>>(_configuration.Value.GeolocationDataFilePath);
            return list;
        }
    }
}