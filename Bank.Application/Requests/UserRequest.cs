namespace Bank.Application.Requests;

public class UserLoginRequest
{
    public required string Email    { set; get; }
    public required string Password { set; get; }
}
