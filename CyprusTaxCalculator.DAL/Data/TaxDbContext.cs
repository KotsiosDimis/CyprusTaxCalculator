using Microsoft.EntityFrameworkCore;
using CyprusTaxCalculator.DAL.Entities;

namespace CyprusTaxCalculator.DAL.Data
{
    public class TaxDbContext : DbContext
    {
        public TaxDbContext(DbContextOptions<TaxDbContext> options) : base(options) { }

        public DbSet<TaxBracket> TaxBrackets { get; set; }
        public DbSet<DeductionRule> DeductionRules { get; set; }

        public DbSet<UserCalculationHistory> UserCalculationHistory { get; set; }
        
    }
}
