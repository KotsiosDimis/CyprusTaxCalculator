namespace CyprusTaxCalculator.DAL.Entities
{
    public class DeductionRule
    {
        public int Id { get; set; }
        public required string DeductionType { get; set; }
        public required string LimitType { get; set; }
        public decimal LimitValue { get; set; }
    }
}