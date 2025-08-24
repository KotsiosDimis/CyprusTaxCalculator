# CyprusTaxCalculator.DAL

Data Access Layer for CyprusTaxCalculator.

## Responsibilities

- Manages database access and entities.
- Handles migrations and database setup.

## Key Components

- `Entities/`: Entity classes for database tables.
- `Migrations/`: Entity Framework Core migrations.
- [`DatabaseSetup.sql`](DatabaseSetup.sql): SQL script for initial database setup.

## Usage

1. Update your connection string in the UI project's `appsettings.json`.
2. Run migrations using Entity Framework Core CLI or Visual Studio.

## Dependencies

- Entity Framework Core
- .NET 9.0