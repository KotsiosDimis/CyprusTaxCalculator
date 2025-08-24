using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CyprusTaxCalculator.BLL.Services;

namespace CyprusTaxCalculator.UI.Pages.Api
{
    [IgnoreAntiforgeryToken] // disable CSRF for API
    public class CalculateModel : PageModel
    {
        private readonly TaxCalculatorService _taxService;

        public CalculateModel(TaxCalculatorService taxService)
        {
            _taxService = taxService;
        }

        public IActionResult OnPost([FromBody] TaxInput input)
        {
            if (input == null)
                return BadRequest("Invalid input");

            var result = _taxService.CalculateTax(input.AnnualIncome, input.LifeInsurancePaid, input.OtherDeductions);

            return new JsonResult(new
            {
                taxableIncome = result.taxableIncome,
                taxPayable = result.taxPayable,
                savings = result.savings
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
