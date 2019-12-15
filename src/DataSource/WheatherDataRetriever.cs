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
    public class WeatherDataRetriever : IOfflineDataRetriever<WeatherInfo>, IOnlineDataRetriever,
        IDataRetriever<WeatherInfo>
    {
        private readonly IOptions<DatasourceConfiguration> _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<WeatherDataRetriever> _logger;

        public WeatherDataRetriever(IOptions<DatasourceConfiguration> configuration,
            IHttpClientFactory httpClientFactory, ILogger<WeatherDataRetriever> logger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
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
                    list.Add(await GetRandomData());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex, "An error occured while retrieving online weather info data");
                list.Add(await GetRandomData());
            }

            return list;
        }

        public async Task<WeatherInfo> GetRandomData()
        {
            var weatherInfoOfflineList = await GetDataFromFile(null);
            Random rnd = new Random();
            var weatherInfoItem = weatherInfoOfflineList[rnd.Next(0, weatherInfoOfflineList.Count - 1)];
            return weatherInfoItem ?? new WeatherInfo();
        }

        public async Task<List<WeatherInfo>> GetDataFromFile(string parameter)
        {
            var list = System.Text.Json.JsonSerializer.Deserialize<List<WeatherInfo>>(
                File.ReadAllText(_configuration.Value.GeolocationDataFilePath));
            return await Task.FromResult(list);
        }
    }
}