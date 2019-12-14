using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Configuration;
using DataSource.Entities; 
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DataSource
{
    public class GeoLocationDataRetriever : IDataRetriever<GeoLocation>
    {
        private readonly IOptions<DatasourceConfiguration> _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public GeoLocationDataRetriever(IOptions<DatasourceConfiguration> configuration,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        private async Task<HttpResponseMessage> GetGeoLocationFromAPI(string parameter)
        {
            string zipCode = parameter;


            var API_PATH_IP_API = string.Format(_configuration.Value.GeoLocationIPPath, zipCode);

            var request = new HttpRequestMessage(HttpMethod.Get, API_PATH_IP_API);

            var client = _httpClientFactory.CreateClient();

            return await client.SendAsync(request);
        }

        public async Task<List<GeoLocation>> GetData(string parameter)
        {
            var response = await GetGeoLocationFromAPI(parameter);

            List<GeoLocation> list = new List<GeoLocation>();

            if (response.IsSuccessStatusCode)
            {
                var locationDetails = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (locationDetails != null)
                {
                    var geoLocationItem = JsonConvert.DeserializeObject<GeoLocation>(locationDetails.Replace(@"\", ""));
                    if (geoLocationItem != null)
                        list.Add(geoLocationItem);
                }
            }
            else
            {
                list.Add(await GetRandomGeoLocation());
            }

            return list;
        }

        private async Task<GeoLocation> GetRandomGeoLocation()
        {
            var geoLocationOfflineList = await GetGeoLocationList();
            Random rnd = new Random();
            var geoLocationItem = geoLocationOfflineList[rnd.Next(0, geoLocationOfflineList.Count - 1)];
            return geoLocationItem ?? new GeoLocation();
        }

        public Task<List<GeoLocation>> GetGeoLocationList()
        {
            var list = System.Text.Json.JsonSerializer.Deserialize<List<GeoLocation>>(
                File.ReadAllText(_configuration.Value.GeolocationDataFilePath));
            return Task.FromResult(list);
        }
    }
}