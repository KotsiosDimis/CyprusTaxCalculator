namespace CyprusTaxCalculator.DAL.Entities
{
    public class DeductionRule
    {
        public int Id { get; set; }
        public string DeductionType { get; set; }
        public string LimitType { get; set; }
        public decimal LimitValue { get; set; }
    }
}
