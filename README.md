# CyprusTaxCalculator

A web application for calculating Cyprus income tax, including deductions and savings, with calculation history tracking.

## Solution Structure

- [`CyprusTaxCalculator.BLL`](CyprusTaxCalculator.BLL/): Business logic and tax calculation services.
- [`CyprusTaxCalculator.DAL`](CyprusTaxCalculator.DAL/): Data access layer, including database entities and migrations.
- [`CyprusTaxCalculator.UI`](CyprusTaxCalculator.UI/): ASP.NET Core Razor Pages web frontend.

## Getting Started

1. Clone the repository.
2. Open `CyprusTaxCalculator.sln` in Visual Studio.
3. Build the solution.
4. Update database connection strings as needed in [`CyprusTaxCalculator.UI/appsettings.json`](CyprusTaxCalculator.UI/appsettings.json).
5. Run database migrations (see DAL README).
6. Start the application from the UI project.

## Requirements

- .NET 9.0 SDK
- PostgreSQL or compatible database

## License

MIT License