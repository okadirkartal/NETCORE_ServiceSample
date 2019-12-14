using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataSource;
using DataSource.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CityServiceController : ControllerBase
    {
        private readonly ILogger<CityServiceController> _logger;
        private readonly IDataRetriever<City> _dataRetriever;

        public CityServiceController(ILogger<CityServiceController> logger, IDataRetriever<City> dataRetriever)
        {
            _logger = logger;
            _dataRetriever = dataRetriever;
        }

        [HttpGet]
        public async Task<IEnumerable<City>> Get(string cityCode)
        {
            try
            {
                var result =await _dataRetriever.GetData(cityCode);
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