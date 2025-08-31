using CyprusTaxCalculator.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using CyprusTaxCalculator.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using CyprusTaxCalculator.BLL.Models; // AdvancedTaxInput lives here
using CyprusTaxCalculator.DAL; // This gives access to SeedData

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL connection
builder.Services.AddDbContext<TaxDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register BLL services
builder.Services.AddScoped<TaxCalculatorService>();
builder.Services.AddScoped<AdvancedTaxCalculatorService>();

// Add Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// DB migration + seeding
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TaxDbContext>();

    var stopwatch = Stopwatch.StartNew();
    Console.WriteLine("⏳ Starting database migration and seeding...");

    db.Database.Migrate();
    SeedData.Initialize(db);

    stopwatch.Stop();
    Console.WriteLine($"✅ Database migration + seeding completed in {stopwatch.ElapsedMilliseconds} ms");
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

// Minimal API for basic calculator
app.MapPost("/calculate", ([FromBody] TaxInput input, TaxCalculatorService taxService) =>
{
    if (input == null)
    {
        return Results.BadRequest("Invalid input");
    }

    var result = taxService.CalculateTax(input.AnnualIncome, input.LifeInsurancePaid, input.OtherDeductions);

    return Results.Json(new
    {
        taxableIncome = result.taxableIncome,
        taxPayable = result.taxPayable,
        savings = result.savings
    });
});

// Minimal API for advanced calculator
app.MapPost("/advanced-calculate", ([FromBody] AdvancedTaxInput input, AdvancedTaxCalculatorService taxService) =>
{
    if (input == null)
    {
        return Results.BadRequest("Invalid input");
    }

    var result = taxService.Calculate(input);
    return Results.Json(result);
});

app.Run();

// Record for basic calculator
public record TaxInput(decimal AnnualIncome, decimal LifeInsurancePaid, decimal OtherDeductions);
