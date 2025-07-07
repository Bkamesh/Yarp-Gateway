
# Yarp-Gateway

This project is a simple reverse proxy built using YARP (Yet Another Reverse Proxy) for .NET.

## Project Structure

- `Program.cs` - Entry point of the application
- `appsettings.json` - Main configuration file
- `appsettings.Development.json` - Development-specific configuration
- `appsettings.QA.json` - QA environment configuration
- `Dockerfile` - For building a Docker image
- `Yarp-Gateway.csproj` - Project file

## Requirements

- .NET 8.0 SDK or higher
- Docker (optional)

## How to Run

### Run Locally

```
dotnet run
```

### Build and Run with Docker

```
docker build -t yarp-gateway .
docker run -p 8080:80 yarp-gateway
```

## Notes

- Update `appsettings.json` files to configure routes and clusters for the proxy.
- YARP handles routing traffic to backend services.
