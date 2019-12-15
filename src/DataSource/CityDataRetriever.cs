using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Configuration;
using DataSource.Contracts;
using Microsoft.Extensions.Options;
using Models;

namespace DataSource
{
    public class CityDataRetriever : IOfflineDataRetriever<City>
    {
        private readonly IOptions<DatasourceConfiguration> _configuration;

        public CityDataRetriever(IOptions<DatasourceConfiguration> configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<List<City>> GetDataFromFile(string parameter)
        {
            int.TryParse(parameter, out int cityId);
            var list = JsonSerializer.Deserialize<List<City>>(File.ReadAllText(_configuration.Value.CityDataFilePath));
            return await Task.Run(() => { return list.Where(x => x.id == cityId).ToList(); });
        }
    }
}