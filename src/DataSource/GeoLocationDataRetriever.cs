using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Configuration;
using DataSource.Contracts;
using Models.GeoLocation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DataSource
{
    public class GeoLocationDataRetriever : IOfflineDataRetriever<GeoLocation>, IOnlineDataRetriever,
        IDataRetriever<GeoLocation>
    {
        private readonly IOptions<DatasourceConfiguration> _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger  _logger;

        public GeoLocationDataRetriever(IOptions<DatasourceConfiguration> configuration,
            IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = loggerFactory.CreateLogger(typeof(GeoLocationDataRetriever));
        }

        public async Task<HttpResponseMessage> GetDataFromApi(string parameter)
        {
            string zipCode = parameter;


            var apiPathIpApi = string.Format(_configuration.Value.GeoLocationApiBaseUrl, zipCode);

            var request = new HttpRequestMessage(HttpMethod.Get, apiPathIpApi);

            var client = _httpClientFactory.CreateClient();

            return await client.SendAsync(request);
        }

        public async Task<List<GeoLocation>> GetData(string parameter)
        {
            List<GeoLocation> list = new List<GeoLocation>();

            try
            {
                var response = await GetDataFromApi(parameter);


                if (response.IsSuccessStatusCode)
                {
                    var locationDetails = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (locationDetails != null)
                    {
                        var geoLocationItem =
                            JsonConvert.DeserializeObject<GeoLocation>(locationDetails.Replace(@"\", ""));
                        if (geoLocationItem != null)
                            list.Add(geoLocationItem);
                    }
                }
                else
                {
                    list.Add(await GetRandomData());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex, "An error occured while retrieving online geo location data");
                list.Add(await GetRandomData());
            }

            return list;
        }

        public async Task<GeoLocation> GetRandomData()
        {
            var geoLocationOfflineList = await GetDataFromFile(null);
            Random rnd = new Random();
            var geoLocationItem = geoLocationOfflineList[rnd.Next(0, geoLocationOfflineList.Count - 1)];
            return geoLocationItem ?? new GeoLocation();
        }

        public async Task<List<GeoLocation>> GetDataFromFile(string parameter)
        {
            var list = System.Text.Json.JsonSerializer.Deserialize<List<GeoLocation>>(
                File.ReadAllText(_configuration.Value.GeolocationDataFilePath));
            return await Task.FromResult(list);
        }
    }
}