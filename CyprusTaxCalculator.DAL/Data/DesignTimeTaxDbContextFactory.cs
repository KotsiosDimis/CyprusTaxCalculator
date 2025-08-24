using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CyprusTaxCalculator.DAL.Data
{
    public class DesignTimeTaxDbContextFactory : IDesignTimeDbContextFactory<TaxDbContext>
    {
        public TaxDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaxDbContext>();
            // Use your actual connection string here:
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=cyprustax;Username=postgres;Password=yourpassword");

            return new TaxDbContext(optionsBuilder.Options);
        }
    }
}