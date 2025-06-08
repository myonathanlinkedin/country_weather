import axios from 'axios';
import type { City, Country, Weather } from '../types';

const API_URL = 'https://localhost:7274/api';

export const weatherApi = {
  getCountries: async (): Promise<Country[]> => {
    const response = await axios.get<Country[]>(`${API_URL}/countries`);
    return response.data;
  },
  
  getCities: async (countryCode: string): Promise<City[]> => {
    const response = await axios.get<City[]>(`${API_URL}/countries/${countryCode}/cities`);
    return response.data;
  },
  
  getWeather: async (cityName: string): Promise<Weather> => {
    const response = await axios.get<Weather>(`${API_URL}/weather/${cityName}`);
    return response.data;
  }
}; 