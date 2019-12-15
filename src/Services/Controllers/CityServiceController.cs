using System;
using System.Collections.Generic;
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
            _logger = logger?? throw  new ArgumentNullException(nameof(logger));
            _dataRetriever = dataRetriever ?? throw  new ArgumentException(nameof(dataRetriever));
        }

        [HttpGet("{id}.{format?}"), FormatFilter]
        public async Task<IEnumerable<City>> Get(string id)
        {
            try
            {
                var result = await _dataRetriever.GetDataFromFile(id);
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