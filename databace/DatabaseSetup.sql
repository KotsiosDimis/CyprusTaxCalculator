-- Table: TaxBrackets
CREATE TABLE IF NOT EXISTS "TaxBrackets" (
    "Id" SERIAL PRIMARY KEY,
    "LowerLimit" NUMERIC(18,2) NOT NULL,
    "UpperLimit" NUMERIC(18,2),
    "Rate" NUMERIC(5,2) NOT NULL
);

-- Table: UserCalculationHistory (Optional for logs)
CREATE TABLE IF NOT EXISTS "UserCalculationHistory" (
    "Id" SERIAL PRIMARY KEY,
    "Timestamp" TIMESTAMP NOT NULL DEFAULT NOW(),
    "AnnualIncome" NUMERIC(18,2) NOT NULL,
    "LifeInsurancePremiums" NUMERIC(18,2),
    "OtherDeductions" NUMERIC(18,2),
    "TaxableIncome" NUMERIC(18,2),
    "TaxPayable" NUMERIC(18,2),
    "TaxSavings" NUMERIC(18,2)
);

-- Table: DeductionPolicy (for storing dynamic cap values)
CREATE TABLE IF NOT EXISTS "DeductionPolicy" (
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NOT NULL UNIQUE,                -- e.g., 'TotalDeductionCap'
    "CapPercentage" NUMERIC(5,2) NOT NULL,      -- e.g., 20.00
    "Description" TEXT
);

-- Seed: TaxBrackets (Cyprus 2025)
INSERT INTO "TaxBrackets" ("LowerLimit", "UpperLimit", "Rate") VALUES
    (0, 19500, 0.00),
    (19500.01, 28000, 20.00),
    (28000.01, 36300, 25.00),
    (36300.01, 60000, 30.00),
    (60000.01, NULL, 35.00)
ON CONFLICT ("LowerLimit", "UpperLimit", "Rate") DO NOTHING;

-- Seed: DeductionPolicy
INSERT INTO "DeductionPolicy" ("Name", "CapPercentage", "Description") VALUES
    ('TotalDeductionCap', 20.00, 'Max Life + Other deductions as % of income')
ON CONFLICT ("Name") DO NOTHING;