using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CyprusTaxCalculator.BLL.Services;
using CyprusTaxCalculator.BLL.Models;
using System.Collections.Generic;

namespace CyprusTaxCalculator.UI.Pages
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly TaxCalculatorService _taxService;

        public IndexModel(TaxCalculatorService taxService)
        {
            _taxService = taxService;
        }

        public IActionResult OnPost([FromBody] TaxInput input)
        {
            if (input == null)
                return BadRequest("Invalid input");

            var result = _taxService.CalculateTax(input.AnnualIncome, input.LifeInsurancePaid, input.OtherDeductions);
            var breakdown = _taxService.GetTaxBreakdown(result.taxableIncome);

            return new JsonResult(new
            {
                taxableIncome = result.taxableIncome,
                taxPayable = result.taxPayable,
                savings = result.savings,
                breakdown = breakdown.Select(b => new
                {
                    lowerLimit = b.LowerLimit,
                    upperLimit = b.UpperLimit,
                    rate = b.Rate,
                    taxedAmount = b.TaxedAmount,
                    tax = b.Tax
                })
            });
        }

        public class TaxInput
        {
            public decimal AnnualIncome { get; set; }
            public decimal LifeInsurancePaid { get; set; }
            public decimal OtherDeductions { get; set; }
        }
    }
}