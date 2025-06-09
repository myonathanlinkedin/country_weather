using System.Text.Json;
using WeatherApp.Application.Interfaces;
using WeatherApp.Domain.ValueObjects;
using WeatherApp.Infrastructure.Configuration;
using WeatherApp.Infrastructure.Models.OpenWeatherMap;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WeatherApp.Infrastructure.Services
{
    public class OpenWeatherMapService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OpenWeatherMapService> _logger;
        private readonly string _apiKey;
        private readonly string _apiBaseUrl;

        public OpenWeatherMapService(
            HttpClient httpClient,
            IOptions<OpenWeatherMapSettings> settings,
            ILogger<OpenWeatherMapService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiKey = settings.Value.ApiKey;
            _apiBaseUrl = settings.Value.ApiUrl;
        }

        public async Task<WeatherData> GetWeatherForCityAsync(string cityName)
        {
            try
            {
                var request = new OpenWeatherMapRequestDto
                {
                    CityName = cityName,
                    Units = "imperial"
                };

                var url = $"{_apiBaseUrl}?q={request.CityName}&appid={_apiKey}&units={request.Units}";
                var response = await _httpClient.GetAsync(url);
                
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("City not found: {CityName}", cityName);
                    throw new CityNotFoundException($"Weather data for city '{cityName}' not found.");
                }
                
                response.EnsureSuccessStatusCode();
                
                var content = await response.Content.ReadAsStringAsync();
                var weatherResponse = JsonSerializer.Deserialize<OpenWeatherMapResponseDto>(content, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (weatherResponse == null)
                {
                    throw new Exception("Failed to deserialize weather data");
                }

                return MapToWeatherData(weatherResponse, cityName);
            }
            catch (CityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching weather data for {CityName}", cityName);
                throw new WeatherServiceException($"An error occurred while fetching weather data for '{cityName}'.", ex);
            }
        }

        private WeatherData MapToWeatherData(OpenWeatherMapResponseDto response, string cityName)
        {
            var main = response.Main;
            var wind = response.Wind;
            var weather = response.Weather?.FirstOrDefault();

            return new WeatherData
            {
                CityName = cityName,
                CountryName = response.Sys?.Country ?? "Unknown",
                Time = DateTime.UtcNow,
                WindSpeed = wind?.Speed ?? 0,
                WindDirection = GetWindDirection(wind?.Deg ?? 0),
                Visibility = (response.Visibility / 1609.34), // Convert meters to miles
                SkyConditions = weather?.Description ?? "Unknown",
                TemperatureFahrenheit = main?.Temp ?? 0,
                DewPoint = CalculateDewPoint(main?.Temp ?? 0, main?.Humidity ?? 0),
                Humidity = main?.Humidity ?? 0,
                Pressure = main?.Pressure ?? 0
            };
        }

        private string GetWindDirection(double degrees)
        {
            string[] directions = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };
            int index = (int)Math.Round(((degrees % 360) / 22.5)) % 16;
            return directions[index];
        }

        private double CalculateDewPoint(double tempF, double humidity)
        {
            // Convert F to C for the calculation
            double tempC = (tempF - 32) * 5 / 9;
            
            // Calculate dew point in Celsius
            double a = 17.27;
            double b = 237.7;
            double alpha = ((a * tempC) / (b + tempC)) + Math.Log(humidity / 100.0);
            double dewPointC = (b * alpha) / (a - alpha);
            
            // Convert back to Fahrenheit
            double dewPointF = (dewPointC * 9 / 5) + 32;
            return Math.Round(dewPointF, 1);
        }
    }

    public class CityNotFoundException : Exception 
    {
        public CityNotFoundException(string message) : base(message) { }
    }

    public class WeatherServiceException : Exception
    {
        public WeatherServiceException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
} 