using CyprusTaxCalculator.DAL.Data;
using CyprusTaxCalculator.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CyprusTaxCalculator.DAL
{
    /// <summary>
    /// Provides initial data for the database, such as tax brackets and deduction rules.
    /// </summary>
    public static class SeedData
    {
        public static void Initialize(TaxDbContext context)
        {
            context.Database.Migrate();

            if (!context.TaxBrackets.Any())
            {
                context.TaxBrackets.AddRange(
                    new TaxBracket { MinIncome = 0, MaxIncome = 19500, Rate = 0m },
                    new TaxBracket { MinIncome = 19501, MaxIncome = 28000, Rate = 0.20m },
                    new TaxBracket { MinIncome = 28001, MaxIncome = 36300, Rate = 0.25m },
                    new TaxBracket { MinIncome = 36301, MaxIncome = 60000, Rate = 0.30m },
                    new TaxBracket { MinIncome = 60001, MaxIncome = null, Rate = 0.35m }
                );
            }

            if (!context.DeductionRules.Any())
                {
                    context.DeductionRules.AddRange(
                        new DeductionRule
                        {
                            DeductionType = "TotalCap",
                            LimitType = "Percentage",
                            LimitValue = 20.00m
                        }
                    );
                }

            context.SaveChanges();
        }
    }
}