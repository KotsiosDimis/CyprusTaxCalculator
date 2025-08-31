namespace CyprusTaxCalculator.DAL.Entities
{
    /// <summary>
    /// Represents a progressive tax bracket with a minimum and (optional) maximum income range and a tax rate.
    /// </summary>
    public class TaxBracket
    {
        public int Id { get; set; }

        /// <summary>
        /// The minimum income for this tax bracket (inclusive).
        /// </summary>
        public decimal MinIncome { get; set; }

        /// <summary>
        /// The maximum income for this tax bracket (inclusive). If null, the bracket has no upper limit.
        /// </summary>
        public decimal? MaxIncome { get; set; }

        /// <summary>
        /// The tax rate applied to income within this bracket.
        /// </summary>
        public decimal Rate { get; set; }
    }
}