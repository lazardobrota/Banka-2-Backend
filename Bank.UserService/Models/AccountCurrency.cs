using Bank.UserService.Functions;

namespace Bank.UserService.Models;

public class AccountCurrency : IAccountBalance
{
    public required Guid      Id               { set; get; }
    public          Account?  Account          { set; get; }
    public required Guid      AccountId        { set; get; }
    public          User?     Employee         { set; get; }
    public required Guid      EmployeeId       { set; get; }
    public          Currency? Currency         { set; get; }
    public required Guid      CurrencyId       { set; get; }
    public required decimal   Balance          { set; get; }
    public required decimal   AvailableBalance { set; get; }
    public required decimal   DailyLimit       { set; get; }
    public required decimal   MonthlyLimit     { set; get; }
    public required DateTime  CreatedAt        { set; get; }
    public required DateTime  ModifiedAt       { set; get; }

    public bool ChangeAvailableBalance(decimal amount)
    {
        AvailableBalance += amount;

        return true;
    }

    public bool ChangeBalance(decimal amount)
    {
        Balance += amount;

        return true;
    }

    public decimal GetAvailableBalance()
    {
        return AvailableBalance;
    }
}
