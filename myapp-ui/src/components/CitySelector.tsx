import React, { useEffect, useState } from 'react';
import { FormControl, InputLabel, Select, MenuItem, FormHelperText, CircularProgress, Box, SelectChangeEvent, Typography, Dialog, DialogContent } from '@mui/material';
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

  const handleCityChange = (event: SelectChangeEvent) => {
    const cityName = event.target.value;
    setSelectedCity(cityName);
    onCitySelect(cityName);
  };

  if (!countryCode) return null;

  return (
    <Box>
      {/* Loading dialog */}
      <Dialog open={loading} PaperProps={{ sx: { borderRadius: 2, p: 1, minWidth: 200 } }}>
        <DialogContent sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 2, p: 3 }}>
          <CircularProgress />
          <Typography variant="body1">Loading cities...</Typography>
        </DialogContent>
      </Dialog>
      
      <FormControl fullWidth variant="outlined" size="small" error={!!error} sx={{ mt: 1 }}>
        <InputLabel id="city-select-label" sx={{ 
          backgroundColor: 'white', 
          px: 0.5
        }}>
          City
        </InputLabel>
        <Select
          labelId="city-select-label"
          id="city-select"
          value={selectedCity}
          onChange={handleCityChange}
          displayEmpty
          disabled={cities.length === 0}
          sx={{ 
            "& .MuiSelect-select": { 
              paddingTop: 1.5, 
              paddingBottom: 1.5 
            } 
          }}
        >
          <MenuItem value="">
            <em>Select a city</em>
          </MenuItem>
          {cities.map((city, index) => (
            <MenuItem key={index} value={city.name}>
              {city.name}
            </MenuItem>
          ))}
        </Select>
        {error && <FormHelperText error>{error}</FormHelperText>}
      </FormControl>
    </Box>
  );
}; 