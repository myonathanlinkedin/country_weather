import { useState } from 'react'
import { Container, Box, Paper, Typography, ThemeProvider, createTheme, CssBaseline, AppBar, Toolbar, Grid } from '@mui/material';
import { blue, grey } from '@mui/material/colors';
import LocationCityIcon from '@mui/icons-material/LocationCity';
import { CountrySelector } from './components/CountrySelector'
import { CitySelector } from './components/CitySelector'
import { WeatherDisplay } from './components/WeatherDisplay'
import { SearchBar } from './components/SearchBar';

// Create LinkedIn-style theme
const theme = createTheme({
  palette: {
    primary: {
      main: blue[700],
    },
    secondary: {
      main: grey[800],
    },
    background: {
      default: '#f5f5f5',
    }
  },
  typography: {
    fontFamily: "'Segoe UI', 'Open Sans', sans-serif",
    h4: {
      fontWeight: 600,
    },
    h5: {
      fontWeight: 600,
    }
  },
  components: {
    MuiPaper: {
      styleOverrides: {
        root: {
          boxShadow: '0px 1px 3px 0px rgba(0,0,0,0.1)'
        }
      }
    }
  }
});

function App() {
  const [selectedCountryCode, setSelectedCountryCode] = useState('')
  const [selectedCityName, setSelectedCityName] = useState('')
  const [searchQuery, setSearchQuery] = useState('')

  const handleCountrySelect = (countryCode: string) => {
    setSelectedCountryCode(countryCode)
    setSelectedCityName('')
  }

  const handleCitySelect = (cityName: string) => {
    setSelectedCityName(cityName)
  }

  const handleSearch = (query: string) => {
    setSearchQuery(query)
    // For direct city search without country selection
    if (query.trim() !== '') {
      setSelectedCityName(query)
    }
  }

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      
      <AppBar position="static" color="primary" elevation={0}>
        <Toolbar>
          <LocationCityIcon sx={{ mr: 2 }} />
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Weather App
          </Typography>
        </Toolbar>
      </AppBar>

      <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
        <Grid container spacing={3}>
          {/* Left Column: Selection Controls */}
          <Grid item xs={12} md={4}>
            <Paper sx={{ p: 3, mb: 3 }}>
              <Typography variant="h5" gutterBottom>
                Search Weather
              </Typography>
              
              <Box sx={{ mb: 3 }}>
                <SearchBar onSearch={handleSearch} />
              </Box>
              
              <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                Or select from available locations:
              </Typography>
              
              <CountrySelector onCountrySelect={handleCountrySelect} />
              
              {selectedCountryCode && (
                <Box sx={{ mt: 2 }}>
                  <CitySelector 
                    countryCode={selectedCountryCode} 
                    onCitySelect={handleCitySelect} 
                  />
                </Box>
              )}
            </Paper>
          </Grid>

          {/* Right Column: Weather Display */}
          <Grid item xs={12} md={8}>
            {selectedCityName ? (
              <WeatherDisplay cityName={selectedCityName} />
            ) : (
              <Paper sx={{ p: 3, height: '100%', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                <Box sx={{ textAlign: 'center', p: 3 }}>
                  <Typography variant="h5" color="text.secondary" gutterBottom>
                    Welcome to Weather App
                  </Typography>
                  <Typography variant="body1" color="text.secondary">
                    Search for a city or select a country and city to view weather details.
                  </Typography>
                </Box>
              </Paper>
            )}
          </Grid>
        </Grid>
        
        <Box sx={{ mt: 4, textAlign: 'center', color: 'text.secondary' }}>
          <Typography variant="body2">
            © {new Date().getFullYear()} Weather Application by Mateus Yonathan
          </Typography>
        </Box>
      </Container>
    </ThemeProvider>
  )
}

export default App 