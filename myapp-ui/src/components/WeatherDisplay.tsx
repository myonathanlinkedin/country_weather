import React, { useEffect, useState } from 'react';
import { Paper, Typography, Grid, Box, Divider, CircularProgress, Alert, Chip, Dialog, DialogContent } from '@mui/material';
import ThermostatIcon from '@mui/icons-material/Thermostat';
import WaterDropIcon from '@mui/icons-material/WaterDrop';
import AirIcon from '@mui/icons-material/Air';
import CompressIcon from '@mui/icons-material/Compress';
import VisibilityIcon from '@mui/icons-material/Visibility';
import CloudIcon from '@mui/icons-material/Cloud';
import AccessTimeIcon from '@mui/icons-material/AccessTime';
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
        setError(null);
      } catch (err) {
        setError('Could not retrieve weather data. City may not exist or there was a connection issue.');
        setLoading(false);
        setWeather(null);
      }
    };

    fetchWeather();
  }, [cityName]);

  if (!cityName) return null;

  return (
    <>
      {/* Loading dialog */}
      <Dialog open={loading} PaperProps={{ sx: { borderRadius: 2, p: 1, minWidth: 200 } }}>
        <DialogContent sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 2, p: 3 }}>
          <CircularProgress />
          <Typography variant="body1">Loading weather data...</Typography>
        </DialogContent>
      </Dialog>
      
      {error && (
        <Paper sx={{ p: 3 }}>
          <Alert severity="error">{error}</Alert>
        </Paper>
      )}
      
      {!loading && !error && weather && (
        <Paper sx={{ p: 3 }}>
          <Box sx={{ mb: 3 }}>
            <Typography variant="h4" gutterBottom>
              {weather.cityName}, {weather.countryName}
            </Typography>
            <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
              <AccessTimeIcon fontSize="small" sx={{ mr: 1, color: 'text.secondary' }} />
              <Typography variant="body2" color="text.secondary">
                {weather.time}
              </Typography>
            </Box>
          </Box>

          <Divider sx={{ my: 2 }} />

          {/* Main weather indicators */}
          <Box sx={{ mb: 3 }}>
            <Grid container spacing={2}>
              <Grid item xs={12} sm={6}>
                <Box sx={{ display: 'flex', alignItems: 'center' }}>
                  <ThermostatIcon fontSize="large" sx={{ mr: 1, color: 'primary.main' }} />
                  <Box>
                    <Typography variant="h5">
                      {weather.temperatureFahrenheit.toFixed(1)}°F
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      {weather.temperatureCelsius.toFixed(1)}°C
                    </Typography>
                  </Box>
                </Box>
              </Grid>
              <Grid item xs={12} sm={6}>
                <Box sx={{ display: 'flex', alignItems: 'center' }}>
                  <CloudIcon fontSize="large" sx={{ mr: 1, color: 'primary.main' }} />
                  <Box>
                    <Typography variant="h5">
                      {weather.skyConditions}
                    </Typography>
                    <Chip 
                      label="Current Conditions" 
                      size="small" 
                      variant="outlined"
                      sx={{ mt: 0.5 }}
                    />
                  </Box>
                </Box>
              </Grid>
            </Grid>
          </Box>

          <Divider sx={{ my: 2 }} />

          {/* Detailed weather data */}
          <Typography variant="h6" gutterBottom sx={{ mb: 2 }}>
            Weather Details
          </Typography>
          
          <Grid container spacing={2}>
            <Grid item xs={12} sm={6} md={4}>
              <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                <AirIcon sx={{ mr: 1, color: 'text.secondary' }} />
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Wind
                  </Typography>
                  <Typography variant="body1">
                    {weather.windSpeed.toFixed(1)} mph from {weather.windDirection}
                  </Typography>
                </Box>
              </Box>
            </Grid>
            
            <Grid item xs={12} sm={6} md={4}>
              <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                <VisibilityIcon sx={{ mr: 1, color: 'text.secondary' }} />
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Visibility
                  </Typography>
                  <Typography variant="body1">
                    {weather.visibility.toFixed(1)} miles
                  </Typography>
                </Box>
              </Box>
            </Grid>
            
            <Grid item xs={12} sm={6} md={4}>
              <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                <WaterDropIcon sx={{ mr: 1, color: 'text.secondary' }} />
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Humidity
                  </Typography>
                  <Typography variant="body1">
                    {weather.humidity.toFixed(0)}%
                  </Typography>
                </Box>
              </Box>
            </Grid>
            
            <Grid item xs={12} sm={6} md={4}>
              <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                <CompressIcon sx={{ mr: 1, color: 'text.secondary' }} />
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Pressure
                  </Typography>
                  <Typography variant="body1">
                    {weather.pressure.toFixed(0)} hPa
                  </Typography>
                </Box>
              </Box>
            </Grid>
            
            <Grid item xs={12} sm={6} md={4}>
              <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                <WaterDropIcon sx={{ mr: 1, color: 'text.secondary' }} />
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Dew Point
                  </Typography>
                  <Typography variant="body1">
                    {weather.dewPoint.toFixed(1)}°F
                  </Typography>
                </Box>
              </Box>
            </Grid>
          </Grid>
        </Paper>
      )}
    </>
  );
}; 