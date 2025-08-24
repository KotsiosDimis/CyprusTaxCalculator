namespace CyprusTaxCalculator.DAL.Entities
{
    public class TaxBracket
    {
        public int Id { get; set; }
        public decimal MinIncome { get; set; }
        public decimal? MaxIncome { get; set; }
        public decimal Rate { get; set; }
    }
}
