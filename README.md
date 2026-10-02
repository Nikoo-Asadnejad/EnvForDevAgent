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
- `GET /openapi/v1.json` exposes the OpenAPI document in the Development environment.

## Build

```bash
dotnet build
```
