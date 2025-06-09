using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WeatherApp.Api.Controllers;
using WeatherApp.Application.Contracts;
using WeatherApp.Application.Interfaces;
using WeatherApp.Domain.ValueObjects;
using Xunit;

namespace WeatherApp.Tests.Controllers
{
    public class WeatherControllerTests
    {
        private readonly Mock<IWeatherService> _mockWeatherService;
        private readonly Mock<ILogger<WeatherController>> _mockLogger;
        private readonly WeatherController _controller;

        public WeatherControllerTests()
        {
            _mockWeatherService = new Mock<IWeatherService>();
            _mockLogger = new Mock<ILogger<WeatherController>>();
            _controller = new WeatherController(_mockWeatherService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetWeather_ReturnsOkResult_WithWeatherData()
        {
            // Arrange
            var cityName = "New York";
            var weatherData = new WeatherData
            {
                CityName = cityName,
                CountryName = "United States",
                Time = DateTime.UtcNow,
                WindSpeed = 5.0,
                WindDirection = "N",
                Visibility = 10.0,
                SkyConditions = "Clear",
                TemperatureFahrenheit = 72.0,
                DewPoint = 45.0,
                Humidity = 65.0,
                Pressure = 1013.0
            };

            _mockWeatherService
                .Setup(x => x.GetWeatherForCityAsync(cityName))
                .ReturnsAsync(weatherData);
            
            // Act
            var result = await _controller.GetWeather(cityName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var weatherDto = Assert.IsType<WeatherDto>(okResult.Value);
            
            Assert.Equal(cityName, weatherDto.CityName);
            Assert.Equal("United States", weatherDto.CountryName);
            Assert.Contains("UTC", weatherDto.Time);
            Assert.Equal(5.0, weatherDto.WindSpeed);
            Assert.Equal("N", weatherDto.WindDirection);
            Assert.Equal(10.0, weatherDto.Visibility);
            Assert.Equal("Clear", weatherDto.SkyConditions);
            Assert.Equal(72.0, weatherDto.TemperatureFahrenheit);
            Assert.Equal(22.22, Math.Round(weatherDto.TemperatureCelsius, 2));
            Assert.Equal(45.0, weatherDto.DewPoint);
            Assert.Equal(65.0, weatherDto.Humidity);
            Assert.Equal(1013.0, weatherDto.Pressure);
        }
    }
} 