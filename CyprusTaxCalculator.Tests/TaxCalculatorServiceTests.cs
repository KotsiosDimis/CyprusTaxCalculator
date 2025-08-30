using Xunit;
using CyprusTaxCalculator.BLL.Services;
using CyprusTaxCalculator.DAL.Entities;
using CyprusTaxCalculator.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CyprusTaxCalculator.Tests
{
    public class TaxCalculatorServiceTests
    {
        private TaxCalculatorService CreateService()
        {
            var options = new DbContextOptionsBuilder<TaxDbContext>()
                .UseInMemoryDatabase(databaseName: "TaxTestDb_" + Guid.NewGuid())
                .Options;

            var context = new TaxDbContext(options);

            // Seed tax brackets
            context.TaxBrackets.AddRange(new List<TaxBracket>
            {
                new TaxBracket { MinIncome = 0, MaxIncome = 19500, Rate = 0 },
                new TaxBracket { MinIncome = 19501, MaxIncome = 28000, Rate = 0.20m },
                new TaxBracket { MinIncome = 28001, MaxIncome = 36300, Rate = 0.25m },
                new TaxBracket { MinIncome = 36301, MaxIncome = 60000, Rate = 0.30m },
                new TaxBracket { MinIncome = 60001, MaxIncome = null, Rate = 0.35m }
            });

            // Seed life insurance rule (max €1200)
            context.DeductionRules.Add(new DeductionRule
            {
                DeductionType = "LifeInsurance",
                LimitType = "Amount",
                LimitValue = 1200m
            });

            context.SaveChanges();

            return new TaxCalculatorService(context);
        }

        [Fact]
        public void ZeroIncome_ReturnsZeroTax()
        {
            var service = CreateService();
            var result = service.CalculateTax(0, 0, 0);

            Assert.Equal(0, result.taxPayable);
            Assert.Equal(0, result.taxableIncome);
            Assert.Equal(0, result.savings);
        }

        [Fact]
        public void MaxDeductionAppliedProperly()
        {
            var service = CreateService();
            decimal income = 30000;
            decimal lifeInsurance = 5000; // More than 1200 cap

            var result = service.CalculateTax(income, lifeInsurance, 0);

            decimal expectedTaxable = income - 1200; // life deduction capped
            Assert.Equal(expectedTaxable, result.taxableIncome);

            // Manual Tax: [19501-28000]=8499*0.20=1699.80, [28001-28800]=800*0.25=200 → Total = 1899.80
            Assert.Equal(1899.80m, Math.Round(result.taxPayable, 2));
        }

        [Fact]
        public void NoDeductionsProvided()
        {
            var service = CreateService();
            decimal income = 30000;
            var result = service.CalculateTax(income, 0, 0);

            Assert.Equal(income, result.taxableIncome);
            Assert.True(result.taxPayable > 0);
            Assert.Equal(0, result.savings);
        }

        [Fact]
        public void HighIncome_CorrectBracketApplied()
        {
            var service = CreateService();
            decimal income = 75000;
            var result = service.CalculateTax(income, 0, 0);

            Assert.True(result.taxableIncome > 60000);
            Assert.True(result.taxPayable > 0);
        }

        [Fact]
        public void MultipleDeductions_CorrectSavings()
        {
            var service = CreateService();
            decimal income = 50000;
            decimal lifeInsurance = 5000; // capped at 1200
            decimal other = 2000;

            var result = service.CalculateTax(income, lifeInsurance, other);

            decimal expectedTaxable = income - 1200 - 2000; // = 46800
            Assert.Equal(expectedTaxable, result.taxableIncome);

            // Tax: [19501–28000]=8499*0.20=1699.80
            //      [28001–36300]=8299*0.25=2074.75
            //      [36301–46800]=10499*0.30=3149.70
            // Total = 6924.25
            Assert.Equal(6924.25m, Math.Round(result.taxPayable, 2));
        }
    }
}