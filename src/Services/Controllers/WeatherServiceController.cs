using System;
using System.Collections.Generic; 
using System.Threading.Tasks;
using DataSource;
using DataSource.Contracts;
using DataSource.Entities;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.Extensions.Logging;
using Models;

namespace Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherServiceController : ControllerBase
    {
        private readonly ILogger<WeatherServiceController> _logger;
        private readonly IDataRetriever<WeatherInfo> _dataRetriever; 

        public WeatherServiceController(ILogger<WeatherServiceController> logger, IDataRetriever<WeatherInfo> dataRetriever)
        {
            _logger = logger;
            _dataRetriever = dataRetriever;
        }

        [HttpGet("{cityCode}.{format?}"),FormatFilter]
        public async Task<List<WeatherInfo>> Get(string cityCode)
        {
            try
            {
                var result = await _dataRetriever.GetData(cityCode);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}