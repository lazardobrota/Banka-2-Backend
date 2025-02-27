namespace Bank.UserService.Configurations;

public static partial class Configuration
{
    public static class Email
    {
        public static readonly string Address  = Environment.GetEnvironmentVariable("BANK_USER_EMAIL_ADDRESS")  ?? "";
        public static readonly string Password = Environment.GetEnvironmentVariable("BANK_USER_EMAIL_PASSWORD") ?? "";
        public static readonly string Server   = Environment.GetEnvironmentVariable("BANK_USER_EMAIL_SERVER")   ?? "smtp.gmail.com";
        public static readonly int    Port     = int.Parse(Environment.GetEnvironmentVariable("BANK_USER_EMAIL_PORT") ?? "587");
    }
}
