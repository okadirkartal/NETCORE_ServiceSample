using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Configuration;
using DataSource.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Newtonsoft.Json;

namespace DataSource
{
    public class WeatherDataRetriever : IOnlineDataRetriever, IDataRetriever<WeatherInfo>
    {
        private readonly IOptions<DatasourceConfiguration> _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<WeatherDataRetriever> _logger;

        public WeatherDataRetriever(IOptions<DatasourceConfiguration> configuration,
            IHttpClientFactory httpClientFactory, ILogger<WeatherDataRetriever> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<HttpResponseMessage> GetDataFromApi(string cityId)
        {
            var apiPathIpApi = _configuration.Value.WeatherApiBaseUrl + "&id=" + cityId;

            var request = new HttpRequestMessage(HttpMethod.Get, apiPathIpApi);

            var client = _httpClientFactory.CreateClient();

            return await client.SendAsync(request);
        }

        public async Task<List<WeatherInfo>> GetData(string parameter)
        {
            List<WeatherInfo> list = new List<WeatherInfo>();

            try
            {
                var response = await GetDataFromApi(parameter);


                if (response.IsSuccessStatusCode)
                {
                    var locationDetails = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (locationDetails != null)
                    {
                        var weatherInfoItem =
                            JsonConvert.DeserializeObject<WeatherInfo>(locationDetails.Replace(@"\", ""));
                        if (weatherInfoItem != null)
                            list.Add(weatherInfoItem);
                    }
                }
                else
                {
                    _logger.LogError(
                        $"An error occured while retrieving online weather info data {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex, "An error occured while retrieving online weather info data");
            }

            return list;
        }
    }
}