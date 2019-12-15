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
    public class CityServiceController : ControllerBase
    {
        private readonly ILogger<CityServiceController> _logger;
        private readonly IOfflineDataRetriever<City> _dataRetriever;

        public CityServiceController(ILogger<CityServiceController> logger, IOfflineDataRetriever<City> dataRetriever)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataRetriever = dataRetriever ?? throw new ArgumentException(nameof(dataRetriever));
        }

        /* SAMPLE REQUEST(GET): https://localhost:44375/cityService/{cityId}.{format} 
                                https://localhost:44375/cityService/745044.json
                                https://localhost:44375/cityService/745044.xml
                            */
        [HttpGet("{id}.{format?}"), FormatFilter]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await _dataRetriever.GetDataFromFile(id);
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