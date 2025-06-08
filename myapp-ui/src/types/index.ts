export interface Country {
  name: string;
  code: string;
}

export interface City {
  name: string;
}

export interface Weather {
  cityName: string;
  countryName: string;
  time: string;
  windSpeed: number;
  windDirection: string;
  visibility: number;
  skyConditions: string;
  temperatureFahrenheit: number;
  temperatureCelsius: number;
  dewPoint: number;
  humidity: number;
  pressure: number;
} 