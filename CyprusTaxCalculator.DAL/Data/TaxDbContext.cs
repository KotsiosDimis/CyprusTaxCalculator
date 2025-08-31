using CyprusTaxCalculator.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CyprusTaxCalculator.DAL.Data
{
    /// <summary>
    /// Entity Framework Core context for the tax calculator database.
    /// </summary>
    public class TaxDbContext : DbContext
    {
        public TaxDbContext(DbContextOptions<TaxDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaxBracket> TaxBrackets { get; set; } = null!;
        public DbSet<DeductionRule> DeductionRules { get; set; } = null!;
        public DbSet<UserCalculationHistory> UserCalculationHistory { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaxBracket>().ToTable("TaxBrackets");
            modelBuilder.Entity<DeductionRule>().ToTable("DeductionRules");
            modelBuilder.Entity<UserCalculationHistory>().ToTable("UserCalculationHistory");
        }
    }
}