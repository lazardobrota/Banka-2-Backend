namespace Bank.UserService.Database;

public static class DatabaseConfig
{
    public static readonly string Host     = Environment.GetEnvironmentVariable("BANK_USER_DATABASE_HOST")     ?? "localhost";
    public static readonly string Port     = Environment.GetEnvironmentVariable("BANK_USER_DATABASE_PORT")     ?? "5432";
    public static readonly string Scheme   = Environment.GetEnvironmentVariable("BANK_USER_DATABASE_SCHEME")   ?? "bank_users";
    public static readonly string Username = Environment.GetEnvironmentVariable("BANK_USER_DATABASE_USERNAME") ?? "root";
    public static readonly string Password = Environment.GetEnvironmentVariable("BANK_USER_DATABASE_PASSWORD") ?? "";

    public static string GetConnectionString()
    {
        return $"Host={Host};Port={Port};Database={Scheme};Username={Username};Password={Password}";
    }
}
