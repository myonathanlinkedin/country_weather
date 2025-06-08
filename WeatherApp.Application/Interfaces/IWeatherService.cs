using WeatherApp.Domain.ValueObjects;

namespace WeatherApp.Application.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherData> GetWeatherForCityAsync(string cityName);
    }
} 