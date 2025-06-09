using Microsoft.AspNetCore.Mvc;
using WeatherApp.Application.Contracts;
using WeatherApp.Application.Interfaces;
using WeatherApp.Infrastructure.Services;

namespace WeatherApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        [HttpGet("{cityName}")]
        [ProducesResponseType(typeof(WeatherDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<WeatherDto>> GetWeather(string cityName)
        {
            try
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
            catch (CityNotFoundException ex)
            {
                _logger.LogWarning(ex, "City not found: {CityName}", cityName);
                return NotFound(new ProblemDetails { 
                    Title = "City Not Found", 
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (WeatherServiceException ex)
            {
                _logger.LogError(ex, "Error retrieving weather for {CityName}", cityName);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails { 
                    Title = "Weather Service Error", 
                    Detail = "An error occurred while retrieving weather data. Please try again later.",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving weather for {CityName}", cityName);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails { 
                    Title = "Unexpected Error", 
                    Detail = "An unexpected error occurred. Please try again later.",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
} 