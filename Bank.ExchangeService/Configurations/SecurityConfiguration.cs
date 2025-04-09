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
            public const string GetLatest                 = "https://data.alpaca.markets/v2/stocks/bars/latest";
            public const int    HistoryTimeFrameInMinutes = 15;
            public const int    LatestTimeFrameInMinutes  = 1;

            private static          int      s_ApiKeyAndSecretIndex = 0;
            private static readonly Lock     s_ApiKeyAndSecretLock  = new();
            private static readonly string[] s_ApiKeys              = EnvironmentUtilities.GetStringArrayVariable("BANK_EXCHANGE_SECURITY_STOCK_API_KEY");
            private static readonly string[] s_ApiSecrets           = EnvironmentUtilities.GetStringArrayVariable("BANK_EXCHANGE_SECURITY_STOCK_API_SECRET");

            // public static           string   ApiKey    => s_ApiKeys[s_ApiKeyIndex = (s_ApiKeyIndex          + 1) % s_ApiKeys.Length];
            // public static           string   ApiSecret => s_ApiSecrets[s_ApiSecretIndex = (s_ApiSecretIndex + 1) % s_ApiSecrets.Length];

            public static (string key, string secret) ApiKeyAndSecret => FindKeyAndSecret();

            public static readonly string StartTime = DateTime.UtcNow.AddDays(-4)
                                                              .ToString("yyyy-MM-ddTHH:mm:ssZ");

            public static readonly string EndTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

            private static (string key, string secret) FindKeyAndSecret()
            {
                lock (s_ApiKeyAndSecretLock)
                {
                    s_ApiKeyAndSecretIndex = (s_ApiKeyAndSecretIndex + 1) % s_ApiKeys.Length;
                }

                return (s_ApiKeys[s_ApiKeyAndSecretIndex], s_ApiSecrets[s_ApiKeyAndSecretIndex]);
            }
        }

        public static class ForexPair
        {
            public const           string GetDataApi  = "https://www.alphavantage.co/query";
            public static readonly string ApiKeyForex = EnvironmentUtilities.GetStringVariable("BANK_EXCHANGE_SECURITY_FOREX_PAIR_API_KEY");
        }
    }
}
