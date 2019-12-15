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
        private readonly ILogger _logger;
        private readonly IDataRetriever<GeoLocation> _dataRetriever;

        public GeoLocationServiceController(ILoggerFactory loggerFactory,
            IDataRetriever<GeoLocation> dataRetriever)
        {
            _logger = loggerFactory.CreateLogger(typeof(GeoLocationServiceController));
            _dataRetriever = dataRetriever ?? throw new ArgumentException(nameof(dataRetriever));
        }


        /* SAMPLE REQUEST(POST): https://localhost:44375/geoLocationService/all.{format} 
                                https://localhost:44375/geoLocationService/all.json
                                https://localhost:44375/geoLocationService/all.xml
                                
          NOTE : zipCode data should be added as BODY parameter
                          */
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

        /* SAMPLE REQUEST(POST): https://localhost:44375/geoLocationService/timeZone.{format} 
                               https://localhost:44375/geoLocationService/timeZone.json
                               https://localhost:44375/geoLocationService/timeZone.xml
                               
         NOTE : zipCode data should be added as BODY parameter
                         */
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