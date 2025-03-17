using Bank.Application.Requests;

namespace Bank.UserService.Test.Examples.Entities;

public static partial class Example
{
    public static partial class Entity
    {
        public static class User
        {
            public static readonly UserLoginRequest LoginRequest = new()
                                                                   {
                                                                       Email    = "admin@gmail.com",
                                                                       Password = "admin"
                                                                   };
        }
    }
}
