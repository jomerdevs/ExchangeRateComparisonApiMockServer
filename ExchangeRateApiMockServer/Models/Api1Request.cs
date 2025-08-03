namespace ExchangeRateApiMockServer.Models
{
    public class Api1Request
    {
        public string from { get; set; } = string.Empty;
        public string to { get; set; } = string.Empty;
        public decimal value { get; set; }
    }
}
