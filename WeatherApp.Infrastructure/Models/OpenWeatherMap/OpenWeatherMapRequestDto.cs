namespace WeatherApp.Infrastructure.Models.OpenWeatherMap
{
    public class OpenWeatherMapRequestDto
    {
        public string CityName { get; set; } = string.Empty;
        public string Units { get; set; } = "imperial";
    }
} 