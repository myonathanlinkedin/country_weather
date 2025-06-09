import React, { useEffect, useState } from 'react';
import { FormControl, InputLabel, Select, MenuItem, FormHelperText, CircularProgress, Box, SelectChangeEvent, Typography, Dialog, DialogContent } from '@mui/material';
import type { Country } from '../types';
import { weatherApi } from '../api/weatherApi';
import config from '../config';

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
        console.log(`Fetching countries from: ${config.apiUrl}/countries`);
        const data = await weatherApi.getCountries();
        console.log('Countries fetched successfully:', data);
        setCountries(data);
        setLoading(false);
      } catch (err: any) {
        console.error('Error fetching countries:', err);
        const errorMessage = err.response ? 
          `Server error: ${err.response.status} ${err.response.statusText}` : 
          'Failed to load countries. Please try again later.';
        setError(errorMessage);
        setLoading(false);
      }
    };

    fetchCountries();
  }, []);

  const handleCountryChange = (event: SelectChangeEvent) => {
    const countryCode = event.target.value;
    setSelectedCountry(countryCode);
    onCountrySelect(countryCode);
  };

  return (
    <Box>
      {/* Loading dialog */}
      <Dialog open={loading} PaperProps={{ sx: { borderRadius: 2, p: 1, minWidth: 200 } }}>
        <DialogContent sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 2, p: 3 }}>
          <CircularProgress />
          <Typography variant="body1">Loading countries...</Typography>
        </DialogContent>
      </Dialog>
      
      <FormControl fullWidth variant="outlined" size="small" error={!!error} sx={{ mt: 1 }}>
        <InputLabel id="country-select-label" sx={{ 
          backgroundColor: 'white', 
          px: 0.5
        }}>
          Country
        </InputLabel>
        <Select
          labelId="country-select-label"
          id="country-select"
          value={selectedCountry}
          onChange={handleCountryChange}
          displayEmpty
          sx={{ 
            "& .MuiSelect-select": { 
              paddingTop: 1.5, 
              paddingBottom: 1.5 
            } 
          }}
        >
          <MenuItem value="">
            <em>Select a country</em>
          </MenuItem>
          {countries.map((country) => (
            <MenuItem key={country.code} value={country.code}>
              {country.name}
            </MenuItem>
          ))}
        </Select>
        {error && <FormHelperText error>{error}</FormHelperText>}
      </FormControl>
    </Box>
  );
}; 