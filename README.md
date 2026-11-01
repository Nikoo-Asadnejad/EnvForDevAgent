# DevAgentEnv API

A minimal ASP.NET Core Web API targeting .NET 10.

This is a test environment for DevAgent tests.

## Requirements

- .NET SDK 10.0 or later

## Run locally

```bash
dotnet restore
dotnet run
```

The API is available at:

- `http://localhost:5054`
- `https://localhost:7121`

## Endpoints

- `GET /weatherforecast` returns sample weather data.
- `GET /weatherforecast/{country}` returns one random forecast for tomorrow with
  `country`, `date`, `temperatureC`, `temperatureF`, and `summary` fields. For
  example, use `/weatherforecast/Canada` or `/weatherforecast/United%20Kingdom`.
  Country names are trimmed; blank names return HTTP 400. Any nonblank name is
  accepted. Forecasts are simulated on each request using the same temperature
  range and summaries as the original endpoint, not actual country weather.
- `GET /openapi/v1.json` exposes the OpenAPI document in the Development environment.

## Build

```bash
dotnet build
```

## Tests

```bash
dotnet test tests/DevAgentEnv.Tests/DevAgentEnv.Tests.csproj
```
