FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file and project files first to optimize build caching
COPY ["WeatherApp.Api/WeatherApp.Api.csproj", "WeatherApp.Api/"]
COPY ["WeatherApp.Application/WeatherApp.Application.csproj", "WeatherApp.Application/"]
COPY ["WeatherApp.Domain/WeatherApp.Domain.csproj", "WeatherApp.Domain/"]
COPY ["WeatherApp.Infrastructure/WeatherApp.Infrastructure.csproj", "WeatherApp.Infrastructure/"]
COPY ["WeatherApp.sln", "./"]

# Restore dependencies
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet build "WeatherApp.Api/WeatherApp.Api.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "WeatherApp.Api/WeatherApp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "WeatherApp.Api.dll"] 