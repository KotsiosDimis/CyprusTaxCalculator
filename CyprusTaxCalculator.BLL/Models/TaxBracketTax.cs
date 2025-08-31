namespace CyprusTaxCalculator.BLL.Models
{
    public class TaxBracketTax
    {
        public decimal LowerLimit { get; set; }
        public decimal? UpperLimit { get; set; }
        public decimal Rate { get; set; }
        public decimal TaxedAmount { get; set; }
        public decimal Tax { get; set; }
    }
}