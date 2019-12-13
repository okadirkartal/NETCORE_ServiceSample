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
    public class WeatherServiceController : ControllerBase
    {
        private readonly ILogger<WeatherServiceController> _logger;
        private readonly IDataRetriever<Weather> _dataRetriever;

        public WeatherServiceController(ILogger<WeatherServiceController> logger, IDataRetriever<Weather> dataRetriever)
        {
            _logger = logger;
            _dataRetriever = dataRetriever;
        }
    }
}