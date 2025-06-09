# Weather App UI

This is the frontend for the Weather App application, built with React, TypeScript, and Material UI.

## Getting Started

### Prerequisites

- Node.js 16+ 
- npm 7+

### Installation

1. Clone the repository
2. Navigate to the UI directory: `cd myapp-ui`
3. Install dependencies: `npm install`

### Configuration

The application uses environment variables for configuration. You can set these in several ways:

#### Method 1: Create a `.env.local` file (recommended for development)

Create a `.env.local` file in the root of the project:

```
VITE_API_URL=http://localhost:5264/api
```

This file will not be committed to source control, allowing for personalized configuration.

#### Method 2: Set environment variables directly

Set the variables before starting the app:

```bash
# On Windows
set VITE_API_URL=http://localhost:5264/api && npm run dev

# On Linux/Mac
VITE_API_URL=http://localhost:5264/api npm run dev
```

#### Method 3: Create environment-specific files

For different environments, create:
- `.env.development` - Used during development
- `.env.production` - Used for production builds

### Available Environment Variables

| Variable      | Description                    | Default Value            |
|---------------|--------------------------------|--------------------------|
| VITE_API_URL  | The base URL for the API       | http://localhost:5264/api |

### Running the Application

Start the development server:

```bash
npm run dev
```

The application will be available at http://localhost:5173 (or another port if 5173 is in use).

### Building for Production

```bash
npm run build
```

The built files will be in the `dist` directory.

## Folder Structure

- `src/api` - API client code
- `src/components` - React components
- `src/config` - Application configuration
- `src/types` - TypeScript type definitions 