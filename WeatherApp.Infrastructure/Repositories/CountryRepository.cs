using Microsoft.EntityFrameworkCore;
using WeatherApp.Application.Interfaces;
using WeatherApp.Domain.Entities;
using WeatherApp.Infrastructure.Data;

namespace WeatherApp.Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _context;

        public CountryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<Country?> GetCountryByCodeAsync(string countryCode)
        {
            return await _context.Countries
                .FirstOrDefaultAsync(c => c.Code.Equals(countryCode, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<City>> GetCitiesByCountryCodeAsync(string countryCode)
        {
            var country = await _context.Countries
                .Include(c => c.Cities)
                .FirstOrDefaultAsync(c => c.Code.Equals(countryCode, StringComparison.OrdinalIgnoreCase));

            return country?.Cities ?? new List<City>();
        }
    }
} 