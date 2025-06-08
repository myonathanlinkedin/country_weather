import React, { useEffect, useState } from 'react';
import type { Weather } from '../types';
import { weatherApi } from '../api/weatherApi';

interface WeatherDisplayProps {
  cityName: string;
}

export const WeatherDisplay: React.FC<WeatherDisplayProps> = ({ cityName }) => {
  const [weather, setWeather] = useState<Weather | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!cityName) {
      setWeather(null);
      return;
    }

    const fetchWeather = async () => {
      setLoading(true);
      try {
        const data = await weatherApi.getWeather(cityName);
        setWeather(data);
        setLoading(false);
      } catch (err) {
        setError('Failed to load weather data. Please try again later.');
        setLoading(false);
      }
    };

    fetchWeather();
  }, [cityName]);

  if (!cityName) return null;
  if (loading) return <div>Loading weather data...</div>;
  if (error) return <div className="error">{error}</div>;
  if (!weather) return null;

  return (
    <div className="weather-display">
      <h2>Weather Details</h2>
      <div className="weather-card">
        <div className="weather-header">
          <h3>{weather.cityName}, {weather.countryName}</h3>
          <div className="time">{weather.time}</div>
        </div>
        
        <div className="weather-body">
          <div className="weather-row">
            <div className="weather-label">Sky Conditions:</div>
            <div className="weather-value">{weather.skyConditions}</div>
          </div>
          <div className="weather-row">
            <div className="weather-label">Temperature:</div>
            <div className="weather-value">
              {weather.temperatureFahrenheit.toFixed(1)}°F / {weather.temperatureCelsius.toFixed(1)}°C
            </div>
          </div>
          <div className="weather-row">
            <div className="weather-label">Wind:</div>
            <div className="weather-value">
              {weather.windSpeed.toFixed(1)} mph from {weather.windDirection}
            </div>
          </div>
          <div className="weather-row">
            <div className="weather-label">Visibility:</div>
            <div className="weather-value">{weather.visibility.toFixed(1)} miles</div>
          </div>
          <div className="weather-row">
            <div className="weather-label">Dew Point:</div>
            <div className="weather-value">{weather.dewPoint.toFixed(1)}°F</div>
          </div>
          <div className="weather-row">
            <div className="weather-label">Humidity:</div>
            <div className="weather-value">{weather.humidity.toFixed(1)}%</div>
          </div>
          <div className="weather-row">
            <div className="weather-label">Pressure:</div>
            <div className="weather-value">{weather.pressure.toFixed(1)} hPa</div>
          </div>
        </div>
      </div>
    </div>
  );
}; 