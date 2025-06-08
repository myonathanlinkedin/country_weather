using Microsoft.AspNetCore.Mvc;
using WeatherApp.Application.Contracts;
using WeatherApp.Application.Interfaces;

namespace WeatherApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("{cityName}")]
        public async Task<ActionResult<WeatherDto>> GetWeather(string cityName)
        {
            var weatherData = await _weatherService.GetWeatherForCityAsync(cityName);

            var weatherDto = new WeatherDto
            {
                CityName = weatherData.CityName,
                CountryName = weatherData.CountryName,
                Time = weatherData.Time.ToString("yyyy-MM-dd HH:mm:ss") + " UTC",
                WindSpeed = weatherData.WindSpeed,
                WindDirection = weatherData.WindDirection,
                Visibility = weatherData.Visibility,
                SkyConditions = weatherData.SkyConditions,
                TemperatureFahrenheit = weatherData.TemperatureFahrenheit,
                TemperatureCelsius = weatherData.TemperatureCelsius,
                DewPoint = weatherData.DewPoint,
                Humidity = weatherData.Humidity,
                Pressure = weatherData.Pressure
            };

            return Ok(weatherDto);
        }
    }
} 