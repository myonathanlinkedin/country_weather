// Environment-specific configuration

interface Config {
  apiUrl: string;
}

// Default to development values
const config: Config = {
  apiUrl: import.meta.env.VITE_API_URL || 'http://localhost:5264/api',
};

export default config; 