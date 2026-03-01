using BlazorServer.Shared;
using Xunit;

namespace BlazorServer.Tests;

public class WeatherForecastTests
{
    [Fact]
    public void TemperatureConversion_IsCorrect()
    {
        var forecast = new WeatherForecast { TemperatureC = 25 };
        var tempF = forecast.TemperatureF;
        Assert.Equal(32 + (int)(25 / 0.5556), tempF);
    }
}
