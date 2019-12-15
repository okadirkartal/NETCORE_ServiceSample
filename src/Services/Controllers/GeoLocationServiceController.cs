using System;
using System.Threading.Tasks;
using DataSource.Contracts;
using Models.GeoLocation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeoLocationServiceController : ControllerBase
    {
        private readonly ILogger<GeoLocationServiceController> _logger;
        private readonly IDataRetriever<GeoLocation> _dataRetriever;

        public GeoLocationServiceController(ILogger<GeoLocationServiceController> logger,
            IDataRetriever<GeoLocation> dataRetriever)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataRetriever = dataRetriever ?? throw new ArgumentException(nameof(dataRetriever));
        }


        [HttpPost]
        [Route("all.{format}"), FormatFilter]
        public async Task<IActionResult> Post([FromBody] string zipCode)
        {
            try
            {
                var result = await _dataRetriever.GetData(zipCode);
                return Ok(result?[0]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NoContent();
            }
        }

        [HttpPost]
        [Route("timeZone.{format}"), FormatFilter]
        public async Task<IActionResult> GetTimeZone([FromBody] string zipCode)
        {
            try
            {
                var result = await _dataRetriever.GetData(zipCode);
                return Ok(result[0].timezone);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NoContent();
            }
        }
    }
}