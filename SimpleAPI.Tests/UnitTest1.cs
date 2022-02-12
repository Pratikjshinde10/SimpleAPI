using System;
using Xunit;
using SimpleAPI.Controllers;

namespace SimpleAPI.Tests
{
    public class UnitTest1
    {       
        WeatherForecastController _controller = new WeatherForecastController(); [Fact]
        public void Test1()
        {
            var returnvalue = _controller.Get();
            Assert.NotNull(returnvalue);
        }
    }
}
