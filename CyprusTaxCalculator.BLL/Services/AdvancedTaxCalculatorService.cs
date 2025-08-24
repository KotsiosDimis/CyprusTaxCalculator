using CyprusTaxCalculator.DAL.Data;
using CyprusTaxCalculator.BLL.Models; // ✅ add this


namespace CyprusTaxCalculator.BLL.Services
{
    public class AdvancedTaxCalculatorService
    {
        private readonly TaxDbContext _context;

        public AdvancedTaxCalculatorService(TaxDbContext context)
        {
            _context = context;
        }

        public AdvancedTaxResult Calculate(AdvancedTaxInput input)
        {
            // 1️⃣ Total Income from Step 1
            decimal totalIncome = input.salary
                + input.selfEmployed
                + input.grossRents
                + input.pension
                + input.widowPension
                + input.redeemedLife13
                + input.redeemedLife46
                + input.otherTaxable;

            // 2️⃣ Total deductions from Step 2
            decimal totalDeductions = input.subscriptions
                + input.donations
                + input.capitalAllowances
                + input.interestRented
                + input.expensesRented
                + input.otherDeductions;

            // First-time employment allowance (example: €8,550 deduction for first 3 years)
            if (input.firstTimeEmployed)
                totalDeductions += 8550;

            // 3️⃣ Total exemptions from Step 3
            decimal totalExemptions = input.socialSecurity
                + input.medicalFund
                + input.pensionFunds
                + input.ghs;

            // Life insurance premium deduction (limit logic)
            decimal lifeInsuranceCap = 1200m; // could be replaced with DB rule
            decimal allowedLifeInsurance = Math.Min(input.lifeInsurance, lifeInsuranceCap);
            totalExemptions += allowedLifeInsurance;

            // 4️⃣ Total taxable income
            decimal totalTaxableIncome = totalIncome - totalDeductions - totalExemptions;
            if (totalTaxableIncome < 0) totalTaxableIncome = 0;

            // 5️⃣ Calculate tax
            decimal taxToDeduct = CalculateTaxFromBrackets(totalTaxableIncome);

            return new AdvancedTaxResult
            {
                TotalIncome = totalIncome,
                TotalTaxableIncome = totalTaxableIncome,
                TotalDeductions = totalDeductions,
                TotalExemptions = totalExemptions,
                TaxToDeduct = taxToDeduct
            };
        }

        private decimal CalculateTaxFromBrackets(decimal income)
        {
            decimal tax = 0;

            if (income > 60000)
                tax += (income - 60000) * 0.35m;

            if (income > 36300)
                tax += (Math.Min(income, 60000) - 36300) * 0.30m;

            if (income > 28000)
                tax += (Math.Min(income, 36300) - 28000) * 0.25m;

            if (income > 19500)
                tax += (Math.Min(income, 28000) - 19500) * 0.20m;

            return tax;
        }
    }

    public class AdvancedTaxResult
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalTaxableIncome { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal TotalExemptions { get; set; }
        public decimal TaxToDeduct { get; set; }
    }
}
