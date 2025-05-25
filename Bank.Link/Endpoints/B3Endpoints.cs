namespace Bank.Link.Endpoints;

public static partial class Endpoint
{
    public static class B3
    {
        public const string ApiBase = "api";

        public static class Account
        {
            public const string Base = $"{ApiBase}/account";

            public const string GetAll = $"{Base}";
        }
    }
}
