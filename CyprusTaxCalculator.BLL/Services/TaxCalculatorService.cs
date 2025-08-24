using CyprusTaxCalculator.DAL.Data;
using CyprusTaxCalculator.DAL.Entities;

namespace CyprusTaxCalculator.BLL.Services
{
    public class TaxCalculatorService
    {
        private readonly TaxDbContext _context;

        public TaxCalculatorService(TaxDbContext context)
        {
            _context = context;
        }

        public (decimal taxableIncome, decimal taxPayable, decimal savings) CalculateTax(decimal annualIncome, decimal lifeInsurancePaid, decimal otherDeductions)
        {
            var lifeRule = _context.DeductionRules.FirstOrDefault(r => r.DeductionType == "LifeInsurance");
            decimal allowedLifeDeduction = 0;

            if (lifeRule != null)
            {
                allowedLifeDeduction = Math.Min(lifeInsurancePaid, lifeRule.LimitValue);
            }

            decimal totalDeductions = allowedLifeDeduction + otherDeductions;
            decimal taxableIncome = Math.Max(annualIncome - totalDeductions, 0);

            var brackets = _context.TaxBrackets.OrderBy(b => b.MinIncome).ToList();
            decimal tax = 0;

            foreach (var bracket in brackets)
            {
                if (taxableIncome > bracket.MinIncome)
                {
                    decimal upperLimit = bracket.MaxIncome ?? taxableIncome;
                    decimal incomeInBracket = Math.Min(taxableIncome, upperLimit) - bracket.MinIncome + 1;
                    tax += incomeInBracket * bracket.Rate;
                }
            }

            decimal taxWithoutLifeDeduction = CalculateTaxWithoutLifeDeduction(annualIncome, otherDeductions);
            decimal savings = taxWithoutLifeDeduction - tax;

            return (taxableIncome, tax, savings);
        }

        private decimal CalculateTaxWithoutLifeDeduction(decimal annualIncome, decimal otherDeductions)
        {
            decimal taxableIncome = Math.Max(annualIncome - otherDeductions, 0);
            var brackets = _context.TaxBrackets.OrderBy(b => b.MinIncome).ToList();
            decimal tax = 0;

            foreach (var bracket in brackets)
            {
                if (taxableIncome > bracket.MinIncome)
                {
                    decimal upperLimit = bracket.MaxIncome ?? taxableIncome;
                    decimal incomeInBracket = Math.Min(taxableIncome, upperLimit) - bracket.MinIncome + 1;
                    tax += incomeInBracket * bracket.Rate;
                }
            }
            return tax;
        }

        public async Task SaveCalculationResultAsync(
            decimal annualIncome,
            decimal lifeInsurancePremiums,
            decimal otherDeductions,
            decimal taxableIncome,
            decimal taxPayable,
            decimal taxSavings)
        {
            var history = new UserCalculationHistory
            {
                AnnualIncome = annualIncome,
                LifeInsurancePremiums = lifeInsurancePremiums,
                OtherDeductions = otherDeductions,
                TaxableIncome = taxableIncome,
                TaxPayable = taxPayable,
                TaxSavings = taxSavings,
                Timestamp = DateTime.UtcNow
            };
            _context.UserCalculationHistory.Add(history);
            await _context.SaveChangesAsync();
        }
    }
}
