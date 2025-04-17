using Bank.Application.Requests;

namespace Bank.UserService.Database.Examples;

public static partial class Example
{
    public static class AccountType
    {
        public static readonly AccountTypeCreateRequest CreateRequest = new()
                                                                        {
                                                                            Name = "Štedni račun",
                                                                            Code = "SAV"
                                                                        };

        public static readonly AccountTypeUpdateRequest UpdateRequest = new()
                                                                        {
                                                                            Name = "Štedni račun",
                                                                            Code = "SAV"
                                                                        };
    }
}
