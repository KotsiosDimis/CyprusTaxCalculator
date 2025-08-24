using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CyprusTaxCalculator.BLL.Services;
using System.Threading.Tasks;

namespace CyprusTaxCalculator.UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly TaxCalculatorService _taxService;

        public IndexModel(TaxCalculatorService taxService)
        {
            _taxService = taxService;
        }

        [BindProperty]
        public decimal AnnualIncome { get; set; }

        [BindProperty]
        public decimal LifeInsurancePaid { get; set; }

        [BindProperty]
        public decimal OtherDeductions { get; set; }

        public decimal TaxableIncome { get; set; }
        public decimal TaxPayable { get; set; }
        public decimal Savings { get; set; }
        public bool Calculated { get; set; } = false;

        public async Task OnPostAsync()
        {
            var result = _taxService.CalculateTax(AnnualIncome, LifeInsurancePaid, OtherDeductions);
            TaxableIncome = result.taxableIncome;
            TaxPayable = result.taxPayable;
            Savings = result.savings;
            Calculated = true;

            // Save calculation history
            await _taxService.SaveCalculationResultAsync(
                AnnualIncome,
                LifeInsurancePaid,
                OtherDeductions,
                TaxableIncome,
                TaxPayable,
                Savings
            );
        }
    }
}