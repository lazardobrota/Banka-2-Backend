using Bank.Application.Utilities;

namespace Bank.ExchangeService.Configurations;

public static partial class Configuration
{
    public static class Security
    {
        public const string AlpacaApiKey    = "PK35PJ8VICVZIEGVUD4B";
        public const string AlpacaSecretKey = "xEAAMGXWQ169tsXXPibdnx7M1jSoWPV98SXx8Pjs";

        public static class Stock
        {
            public const string GetAllApi                 = "https://paper-api.alpaca.markets/v2/assets";
            public const string GetHistoryApi             = "https://data.alpaca.markets/v2/stocks/bars";
            public const string GetLatest                 = "https://data.alpaca.markets/v2/stocks/bars/latest";
            public const int    HistoryTimeFrameInMinutes = 15;
            public const int    LatestTimeFrameInMinutes  = 1;

            private static          int      s_ApiKeyIndex    = 0;
            private static          int      s_ApiSecretIndex = 0;
            private static readonly string[] s_ApiKeys        = EnvironmentUtilities.GetStringArrayVariable("BANK_EXCHANGE_SECURITY_STOCK_API_KEY");
            private static readonly string[] s_ApiSecrets     = EnvironmentUtilities.GetStringArrayVariable("BANK_EXCHANGE_SECURITY_STOCK_API_SECRET");
            public static           string   ApiKey    => s_ApiKeys[s_ApiKeyIndex = (s_ApiKeyIndex          + 1) % s_ApiKeys.Length];
            public static           string   ApiSecret => s_ApiSecrets[s_ApiSecretIndex = (s_ApiSecretIndex + 1) % s_ApiSecrets.Length];

            public static readonly string StartTime = DateTime.UtcNow.AddDays(-4).ToString("yyyy-MM-ddTHH:mm:ssZ");
            public static readonly string EndTime   = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            // public const string StartTime = "2025-04-04T12:30:00Z";
            // public const string EndTime   = "2025-04-07T12:30:00Z";
        }
    }
}
