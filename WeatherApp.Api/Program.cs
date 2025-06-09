using Microsoft.EntityFrameworkCore;
using WeatherApp.Application.Interfaces;
using WeatherApp.Infrastructure.Configuration;
using WeatherApp.Infrastructure.Data;
using WeatherApp.Infrastructure.Repositories;
using WeatherApp.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add in-memory database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("WeatherAppDb"));

// Add configuration for OpenWeatherMap
builder.Services.Configure<OpenWeatherMapSettings>(
    builder.Configuration.GetSection("OpenWeatherMap"));

// Add repositories and services
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IWeatherService, OpenWeatherMapService>();

// Add HttpClient for OpenWeatherMap API integration
builder.Services.AddHttpClient<OpenWeatherMapService>();

// Add CORS - Allow all origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Initialize and seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS - before routing
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
