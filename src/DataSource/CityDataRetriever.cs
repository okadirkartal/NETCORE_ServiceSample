using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Configuration;
using DataSource.Entities;
using Microsoft.Extensions.Options;

namespace DataSource
{
    public class CityDataRetriever : IDataRetriever<City>
    {
        private IOptions<DatasourceConfiguration> _configuration;

        public CityDataRetriever(IOptions<DatasourceConfiguration> configuration)
        {
            _configuration = configuration;
        }

        public Task<List<City>> GetData(string parameter)
        {
            string cityCode = parameter;
            var list = JsonSerializer.Deserialize<List<City>>(File.ReadAllText(_configuration.Value.CityDataFilePath));
            if (!string.IsNullOrWhiteSpace(cityCode))
                list = list.Where(x => x.Code == cityCode).ToList();
            return Task.FromResult(list);
        }
    }
}