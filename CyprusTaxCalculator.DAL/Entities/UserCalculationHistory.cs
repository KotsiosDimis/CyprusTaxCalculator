using System;

namespace CyprusTaxCalculator.DAL.Entities
{
    public class UserCalculationHistory
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal AnnualIncome { get; set; }
        public decimal LifeInsurancePremiums { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal TaxableIncome { get; set; }
        public decimal TaxPayable { get; set; }
        public decimal TaxSavings { get; set; }
    }
}