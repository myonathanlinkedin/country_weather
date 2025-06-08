import React, { useEffect, useState } from 'react';
import type { City } from '../types';
import { weatherApi } from '../api/weatherApi';

interface CitySelectorProps {
  countryCode: string;
  onCitySelect: (cityName: string) => void;
}

export const CitySelector: React.FC<CitySelectorProps> = ({ countryCode, onCitySelect }) => {
  const [cities, setCities] = useState<City[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [selectedCity, setSelectedCity] = useState<string>('');

  useEffect(() => {
    if (!countryCode) {
      setCities([]);
      setSelectedCity('');
      return;
    }

    const fetchCities = async () => {
      setLoading(true);
      try {
        const data = await weatherApi.getCities(countryCode);
        setCities(data);
        setLoading(false);
      } catch (err) {
        setError('Failed to load cities. Please try again later.');
        setLoading(false);
      }
    };

    fetchCities();
  }, [countryCode]);

  const handleCityChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const cityName = e.target.value;
    setSelectedCity(cityName);
    onCitySelect(cityName);
  };

  if (!countryCode) return null;
  if (loading) return <div>Loading cities...</div>;
  if (error) return <div className="error">{error}</div>;

  return (
    <div className="city-selector">
      <h2>Select a City</h2>
      <select 
        value={selectedCity} 
        onChange={handleCityChange}
        className="form-select"
        disabled={cities.length === 0}
      >
        <option value="">-- Select a city --</option>
        {cities.map((city, index) => (
          <option key={index} value={city.name}>
            {city.name}
          </option>
        ))}
      </select>
    </div>
  );
}; 