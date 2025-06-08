using WeatherApp.Domain.Entities;

namespace WeatherApp.Application.Interfaces
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<Country?> GetCountryByCodeAsync(string countryCode);
        Task<IEnumerable<City>> GetCitiesByCountryCodeAsync(string countryCode);
    }
} 