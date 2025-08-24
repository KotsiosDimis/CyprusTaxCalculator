-- Table: TaxBrackets
CREATE TABLE IF NOT EXISTS "TaxBrackets" (
    "Id" SERIAL PRIMARY KEY,
    "LowerLimit" NUMERIC(18,2) NOT NULL,
    "UpperLimit" NUMERIC(18,2),
    "Rate" NUMERIC(5,2) NOT NULL
);

-- Table: DeductionRules
CREATE TABLE IF NOT EXISTS "DeductionRules" (
    "Id" SERIAL PRIMARY KEY,
    "Type" VARCHAR(100) NOT NULL,
    "Description" VARCHAR(255),
    "MaxAmount" NUMERIC(18,2) NOT NULL
);

-- (Optional) Table: UserCalculationHistory
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

-- Add unique constraints for safe seeding
ALTER TABLE "TaxBrackets" ADD CONSTRAINT IF NOT EXISTS uq_taxbrackets_limits UNIQUE ("LowerLimit", "UpperLimit", "Rate");
ALTER TABLE "DeductionRules" ADD CONSTRAINT IF NOT EXISTS uq_deductionrules_type UNIQUE ("Type");

-- Seed: TaxBrackets
INSERT INTO "TaxBrackets" ("LowerLimit", "UpperLimit", "Rate") VALUES
    (0, 19500, 0.00),
    (19500, 28000, 20.00),
    (28000, 36300, 25.00),
    (36300, 60000, 30.00),
    (60000, NULL, 35.00)
ON CONFLICT ("LowerLimit", "UpperLimit", "Rate") DO NOTHING;

-- Seed: DeductionRules
INSERT INTO "DeductionRules" ("Type", "Description", "MaxAmount") VALUES
    ('LifeInsurance', 'Life insurance premium deduction', 7000.00),
    ('Other', 'Other deductible expenses', 5000.00)
ON CONFLICT