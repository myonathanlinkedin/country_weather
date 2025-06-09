# Country Weather App

A full-stack application using ASP.NET Core 9 Web API with React.js and TypeScript.

## Overview

This application allows users to:
1. Select a country from a dropdown list
2. Select a city in the chosen country
3. View weather details for that city

The weather data is retrieved from the OpenWeatherMap API, while country and city data is stored in an in-memory database.

## Complete Project Structure

### Backend Structure (.NET 9)

```
country_weather/
├── WeatherApp.Api/                    # API entry point
│   ├── Controllers/                   # API controllers
│   ├── DTOs/                          # Data Transfer Objects
│   ├── Program.cs                     # Application startup
│   ├── appsettings.json               # Configuration
│   └── Dockerfile                     # Container definition
│
├── WeatherApp.Application/            # Application layer
│   ├── Contracts/                     # DTOs and transfer objects
│   ├── Interfaces/                    # Service interfaces
│   └── Exceptions/                    # Custom exceptions
│
├── WeatherApp.Domain/                 # Domain layer
│   └── Entities/                      # Domain entities
│
├── WeatherApp.Infrastructure/         # Infrastructure layer
│   ├── Configuration/                 # External API configuration
│   ├── Data/                          # Database context
│   │   └── AppDbContext.cs            # EF Core context
│   ├── Repositories/                  # Data access implementation
│   └── Services/                      # External service integration
│
└── WeatherApp.Tests/                  # Unit & integration tests
    ├── Controllers/                   # Controller tests
    └── Services/                      # Service tests
```

### Frontend Structure (React + TypeScript)

```
myapp-ui/
├── public/                            # Static assets
├── src/
│   ├── api/                           # API client
│   │   └── weatherApi.ts              # Backend API integration
│   ├── components/                    # UI components
│   │   ├── CountrySelector.tsx        # Country dropdown
│   │   ├── CitySelector.tsx           # City dropdown
│   │   ├── WeatherDisplay.tsx         # Weather information display
│   │   └── SearchBar.tsx              # Direct city search
│   ├── config/                        # Configuration
│   │   └── index.ts                   # Environment settings
│   ├── types/                         # TypeScript types
│   │   └── index.ts                   # Interface definitions
│   ├── App.tsx                        # Main component
│   └── main.tsx                       # Application entry point
├── package.json                       # Dependencies & scripts
└── tsconfig.json                      # TypeScript configuration
```

## Database and External APIs

- **Entity Framework Core In-Memory Database**: Used to store and retrieve countries and cities data
- **OpenWeatherMap API**: Provides real-time weather data for the selected cities

### Countries
- United States (US)
- United Kingdom (UK)
- Japan (JP)
- Australia (AU)
- Germany (DE)
- Indonesia (ID)

Each country has multiple major cities. Indonesia specifically includes 10 major cities/provinces.

## Complete Guide to Run the Application

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (v18+ recommended)
- [npm](https://www.npmjs.com/) (v8+ recommended)

### Backend Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/country_weather.git
   cd country_weather
   ```

2. Update the OpenWeatherMap API key in `WeatherApp.Api/appsettings.json`:
   ```json
   {
     "OpenWeatherMap": {
       "ApiKey": "b8ebf0e8c123cec78eed69a900786867",
       "ApiUrl": "https://api.openweathermap.org/data/2.5"
     }
   }
   ```

3. Build the solution:
   ```bash
   dotnet build
   ```

4. Run the API:
   ```bash
   cd WeatherApp.Api
   dotnet run
   ```
   
5. Verify the API is running:
   - API endpoints will be available at https://localhost:7274 and http://localhost:5274
   - You can test the API via Swagger UI at https://localhost:7274/swagger

### Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd myapp-ui
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Configure environment (optional):
   - Create a `.env` file in the `myapp-ui` directory if you need to customize the API URL:
     ```
     VITE_API_URL=http://localhost:5274
     ```

4. Start the development server:
   ```bash
   npm run dev
   ```
   
5. Access the application:
   - The frontend will be available at http://localhost:5173 (or another port if 5173 is in use)
   - You should see the Weather App interface with country and city dropdowns

### Running in Production Mode

#### Backend:

```bash
cd WeatherApp.Api
dotnet publish -c Release
```

The published files will be in `WeatherApp.Api/bin/Release/net9.0/publish/`

#### Frontend:

```bash
cd myapp-ui
npm run build
```

The built files will be in the `myapp-ui/dist/` directory, which can be served with any static file server.

### Docker Support

The application includes a Dockerfile for containerizing the API:

```bash
cd WeatherApp.Api
docker build -t weather-app-api .
docker run -p 8080:80 weather-app-api
```

## API Endpoints

- `GET /api/countries`: Get list of all countries
- `GET /api/countries/{countryCode}/cities`: Get cities for a specific country
- `GET /api/weather/{cityName}`: Get weather data for a specific city from OpenWeatherMap

## Running Tests

To run the unit tests:

```bash
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

## Troubleshooting

- **API Connection Issues**: Ensure CORS is properly configured in the API and that the frontend is pointing to the correct API URL
- **Missing Weather Data**: Verify that the OpenWeatherMap API key is valid and correctly set in appsettings.json
- **City Not Found**: Some city names might not match exactly what OpenWeatherMap expects. Try searching for a major city

### 📜 License - Apache License 2.0 (TL;DR)

This project follows the **Apache License 2.0**, which means:

- ✅ **You can** use, modify, and distribute the code freely.  
- ✅ **You must** include the original license when distributing.  
- ✅ **You must** include the `NOTICE` file if one is provided.  
- ✅ **You can** use this in personal & commercial projects.  
- ✅ **No warranties** – use at your own risk! 🚀  

For full details, check the [Apache License 2.0](http://www.apache.org/licenses/LICENSE-2.0).  

## Screenshots 

![image](https://github.com/user-attachments/assets/6e254ff5-5301-4457-b231-db7dbe932d10)
