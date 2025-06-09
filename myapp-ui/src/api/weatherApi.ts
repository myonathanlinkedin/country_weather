import axios from 'axios';
import type { City, Country, Weather } from '../types';
import config from '../config';

// Configure axios defaults
const api = axios.create({
  baseURL: config.apiUrl,
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  }
});

export const weatherApi = {
  getCountries: async (): Promise<Country[]> => {
    console.log(`API call: GET ${config.apiUrl}/countries`);
    try {
      const response = await api.get<Country[]>('/countries');
      console.log('Countries API response:', response);
      return response.data;
    } catch (error) {
      console.error('Countries API error:', error);
      throw error;
    }
  },
  
  getCities: async (countryCode: string): Promise<City[]> => {
    try {
      const response = await api.get<City[]>(`/countries/${countryCode}/cities`);
      return response.data;
    } catch (error) {
      console.error(`Cities API error for country ${countryCode}:`, error);
      throw error;
    }
  },
  
  getWeather: async (cityName: string): Promise<Weather> => {
    try {
      const response = await api.get<Weather>(`/weather/${cityName}`);
      return response.data;
    } catch (error) {
      console.error(`Weather API error for city ${cityName}:`, error);
      throw error;
    }
  }
}; 