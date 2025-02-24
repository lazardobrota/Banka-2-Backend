namespace Bank.UserService.Models;

public class Account
{
    public required Guid   Id            { set; get; }
    public required string AccountNumber { set; get; }
    public required User   User          { set; get; }
    public required Guid   UserId        { set; get; }
}
