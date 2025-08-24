using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CyprusTaxCalculator.UI.Pages
{
    public class AdvancedCalculatorModel : PageModel
    {
        public void OnGet()
        {
            // This page uses AJAX to call /advanced-calculate
            // No server-side setup is required for now.
            // If in the future we want to preload tax brackets or deduction rules,
            // we can pass them here via ViewData or a strongly typed model.
        }
    }
}
