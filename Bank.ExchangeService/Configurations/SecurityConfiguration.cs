using Bank.Application.Utilities;

namespace Bank.ExchangeService.Configurations;

public static partial class Configuration
{
    public static class Security
    {
        public static class Stock
        {
            public const string GetAllApi                 = "https://paper-api.alpaca.markets/v2/assets";
            public const string GetHistoryApi             = "https://data.alpaca.markets/v2/stocks/bars";
            public const string GetLatest                 = "https://data.alpaca.markets/v2/stocks/snapshots";
            public const int    HistoryTimeFrameInMinutes = 15;
            public const int    LatestTimeFrameInMinutes  = 1;

            public static readonly string StartTime = DateTime.UtcNow.AddDays(-4)
                                                              .ToString("yyyy-MM-ddTHH:mm:ssZ");

            public static readonly string EndTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        public static class ForexPair
        {
            public const string GetDataApi = "https://www.alphavantage.co/query";
        }

        public static class Option
        {
            public const string OptionChainApi = "https://data.alpaca.markets/v1beta1/options/snapshots";
        }

        public static class Keys
        {
            private static          int      s_ApiKeyAndSecretIndex = 0;
            private static readonly Lock     s_ApiKeyAndSecretLock  = new();
            private static readonly string[] s_ApiKeys              = EnvironmentUtilities.GetStringArrayVariable("BANK_EXCHANGE_SECURITY_STOCK_API_KEY");
            private static readonly string[] s_ApiSecrets           = EnvironmentUtilities.GetStringArrayVariable("BANK_EXCHANGE_SECURITY_STOCK_API_SECRET");

            // Forex
            public static readonly string ApiKeyForex = EnvironmentUtilities.GetStringVariable("BANK_EXCHANGE_SECURITY_FOREX_PAIR_API_KEY");

            public static (string key, string secret) AlpacaApiKeyAndSecret => FindKeyAndSecret();

            private static (string key, string secret) FindKeyAndSecret()
            {
                lock (s_ApiKeyAndSecretLock)
                {
                    s_ApiKeyAndSecretIndex = (s_ApiKeyAndSecretIndex + 1) % s_ApiKeys.Length;
                }

                return (s_ApiKeys[s_ApiKeyAndSecretIndex], s_ApiSecrets[s_ApiKeyAndSecretIndex]);
            }
        }
    }
}
