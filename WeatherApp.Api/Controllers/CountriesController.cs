using Microsoft.AspNetCore.Mvc;
using WeatherApp.Application.Contracts;
using WeatherApp.Application.Interfaces;

namespace WeatherApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;

        public CountriesController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountries()
        {
            var countries = await _countryRepository.GetAllCountriesAsync();
            var countryDtos = countries.Select(c => new CountryDto
            {
                Name = c.Name,
                Code = c.Code
            });

            return Ok(countryDtos);
        }

        [HttpGet("{countryCode}/cities")]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetCities(string countryCode)
        {
            var cities = await _countryRepository.GetCitiesByCountryCodeAsync(countryCode);
            var cityDtos = cities.Select(c => new CityDto
            {
                Name = c.Name
            });

            return Ok(cityDtos);
        }
    }
} 