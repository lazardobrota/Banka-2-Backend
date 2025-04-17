using Bank.Application.Requests;
using Bank.UserService.Database.Seeders;

namespace Bank.UserService.Database.Examples;

public static partial class Example
{
    public static class User
    {
        public static readonly UserLoginRequest LoginRequest = new()
                                                               {
                                                                   Email    = Seeder.Client.Client01.Email,
                                                                   Password = "client1"
                                                               };

        public static readonly UserActivationRequest ActivationRequest = new()
                                                                         {
                                                                             Password        = "M4rk0Petrovic@2024",
                                                                             ConfirmPassword = "M4rk0Petrovic@2024"
                                                                         };

        public static readonly UserRequestPasswordResetRequest RequestPasswordResetRequest = new()
                                                                                             {
                                                                                                 Email = "marko.petrovic@example.com"
                                                                                             };

        public static readonly UserPasswordResetRequest PasswordResetRequest = new()
                                                                               {
                                                                                   Password        = "M4rk0Petrovic@2025",
                                                                                   ConfirmPassword = "M4rk0Petrovic@2025"
                                                                               };
    }
}
