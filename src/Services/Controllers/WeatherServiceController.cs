using System;
using System.Threading.Tasks;
using DataSource.Contracts;
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

        public WeatherServiceController(ILogger<WeatherServiceController> logger,
            IDataRetriever<WeatherInfo> dataRetriever)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataRetriever = dataRetriever ?? throw new ArgumentException(nameof(dataRetriever));
        }

        /* SAMPLE REQUEST(GET): https://localhost:44375/weatherService/{cityId}.{format} 
                              https://localhost:44375/weatherService/745044.json
                              https://localhost:44375/weatherService/745044.xml
                          */
        [HttpGet("{cityCode}.{format?}"), FormatFilter]
        public async Task<IActionResult> Get(string cityCode)
        {
            try
            {
                var result = await _dataRetriever.GetData(cityCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NoContent();
            }
        }
    }
}