import React, { useEffect, useState } from 'react';
import type { Country } from '../types';
import { weatherApi } from '../api/weatherApi';

interface CountrySelectorProps {
  onCountrySelect: (countryCode: string) => void;
}

export const CountrySelector: React.FC<CountrySelectorProps> = ({ onCountrySelect }) => {
  const [countries, setCountries] = useState<Country[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedCountry, setSelectedCountry] = useState<string>('');

  useEffect(() => {
    const fetchCountries = async () => {
      try {
        const data = await weatherApi.getCountries();
        setCountries(data);
        setLoading(false);
      } catch (err) {
        setError('Failed to load countries. Please try again later.');
        setLoading(false);
      }
    };

    fetchCountries();
  }, []);

  const handleCountryChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const countryCode = e.target.value;
    setSelectedCountry(countryCode);
    onCountrySelect(countryCode);
  };

  if (loading) return <div>Loading countries...</div>;
  if (error) return <div className="error">{error}</div>;

  return (
    <div className="country-selector">
      <h2>Select a Country</h2>
      <select 
        value={selectedCountry} 
        onChange={handleCountryChange}
        className="form-select"
      >
        <option value="">-- Select a country --</option>
        {countries.map((country) => (
          <option key={country.code} value={country.code}>
            {country.name}
          </option>
        ))}
      </select>
    </div>
  );
}; 