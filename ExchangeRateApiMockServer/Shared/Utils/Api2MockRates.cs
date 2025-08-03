namespace ExchangeRateApiMockServer.Shared.Utils
{
    public static class Api2MockRates
    {
        private static readonly Dictionary<string, decimal> BaseRates = new()
        {
            { "USD", 1.0m },
            { "EUR", 0.89m },
            { "DOP", 58.13m },
            { "GBP", 0.78m },
            { "BRL", 4.81m },
            { "JPY", 142.52m },
            { "CAD", 1.12m }
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
