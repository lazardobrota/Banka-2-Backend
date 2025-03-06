namespace Bank.UserService.Models;

public class AccountCurrency
{
    public required Guid     Id               { set; get; }
    public required Account  Account          { set; get; }
    public required Guid     AccountId        { set; get; }
    public required User     Employee         { set; get; }
    public required Guid     EmployeeId       { set; get; }
    public required Currency Currency         { set; get; }
    public required Guid     CurrencyId       { set; get; }
    public required decimal  Balance          { set; get; }
    public required decimal  AvailableBalance { set; get; }
    public required decimal  DailyLimit       { set; get; }
    public required decimal  MonthlyLimit     { set; get; }
    public required DateTime CreatedAt        { set; get; }
    public required DateTime ModifiedAt       { set; get; }
}
