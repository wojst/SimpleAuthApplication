namespace SimpleAuthApplication.Dtos
{
    public class CurrencyRateDto
    {
        public string Currency { get; set; }
        public string Code { get; set; }
        public decimal Mid { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
