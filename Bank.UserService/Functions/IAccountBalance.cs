namespace Bank.UserService.Functions;

public interface IAccountBalance
{
    public bool ChangeAvailableBalance(decimal amount);

    public bool ChangeBalance(decimal amount);

    public decimal GetAvailableBalance();
}
