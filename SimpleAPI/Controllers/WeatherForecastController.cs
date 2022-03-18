using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SimpleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching", "Pleasant"
        };
        
        private readonly IConfiguration _config;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IConfiguration config)
        {
            _config = config;
        }      

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            string keyid = _config.GetValue<string>("keyid");
            var rng = new Random();
            return Enumerable.Range(1, 12).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
