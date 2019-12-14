using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Configuration;
using DataSource.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DataSource
{
    public class GeoLocationDataRetriever : IDataRetriever<GeoLocation>
    {
        private readonly IOptions<DatasourceConfiguration> _configuration;
        private readonly ILogger<GeoLocationDataRetriever> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public GeoLocationDataRetriever(IOptions<DatasourceConfiguration> configuration,
            IHttpClientFactory httpClientFactory, ILogger<GeoLocationDataRetriever> logger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<List<GeoLocation>> GetData(string parameter)
        {
            string zipCode = parameter;

            List<GeoLocation> list = new List<GeoLocation>();

            var API_PATH_IP_API = string.Format(_configuration.Value.GeoLocationIPPath, zipCode);

            var request = new HttpRequestMessage(HttpMethod.Get, API_PATH_IP_API);

            var client = _httpClientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var locationDetails = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (locationDetails != null)
                {
                    _logger.LogDebug(locationDetails);
                    var geoLocationItem = JsonConvert.DeserializeObject<GeoLocation>(locationDetails.Replace(@"\", ""));
                    // var geoLocationItem = new GeoLocation() {city = locationDetails};
                    if (geoLocationItem != null)
                        list.Add(geoLocationItem);
                }
            }
            else
            {
                var geoLocationOfflineList = await GetGeoLocationList();
                Random rnd=new Random();
                var geoLocationItem = geoLocationOfflineList[rnd.Next(0, geoLocationOfflineList.Count - 1)];
                list.Add(geoLocationItem);
            }

            return list;
        }
        
        public Task<List<GeoLocation>> GetGeoLocationList()
        {
            var list =  System.Text.Json.JsonSerializer.Deserialize<List<GeoLocation>>(File.ReadAllText(_configuration.Value.GeolocationDataFilePath));
            return Task.FromResult(list);
        }
    }
}