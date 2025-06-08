using System.Text.Json.Serialization;

namespace WeatherApp.Infrastructure.Models.OpenWeatherMap
{
    public class OpenWeatherMapResponseDto
    {
        [JsonPropertyName("main")]
        public MainData? Main { get; set; }
        
        [JsonPropertyName("wind")]
        public WindData? Wind { get; set; }
        
        [JsonPropertyName("weather")]
        public List<WeatherInfo>? Weather { get; set; }
        
        [JsonPropertyName("visibility")]
        public int Visibility { get; set; }
        
        [JsonPropertyName("sys")]
        public SysData? Sys { get; set; }
    }

    public class MainData
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }
        
        [JsonPropertyName("humidity")]
        public double Humidity { get; set; }
        
        [JsonPropertyName("pressure")]
        public double Pressure { get; set; }
    }

    public class WindData
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }
        
        [JsonPropertyName("deg")]
        public double Deg { get; set; }
    }

    public class WeatherInfo
    {
        [JsonPropertyName("main")]
        public string Main { get; set; } = string.Empty;
        
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }

    public class SysData
    {
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;
    }
} 