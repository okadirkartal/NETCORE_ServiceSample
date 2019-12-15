using System;
using System.Linq;
using System.Threading.Tasks; 
using DataSource.Contracts;
using DataSource.Entities;
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
            _logger = logger?? throw  new ArgumentNullException(nameof(logger));
            _dataRetriever = dataRetriever ?? throw  new ArgumentException(nameof(dataRetriever));
        }


        [HttpPost]
        [Route("all.{format}"), FormatFilter]
        public async Task<GeoLocation> Post([FromBody] string zipCode)
        {
            try
            {
                var result = await _dataRetriever.GetData(zipCode);
                return result?[0];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        [HttpPost]
        [Route("timeZone.{format}"), FormatFilter]
        public async Task<Timezone> GetTimeZone([FromBody] string zipCode)
        {
            try
            {
                var result = await _dataRetriever.GetData(zipCode);
                return (result.Any()) ? result[0].timezone : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}