using Microsoft.AspNetCore.Mvc.RazorPages;
using CyprusTaxCalculator.DAL.Data;
using CyprusTaxCalculator.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CyprusTaxCalculator.UI.Pages
{
    public class HistoryModel : PageModel
    {
        private readonly TaxDbContext _context;

        public HistoryModel(TaxDbContext context)
        {
            _context = context;
        }

        public List<UserCalculationHistory> History { get; set; } = new();

        public void OnGet()
        {
            History = _context.UserCalculationHistory
                .OrderByDescending(h => h.Timestamp)
                .Take(20)
                .ToList();
        }
    }
}