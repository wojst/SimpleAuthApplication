namespace SimpleAuthApplication.Models
{
    public class CurrencyRate
    {
        public Guid Id { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public decimal Mid { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
