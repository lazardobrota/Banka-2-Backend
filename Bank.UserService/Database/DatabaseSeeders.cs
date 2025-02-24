namespace Bank.UserService.Database;

public static class DatabaseSeeders
{
    public static async Task SeedUsers(this ApplicationContext context)
    {
        if (context.Users.Any())
            return;
        
        //TODO: add User seed
    }

    public static async Task SeedAccounts(this ApplicationContext context)
    {
        if (context.Accounts.Any())
            return;
        
        //TODO: add Account seed
    }
}
