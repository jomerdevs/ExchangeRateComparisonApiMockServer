namespace ExchangeRateApiMockServer.Shared.Utils
{
    public static class ReadConfigValueMock
    {
        /// <summary>
        /// Metodo que se encarga de retonar el valor obtenido desde una var de enviroment o desde el config del aplicativo
        /// </summary>
        /// <param name="config"></param>
        /// <param name="keyConfig"></param>
        /// <returns></returns>
        public static string GetConfigValue(IConfiguration config, string keyConfig)
        {
            string? varConfig = config.GetValue<string>(keyConfig);
            if (!string.IsNullOrEmpty(varConfig))
            {
                return varConfig;
            }

            // Retorna un valor por defecto o lanza una excepción con un mensaje claro
            throw new InvalidOperationException($"There is no configuration for '{keyConfig}' in the environment.");
        }
    }
}
