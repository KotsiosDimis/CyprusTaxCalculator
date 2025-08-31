using CyprusTaxCalculator.BLL.Services;
using CyprusTaxCalculator.DAL.Entities;
using CyprusTaxCalculator.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CyprusTaxCalculator.Tests
{
    public class TaxCalculatorServiceTests
    {
        private TaxCalculatorService CreateService()
        {
            var options = new DbContextOptionsBuilder<TaxDbContext>()
                .UseInMemoryDatabase($"TaxTestDb_{Guid.NewGuid()}")
                .Options;

            var context = new TaxDbContext(options);

            context.TaxBrackets.AddRange(
                new TaxBracket { MinIncome = 0, MaxIncome = 19500, Rate = 0m },
                new TaxBracket { MinIncome = 19501, MaxIncome = 28000, Rate = 0.20m },
                new TaxBracket { MinIncome = 28001, MaxIncome = 36300, Rate = 0.25m },
                new TaxBracket { MinIncome = 36301, MaxIncome = 60000, Rate = 0.30m },
                new TaxBracket { MinIncome = 60001, MaxIncome = null, Rate = 0.35m }
            );

            context.SaveChanges();

            return new TaxCalculatorService(context);
        }

        [Fact]
        public void ZeroIncome_ReturnsZero()
        {
            var service = CreateService();
            var result = service.CalculateTax(0, 0, 0);
            Assert.Equal(0, result.taxableIncome);
            Assert.Equal(0, result.taxPayable);
            Assert.Equal(0, result.savings);
        }

        [Fact]
        public void IncomeBelow19500_TaxShouldBeZero()
        {
            var service = CreateService();
            var result = service.CalculateTax(18000, 500, 300);
            Assert.Equal(17200, result.taxableIncome);
            Assert.Equal(0, result.taxPayable);
        }

        [Fact]
        public void CombinedDeductionsUnderCap_FullyApplied()
        {
            var service = CreateService();
            var result = service.CalculateTax(30000, 3000, 2000); // Combined = 5k < cap (6k)
            Assert.Equal(25000, result.taxableIncome);
        }

        [Fact]
        public void CombinedDeductionsAtCap_FullyApplied()
        {
            var service = CreateService();
            var result = service.CalculateTax(30000, 3000, 3000); // Combined = 6k == cap
            Assert.Equal(24000, result.taxableIncome);
        }

        [Fact]
        public void CombinedDeductionsOverCap_AreCapped()
        {
            var service = CreateService();
            var result = service.CalculateTax(30000, 4000, 4000); // Combined = 8k > cap (6k)
            Assert.Equal(24000, result.taxableIncome); // Only 6k applied
        }

        [Fact]
        public void HighIncome_AllDeductionsUnderCap()
        {
            var service = CreateService();
            var result = service.CalculateTax(100000, 10000, 10000); // Combined = 20k == cap
            Assert.Equal(80000, result.taxableIncome);
        }

        [Fact]
        public void DeductionsExceedIncome_TaxableFloored()
        {
            var service = CreateService();
            var result = service.CalculateTax(1000, 1000, 1000); // Cap = 200
            Assert.Equal(800, result.taxableIncome); // Only 200 applied
        }

        [Fact]
        public void NegativeDeductions_Ignored()
        {
            var service = CreateService();
            var result = service.CalculateTax(30000, -500, -1000);
            Assert.Equal(30000, result.taxableIncome);
        }

        [Fact]
        public void OnlyLifeInsurance_WithinCap()
        {
            var service = CreateService();
            var result = service.CalculateTax(25000, 4000, 0); // Cap = 5000
            Assert.Equal(21000, result.taxableIncome);
        }

        [Fact]
        public void OnlyOtherDeductions_WithinCap()
        {
            var service = CreateService();
            var result = service.CalculateTax(36000, 0, 7000); // Cap = 7200
            Assert.Equal(29000, result.taxableIncome);
        }

        [Fact]
        public void AllZeroDeductions_IncomeTaxedFully()
        {
            var service = CreateService();
            var result = service.CalculateTax(45000, 0, 0);
            Assert.Equal(45000, result.taxableIncome);
        }

        [Fact]
        public void LifeInsuranceAndOther_SharedCap()
        {
            var service = CreateService();
            var result = service.CalculateTax(50000, 7000, 5000); // Combined = 12k > cap = 10k
            Assert.Equal(40000, result.taxableIncome);
        }

        [Fact]
        public void EdgeCase_AtBracketLimit()
        {
            var service = CreateService();
            var result = service.CalculateTax(28000, 0, 0);
            Assert.Equal(28000, result.taxableIncome);
        }

        [Fact]
        public void RealWorldCase()
        {
            var service = CreateService();
            var result = service.CalculateTax(28000, 1000, 2000); // Cap = 5600, applies fully
            Assert.Equal(25000, result.taxableIncome);
        }

        [Fact]
        public void Savings_AreCorrect()
        {
            var service = CreateService();
            var result = service.CalculateTax(40000, 5000, 3000); // Cap = 8000 → allowed
            Assert.True(result.savings > 0);
        }

        [Fact]
        public void LifeInsuranceZero_OtherDeductionsCapped()
        {
            var service = CreateService();
            var result = service.CalculateTax(30000, 0, 8000); // Cap = 6000
            Assert.Equal(24000, result.taxableIncome);
        }

        [Fact]
        public void LifeInsuranceZero_OtherDeductionsUnderCap()
        {
            var service = CreateService();
            var result = service.CalculateTax(30000, 0, 3000); // Cap = 6000
            Assert.Equal(27000, result.taxableIncome);
        }

        [Fact]
        public void FullCapApplied_WhenCombinedExactlyEqualsCap()
        {
            var service = CreateService();
            var result = service.CalculateTax(36000, 3000, 4200); // Combined = 7200 = cap
            Assert.Equal(28800, result.taxableIncome);
        }
    }
}