using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Api.Controllers;
using WeatherApp.Application.Contracts;
using WeatherApp.Application.Interfaces;
using WeatherApp.Domain.Entities;
using WeatherApp.Infrastructure.Data;
using WeatherApp.Infrastructure.Repositories;
using Xunit;

namespace WeatherApp.Tests.Controllers
{
    public class CountriesControllerTests
    {
        private readonly AppDbContext _dbContext;
        private readonly ICountryRepository _repository;
        private readonly CountriesController _controller;

        public CountriesControllerTests()
        {
            // Set up the in-memory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestCountriesDb")
                .Options;

            _dbContext = new AppDbContext(options);
            _repository = new CountryRepository(_dbContext);
            _controller = new CountriesController(_repository);

            // Ensure the database is created and seeded
            _dbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetCountries_ReturnsOkResult_WithListOfCountries()
        {
            // Act
            var result = await _controller.GetCountries();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<CountryDto>>(okResult.Value);
            Assert.Equal(6, returnValue.Count());
            Assert.Contains(returnValue, c => c.Name == "United States" && c.Code == "US");
            Assert.Contains(returnValue, c => c.Name == "United Kingdom" && c.Code == "UK");
        }

        [Fact]
        public async Task GetCities_ReturnsOkResult_WithListOfCities()
        {
            // Act
            var result = await _controller.GetCities("US");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<CityDto>>(okResult.Value);
            Assert.Equal(5, returnValue.Count());
            Assert.Contains(returnValue, c => c.Name == "New York");
            Assert.Contains(returnValue, c => c.Name == "Los Angeles");
        }
    }
} 