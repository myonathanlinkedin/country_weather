namespace WeatherApp.Application.Contracts
{
    public class WeatherDto
    {
        public string CityName { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public double WindSpeed { get; set; }
        public string WindDirection { get; set; } = string.Empty;
        public double Visibility { get; set; }
        public string SkyConditions { get; set; } = string.Empty;
        public double TemperatureFahrenheit { get; set; }
        public double TemperatureCelsius { get; set; }
        public double DewPoint { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
    }
} 