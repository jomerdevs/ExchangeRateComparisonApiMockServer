namespace ExchangeRateApiMockServer.Shared.Utils
{
    public static class Api1MockRates
    {
        private static readonly Dictionary<string, decimal> BaseRates = new()
        {
            { "USD", 1.00m },
            { "EUR", 0.91m },
            { "DOP", 59.31m },
            { "GBP", 0.78m },
            { "BRL", 5.12m },
            { "JPY", 143.54m },
            { "CAD", 1.32m }
        };

        public static decimal GetRate(string from, string to)
        {
            // Normalizamos a mayusculas
            from = from.ToUpperInvariant();
            to = to.ToUpperInvariant();

            // Si se desconoce, se usa un rango random
            var fromRate = BaseRates.ContainsKey(from) ? BaseRates[from] : RandomRate(0.5m, 100m);
            var toRate = BaseRates.ContainsKey(to) ? BaseRates[to] : RandomRate(0.5m, 100m);

            // Simula una tasa de cambio de mercado real
            var rate = toRate / fromRate;

            return rate;
        }

        private static decimal RandomRate(decimal min, decimal max)
        {
            var rand = new Random();
            var range = (double)(max - min);
            return (decimal)(min + (decimal)(rand.NextDouble() * range));
        }
    }
}
