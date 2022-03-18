using System;
using Xunit;
using SimpleAPI.Controllers;
using Microsoft.Extensions.Configuration;

namespace SimpleAPI.Tests
{
    public class UnitTest1
    {
        private static IConfiguration _config;
        WeatherForecastController _controller = new WeatherForecastController(_config);
        
        [Fact]
        public void Test1()
        {
            var returnvalue = _controller.Get();
            Assert.NotNull(returnvalue);            
        }
    }
}
