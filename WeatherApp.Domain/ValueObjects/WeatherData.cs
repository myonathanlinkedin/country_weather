namespace WeatherApp.Domain.ValueObjects
{
    public class WeatherData
    {
        public string CityName { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public DateTime Time { get; set; } = DateTime.UtcNow;
        public double WindSpeed { get; set; }
        public string WindDirection { get; set; } = string.Empty;
        public double Visibility { get; set; }
        public string SkyConditions { get; set; } = string.Empty;
        public double TemperatureFahrenheit { get; set; }
        public double TemperatureCelsius => (TemperatureFahrenheit - 32) * 5 / 9;
        public double DewPoint { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
    }
} 