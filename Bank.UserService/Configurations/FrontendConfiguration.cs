namespace Bank.UserService.Configurations;

public static partial class Configuration
{
    public static class Frontend
    {
        public static readonly string BaseUrl = Environment.GetEnvironmentVariable("BANK_USER_FRONTEND_BASE_URL") ?? "http://localhost:5173";

        public static class Route
        {
            public static readonly string Activate      = Environment.GetEnvironmentVariable("BANK_USER_FRONTEND_ROUTE_ACTIVATE")       ?? "/activate";
            public static readonly string ResetPassword = Environment.GetEnvironmentVariable("BANK_USER_FRONTEND_ROUTE_RESET_PASSWORD") ?? "/reset-password";
        }
    }
}
