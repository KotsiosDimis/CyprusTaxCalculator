using CyprusTaxCalculator.DAL.Entities;

namespace CyprusTaxCalculator.DAL.Data
{
    public static class SeedData
    {
        public static void Initialize(TaxDbContext context)
        {
            // Only seed if tables are empty
            if (!context.TaxBrackets.Any())
            {
                context.TaxBrackets.AddRange(
                    new TaxBracket { MinIncome = 0, MaxIncome = 19500, Rate = 0.00m },
                    new TaxBracket { MinIncome = 19501, MaxIncome = 28000, Rate = 0.20m },
                    new TaxBracket { MinIncome = 28001, MaxIncome = 36300, Rate = 0.25m },
                    new TaxBracket { MinIncome = 36301, MaxIncome = 60000, Rate = 0.30m },
                    new TaxBracket { MinIncome = 60001, MaxIncome = null, Rate = 0.35m }
                );
            }

            if (!context.DeductionRules.Any())
            {
                context.DeductionRules.Add(
                    new DeductionRule
                    {
                        DeductionType = "LifeInsurance",
                        LimitType = "Amount",
                        LimitValue = 1200m // Max €1200/year allowed
                    }
                );
            }

            context.SaveChanges();
        }
    }
}
