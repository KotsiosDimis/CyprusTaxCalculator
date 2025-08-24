namespace CyprusTaxCalculator.BLL.Models
{
    public record AdvancedTaxInput(
        decimal salary,
        decimal selfEmployed,
        decimal grossRents,
        decimal pension,
        decimal widowPension,
        decimal redeemedLife13,
        decimal redeemedLife46,
        decimal otherTaxable,
        decimal nonTaxable,
        decimal subscriptions,
        decimal donations,
        decimal capitalAllowances,
        decimal interestRented,
        decimal expensesRented,
        decimal otherDeductions,
        bool firstTimeEmployed,
        decimal socialSecurity,
        decimal medicalFund,
        decimal pensionFunds,
        decimal ghs,
        decimal lifeInsurance,
        string employmentType,
        decimal amountAvailableLife
    );
}
