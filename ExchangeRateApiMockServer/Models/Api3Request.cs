namespace ExchangeRateApiMockServer.Models
{
    public class Api3Request
    {
        public Exchange exchange { get; set; } = new Exchange();
    }

    public class Exchange
    {
        public string sourceCurrency { get; set; } = string.Empty;
        public string targetCurrency { get; set; } = string.Empty;
        public decimal quantity { get; set; }
        }
}
