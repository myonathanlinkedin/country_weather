# Country Weather App

A full-stack application using ASP.NET Core 9 Web API with React.js and TypeScript.

## Overview

This application allows users to:
1. Select a country from a dropdown list
2. Select a city in the chosen country
3. View weather details for that city

The weather data is retrieved from the OpenWeatherMap API, while country and city data is stored in an in-memory database.

## Project Structure

### Backend (.NET 9)

The backend follows a clean architecture with Domain-Driven Design principles:

- **WeatherApp.Api**: Entry point with controllers and API endpoints
- **WeatherApp.Application**: Business logic, interfaces, and DTOs
- **WeatherApp.Domain**: Core entities and value objects
- **WeatherApp.Infrastructure**: Repository implementations and API integrations
- **WeatherApp.Tests**: Unit tests for services, temperature conversion, and controllers

### Frontend (React + TypeScript)

The frontend is built using React with TypeScript, organized as:

- **components/**: UI components for country selection, city selection, and weather display
- **api/**: API client for communicating with the backend
- **types/**: TypeScript interfaces matching backend DTOs

## Database and External APIs

- **Entity Framework Core In-Memory Database**: Used to store and retrieve countries and cities data
- **OpenWeatherMap API**: Provides real-time weather data for the selected cities

### Countries
- United States (US)
- United Kingdom (UK)
- Japan (JP)
- Australia (AU)
- Germany (DE)

Each country has 5 major cities.

## How to Run

### Backend

1. Navigate to the project root
2. Update the OpenWeatherMap API key in `WeatherApp.Api/appsettings.json`
3. Build the solution:
   ```
   dotnet build
   ```
4. Run the API:
   ```
   cd WeatherApp.Api
   dotnet run
   ```
   
The API will be available at https://localhost:7274 and http://localhost:5274

### Frontend

1. Navigate to the frontend directory:
   ```
   cd myapp-ui
   ```
2. Install dependencies:
   ```
   npm install
   ```
3. Start the development server:
   ```
   npm run dev
   ```
   
The frontend will be available at http://localhost:5173

## API Endpoints

- `GET /api/countries`: Get list of all countries
- `GET /api/countries/{countryCode}/cities`: Get cities for a specific country
- `GET /api/weather/{cityName}`: Get weather data for a specific city from OpenWeatherMap

## Running Tests

To run the unit tests:

```
cd WeatherApp.Tests
dotnet test
```

The tests cover:
- Temperature conversion logic (F to C)
- Weather service success and failure cases (using mocked HTTP responses)
- Controller behavior with properly mocked dependencies

## Design Decisions

1. **In-Memory Database**: Uses Entity Framework Core's in-memory database provider for storing country and city data
2. **OpenWeatherMap Integration**: Real weather data from a third-party API
3. **Clean Architecture**: Ensures separation of concerns and testability
4. **DTOs**: Used to decouple external contracts from internal models
5. **CORS Configuration**: Set up to allow the React frontend to communicate with the API
6. **Type Safety**: TypeScript interfaces match C# DTOs for end-to-end type safety

## Future Enhancements

- Persistence layer with Entity Framework Core and SQL Server
- Weather data caching to reduce API calls
- User authentication and favorites
- Geolocation for automatic city detection
- Historical weather data and forecasts 