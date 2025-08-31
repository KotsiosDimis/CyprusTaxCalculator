namespace CyprusTaxCalculator.DAL.Entities
{
    public class DeductionRule
    {
        public int Id { get; set; }

        public string DeductionType { get; set; } = string.Empty;

        public string LimitType { get; set; } = "Amount"; // ✅ ADD THIS LINE

        public decimal LimitValue { get; set; }
    }
}