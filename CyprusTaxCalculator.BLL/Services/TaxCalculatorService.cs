using CyprusTaxCalculator.DAL.Data;
using CyprusTaxCalculator.DAL.Entities;
using CyprusTaxCalculator.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CyprusTaxCalculator.BLL.Services
{
    public class TaxCalculatorService
    {
        private readonly TaxDbContext _context;

        public TaxCalculatorService(TaxDbContext context)
        {
            _context = context;
        }

        public (decimal taxableIncome, decimal taxPayable, decimal savings) CalculateTax(
            decimal annualIncome,
            decimal lifeInsurancePaid,
            decimal otherDeductions)
        {
            annualIncome = Math.Max(annualIncome, 0);
            lifeInsurancePaid = Math.Max(lifeInsurancePaid, 0);
            otherDeductions = Math.Max(otherDeductions, 0);

            // Cap total deductions to 1/5 of income
            decimal maxTotalDeductions = annualIncome / 5;
            decimal totalRequested = lifeInsurancePaid + otherDeductions;

            decimal allowedLife = lifeInsurancePaid;
            decimal allowedOther = otherDeductions;

            if (totalRequested > maxTotalDeductions)
            {
                decimal ratio = maxTotalDeductions / totalRequested;
                allowedLife = Math.Round(lifeInsurancePaid * ratio, 2, MidpointRounding.AwayFromZero);
                allowedOther = Math.Round(otherDeductions * ratio, 2, MidpointRounding.AwayFromZero);
            }

            decimal totalDeductions = allowedLife + allowedOther;
            decimal taxableIncome = Math.Max(annualIncome - totalDeductions, 0);

            decimal tax = CalculateTaxFromIncome(taxableIncome);
            decimal taxWithoutLife = CalculateTaxFromIncome(Math.Max(annualIncome - allowedOther, 0));
            decimal savings = Math.Round(taxWithoutLife - tax, 2, MidpointRounding.AwayFromZero);

            return (taxableIncome, tax, savings);
        }

        private decimal CalculateTaxFromIncome(decimal taxableIncome)
        {
            var brackets = _context.TaxBrackets
                .OrderBy(b => b.MinIncome)
                .ToList();

            decimal tax = 0;

            foreach (var bracket in brackets)
            {
                decimal lower = bracket.MinIncome;
                decimal upper = bracket.MaxIncome ?? decimal.MaxValue;

                if (taxableIncome > lower)
                {
                    decimal incomeInBracket = Math.Min(taxableIncome, upper) - lower;
                    tax += incomeInBracket * bracket.Rate;
                }
            }

            return Math.Round(tax, 2, MidpointRounding.AwayFromZero);
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

        public List<TaxBracketTax> GetTaxBreakdown(decimal taxableIncome)
        {
            var brackets = _context.TaxBrackets.OrderBy(b => b.MinIncome).ToList();
            var breakdown = new List<TaxBracketTax>();

            foreach (var bracket in brackets)
            {
                decimal lower = bracket.MinIncome;
                decimal upper = bracket.MaxIncome ?? decimal.MaxValue;

                if (taxableIncome > lower)
                {
                    decimal incomeInBracket = Math.Min(taxableIncome, upper) - lower;
                    if (incomeInBracket > 0)
                    {
                        decimal tax = incomeInBracket * bracket.Rate;
                        breakdown.Add(new TaxBracketTax
                        {
                            LowerLimit = lower,
                            UpperLimit = bracket.MaxIncome,
                            Rate = bracket.Rate,
                            TaxedAmount = incomeInBracket,
                            Tax = Math.Round(tax, 2, MidpointRounding.AwayFromZero)
                        });
                    }
                }
            }
            return breakdown;
        }
    }
}