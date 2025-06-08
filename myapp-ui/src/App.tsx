import { useState } from 'react'
import './App.css'
import { CountrySelector } from './components/CountrySelector'
import { CitySelector } from './components/CitySelector'
import { WeatherDisplay } from './components/WeatherDisplay'

function App() {
  const [selectedCountryCode, setSelectedCountryCode] = useState('')
  const [selectedCityName, setSelectedCityName] = useState('')

  const handleCountrySelect = (countryCode: string) => {
    setSelectedCountryCode(countryCode)
    setSelectedCityName('')
  }

  const handleCitySelect = (cityName: string) => {
    setSelectedCityName(cityName)
  }

  return (
    <div className="container">
      <header>
        <h1>Weather App</h1>
      </header>

      <main>
        <div className="selectors">
          <CountrySelector onCountrySelect={handleCountrySelect} />
          
          {selectedCountryCode && (
            <CitySelector 
              countryCode={selectedCountryCode} 
              onCitySelect={handleCitySelect} 
            />
          )}
        </div>

        {selectedCityName && (
          <WeatherDisplay cityName={selectedCityName} />
        )}
      </main>
    </div>
  )
}

export default App 