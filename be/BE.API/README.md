# Electric Vehicle Warranty Management Backend

This project is an ASP.NET Core Web API for managing electric vehicle warranties. It provides:

- Vehicle and customer registration
- Part serial attachment to vehicles
- Service and warranty history tracking
- Warranty claim creation and status tracking

## Tech Stack
- ASP.NET Core Web API
- PostgreSQL

## Getting Started
1. Restore dependencies:
   ```powershell
   dotnet restore
   ```
2. Run the development server:
   ```powershell
   dotnet run
   ```

## Features
- Register vehicles by VIN
- Attach serial numbers of installed parts
- Store service & warranty history
- Create and track warranty claims

## Next Steps
- Implement PostgreSQL integration
- Add authentication and role-based access
- Connect frontend to backend API
