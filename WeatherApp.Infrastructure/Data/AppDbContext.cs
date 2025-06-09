using Microsoft.EntityFrameworkCore;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entities
            modelBuilder.Entity<Country>()
                .HasMany(c => c.Cities)
                .WithOne(c => c.Country)
                .HasForeignKey(c => c.CountryId);

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Countries
            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, Name = "United States", Code = "US" },
                new Country { Id = 2, Name = "United Kingdom", Code = "UK" },
                new Country { Id = 3, Name = "Japan", Code = "JP" },
                new Country { Id = 4, Name = "Australia", Code = "AU" },
                new Country { Id = 5, Name = "Germany", Code = "DE" },
                new Country { Id = 6, Name = "Indonesia", Code = "ID" }
            );

            // Seed Cities
            modelBuilder.Entity<City>().HasData(
                // US Cities
                new City { Id = 1, Name = "New York", CountryId = 1 },
                new City { Id = 2, Name = "Los Angeles", CountryId = 1 },
                new City { Id = 3, Name = "Chicago", CountryId = 1 },
                new City { Id = 4, Name = "Houston", CountryId = 1 },
                new City { Id = 5, Name = "Miami", CountryId = 1 },
                
                // UK Cities
                new City { Id = 6, Name = "London", CountryId = 2 },
                new City { Id = 7, Name = "Manchester", CountryId = 2 },
                new City { Id = 8, Name = "Birmingham", CountryId = 2 },
                new City { Id = 9, Name = "Glasgow", CountryId = 2 },
                new City { Id = 10, Name = "Liverpool", CountryId = 2 },
                
                // Japan Cities
                new City { Id = 11, Name = "Tokyo", CountryId = 3 },
                new City { Id = 12, Name = "Osaka", CountryId = 3 },
                new City { Id = 13, Name = "Kyoto", CountryId = 3 },
                new City { Id = 14, Name = "Sapporo", CountryId = 3 },
                new City { Id = 15, Name = "Yokohama", CountryId = 3 },
                
                // Australia Cities
                new City { Id = 16, Name = "Sydney", CountryId = 4 },
                new City { Id = 17, Name = "Melbourne", CountryId = 4 },
                new City { Id = 18, Name = "Brisbane", CountryId = 4 },
                new City { Id = 19, Name = "Perth", CountryId = 4 },
                new City { Id = 20, Name = "Adelaide", CountryId = 4 },
                
                // Germany Cities
                new City { Id = 21, Name = "Berlin", CountryId = 5 },
                new City { Id = 22, Name = "Munich", CountryId = 5 },
                new City { Id = 23, Name = "Hamburg", CountryId = 5 },
                new City { Id = 24, Name = "Frankfurt", CountryId = 5 },
                new City { Id = 25, Name = "Cologne", CountryId = 5 },
                
                // Indonesia Provinces (Major Cities)
                new City { Id = 26, Name = "Jakarta", CountryId = 6 },
                new City { Id = 27, Name = "Surabaya", CountryId = 6 },
                new City { Id = 28, Name = "Bandung", CountryId = 6 },
                new City { Id = 29, Name = "Medan", CountryId = 6 },
                new City { Id = 30, Name = "Makassar", CountryId = 6 },
                new City { Id = 31, Name = "Semarang", CountryId = 6 },
                new City { Id = 32, Name = "Palembang", CountryId = 6 },
                new City { Id = 33, Name = "Tangerang", CountryId = 6 },
                new City { Id = 34, Name = "Depok", CountryId = 6 },
                new City { Id = 35, Name = "Yogyakarta", CountryId = 6 }
            );
        }
    }
} 