using System; 
using System.Threading.Tasks;
using DataSource;
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
            _logger = logger;
            _dataRetriever = dataRetriever;
        }

        [HttpPost("{format?}"),FormatFilter]
        public async Task<GeoLocation> Post([FromBody]string zipCode)
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
//        
//        [HttpPost]
//        public async Task<string> GetTimeZone([FromBody]string zipCode)
//        {
//            try
//            {
//                var result = await _dataRetriever.GetData(zipCode);// _dataRetriever.GetData(cityCode);
//                return result?[0]?.timezone;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex.Message);
//                return null;
//            }
//        }
    }
}