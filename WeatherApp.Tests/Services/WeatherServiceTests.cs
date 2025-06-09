using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using WeatherApp.Application.Interfaces;
using WeatherApp.Domain.ValueObjects;
using WeatherApp.Infrastructure.Configuration;
using WeatherApp.Infrastructure.Services;
using Xunit;

namespace WeatherApp.Tests.Services
{
    public class WeatherServiceTests
    {
        [Fact]
        public async Task GetWeatherForCityAsync_ReturnsWeatherData()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(GetMockWeatherJson()),
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            
            var mockSettings = new Mock<IOptions<OpenWeatherMapSettings>>();
            mockSettings
                .Setup(x => x.Value)
                .Returns(new OpenWeatherMapSettings { 
                    ApiKey = "test-api-key",
                    ApiUrl = "https://api.openweathermap.org/data/2.5/weather" 
                });
            
            var mockLogger = new Mock<ILogger<OpenWeatherMapService>>();
            
            var service = new OpenWeatherMapService(httpClient, mockSettings.Object, mockLogger.Object);
            var cityName = "New York";

            // Act
            var result = await service.GetWeatherForCityAsync(cityName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cityName, result.CityName);
            Assert.Equal("US", result.CountryName);
            Assert.Equal(5.1, result.WindSpeed);
            Assert.NotEmpty(result.WindDirection);
            Assert.NotEqual(0, result.Visibility);
            Assert.Equal("clear sky", result.SkyConditions);
            Assert.Equal(72.0, result.TemperatureFahrenheit);
            Assert.InRange(result.TemperatureCelsius, 21.5, 22.5); // Around 22.22°C
            Assert.NotEqual(0, result.Humidity);
            Assert.NotEqual(0, result.Pressure);
        }

        [Fact]
        public async Task WeatherService_ShouldHandleFailures()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("{\"message\":\"City not found\"}")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            
            var mockSettings = new Mock<IOptions<OpenWeatherMapSettings>>();
            mockSettings
                .Setup(x => x.Value)
                .Returns(new OpenWeatherMapSettings { 
                    ApiKey = "test-api-key",
                    ApiUrl = "https://api.openweathermap.org/data/2.5/weather" 
                });
            
            var mockLogger = new Mock<ILogger<OpenWeatherMapService>>();
            
            var service = new OpenWeatherMapService(httpClient, mockSettings.Object, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<CityNotFoundException>(() => service.GetWeatherForCityAsync("NonExistentCity"));
        }

        [Theory]
        [InlineData(32, 0)] // Freezing point
        [InlineData(212, 100)] // Boiling point
        [InlineData(98.6, 37)] // Body temperature (rounded)
        [InlineData(68, 20)] // Room temperature
        [InlineData(-40, -40)] // Same in both scales
        public void TemperatureConversion_IsCorrect(double fahrenheit, double expectedCelsius)
        {
            // Arrange
            var weatherData = new WeatherData { TemperatureFahrenheit = fahrenheit };

            // Act
            var actualCelsius = Math.Round(weatherData.TemperatureCelsius);

            // Assert
            Assert.Equal(expectedCelsius, actualCelsius);
        }

        private string GetMockWeatherJson()
        {
            return @"{
                ""coord"": {
                    ""lon"": -74.006,
                    ""lat"": 40.7143
                },
                ""weather"": [
                    {
                        ""id"": 800,
                        ""main"": ""Clear"",
                        ""description"": ""clear sky"",
                        ""icon"": ""01d""
                    }
                ],
                ""base"": ""stations"",
                ""main"": {
                    ""temp"": 72.0,
                    ""feels_like"": 71.2,
                    ""temp_min"": 68.68,
                    ""temp_max"": 74.52,
                    ""pressure"": 1016,
                    ""humidity"": 50
                },
                ""visibility"": 10000,
                ""wind"": {
                    ""speed"": 5.1,
                    ""deg"": 90
                },
                ""clouds"": {
                    ""all"": 0
                },
                ""dt"": 1663844904,
                ""sys"": {
                    ""type"": 2,
                    ""id"": 2039034,
                    ""country"": ""US"",
                    ""sunrise"": 1663842345,
                    ""sunset"": 1663886265
                },
                ""timezone"": -14400,
                ""id"": 5128581,
                ""name"": ""New York"",
                ""cod"": 200
            }";
        }
    }
} 
 