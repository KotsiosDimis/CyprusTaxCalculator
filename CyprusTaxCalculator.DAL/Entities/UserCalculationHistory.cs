namespace CyprusTaxCalculator.DAL.Entities
{
    /// <summary>
    /// Represents a historical record of a user's tax calculation.
    /// </summary>
    public class UserCalculationHistory
    {
        public int Id { get; set; }

        /// <summary>
        /// The user's reported annual income.
        /// </summary>
        public decimal AnnualIncome { get; set; }

        /// <summary>
        /// The total life insurance premiums paid by the user.
        /// </summary>
        public decimal LifeInsurancePremiums { get; set; }

        /// <summary>
        /// Any other deductions applied during the calculation.
        /// </summary>
        public decimal OtherDeductions { get; set; }

        /// <summary>
        /// The final taxable income after applying deductions and exemptions.
        /// </summary>
        public decimal TaxableIncome { get; set; }

        /// <summary>
        /// The total tax payable based on the taxable income.
        /// </summary>
        public decimal TaxPayable { get; set; }

        /// <summary>
        /// The amount of tax saved due to life insurance deductions.
        /// </summary>
        public decimal TaxSavings { get; set; }

        /// <summary>
        /// The date and time when the calculation was saved.
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}