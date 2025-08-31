# Cyprus Tax Calculator – Business Logic Layer (BLL)

This project handles the tax calculation logic based on the Cyprus 2025 tax system. It includes:

- Deduction rules (life insurance, other deductions)
- Progressive tax brackets
- Clean separation of concerns
- Fully testable service

## Features

- 20% cap on total deductions
- Calculation of taxable income, total tax, and tax savings
- Uses EF Core to fetch tax brackets

## Folder Structure

- `Models/` – Contains domain models like `TaxBracketTax`
- `Services/` – Contains the `TaxCalculatorService` class

## Requirements

- .NET 9.0
- EF Core
- Connected to `CyprusTaxCalculator.DAL` for DB access