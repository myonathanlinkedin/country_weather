using System.Text.Json;
using WeatherApp.Application.Interfaces;
using WeatherApp.Domain.ValueObjects;
using WeatherApp.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WeatherApp.Infrastructure.Services
{
    public class OpenWeatherMapService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OpenWeatherMapService> _logger;
        private readonly string _apiKey;
        private readonly string _apiBaseUrl = "https://api.openweathermap.org/data/2.5/weather";

        public OpenWeatherMapService(
            HttpClient httpClient,
            IOptions<OpenWeatherMapSettings> settings,
            ILogger<OpenWeatherMapService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiKey = settings.Value.ApiKey;
        }

        public async Task<WeatherData> GetWeatherForCityAsync(string cityName)
        {
            try
            {
                var url = $"{_apiBaseUrl}?q={cityName}&appid={_apiKey}&units=imperial";
                var response = await _httpClient.GetAsync(url);
                
                response.EnsureSuccessStatusCode();
                
                var content = await response.Content.ReadAsStringAsync();
                var weatherResponse = JsonSerializer.Deserialize<OpenWeatherMapResponse>(content, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (weatherResponse == null)
                {
                    throw new Exception("Failed to deserialize weather data");
                }

                return MapToWeatherData(weatherResponse, cityName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching weather data for {CityName}", cityName);
                throw;
            }
        }

        private WeatherData MapToWeatherData(OpenWeatherMapResponse response, string cityName)
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

    // Response classes for JSON deserialization
    public class OpenWeatherMapResponse
    {
        public MainData? Main { get; set; }
        public WindData? Wind { get; set; }
        public List<WeatherData>? Weather { get; set; }
        public int Visibility { get; set; }
        public SysData? Sys { get; set; }

        public class MainData
        {
            public double Temp { get; set; }
            public double Humidity { get; set; }
            public double Pressure { get; set; }
        }

        public class WindData
        {
            public double Speed { get; set; }
            public double Deg { get; set; }
        }

        public class WeatherData
        {
            public string Main { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }

        public class SysData
        {
            public string Country { get; set; } = string.Empty;
        }
    }
} 