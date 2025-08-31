using CyprusTaxCalculator.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CyprusTaxCalculator.DAL
{
    /// <summary>
    /// Factory for creating the TaxDbContext during design time (e.g. for EF Core CLI tools).
    /// </summary>
    public class DesignTimeTaxDbContextFactory : IDesignTimeDbContextFactory<TaxDbContext>
    {
        public TaxDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TaxDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection")); // ✅ PostgreSQL here

            return new TaxDbContext(optionsBuilder.Options);
        }
    }
}