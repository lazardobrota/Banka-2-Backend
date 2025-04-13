using Bank.Application.Requests;
using Bank.Application.Responses;

namespace Bank.OpenApi.Examples;

public static partial class Example
{
    public static class User
    {
        public static readonly UserLoginRequest LoginRequest = new()
                                                               {
                                                                   Email    = Constant.Email,
                                                                   Password = Constant.Password,
                                                               };

        public static readonly UserActivationRequest ActivationRequest = new()
                                                                         {
                                                                             Password        = Constant.Password,
                                                                             ConfirmPassword = Constant.Password,
                                                                         };

        public static readonly UserRequestPasswordResetRequest RequestPasswordResetRequest = new()
                                                                                             {
                                                                                                 Email = Constant.Email,
                                                                                             };

        public static readonly UserPasswordResetRequest PasswordResetRequest = new()
                                                                               {
                                                                                   Password        = Constant.Password,
                                                                                   ConfirmPassword = Constant.Password,
                                                                               };

        public static readonly UserResponse Response = new()
                                                       {
                                                           Id                         = Constant.Id,
                                                           FirstName                  = Constant.FirstName,
                                                           LastName                   = Constant.LastName,
                                                           DateOfBirth                = Constant.CreationDate,
                                                           Gender                     = Constant.Gender,
                                                           UniqueIdentificationNumber = Constant.UniqueIdentificationNumber,
                                                           Username                   = Constant.Username,
                                                           Email                      = Constant.Email,
                                                           PhoneNumber                = Constant.Phone,
                                                           Address                    = Constant.Address,
                                                           Role                       = Constant.Role,
                                                           Department                 = Constant.Department,
                                                           Accounts                   = [Account.SimpleResponse],
                                                           CreatedAt                  = Constant.CreatedAt,
                                                           ModifiedAt                 = Constant.ModifiedAt,
                                                           Activated                  = Constant.Boolean,
                                                       };

        public static readonly UserSimpleResponse SimpleResponse = new()
                                                                   {
                                                                       Id                         = Constant.Id,
                                                                       FirstName                  = Constant.FirstName,
                                                                       LastName                   = Constant.LastName,
                                                                       DateOfBirth                = Constant.CreationDate,
                                                                       Gender                     = Constant.Gender,
                                                                       UniqueIdentificationNumber = Constant.UniqueIdentificationNumber,
                                                                       Username                   = Constant.Username,
                                                                       Email                      = Constant.Email,
                                                                       PhoneNumber                = Constant.Phone,
                                                                       Address                    = Constant.Address,
                                                                       Role                       = Constant.Role,
                                                                       Department                 = Constant.Department,
                                                                       CreatedAt                  = Constant.CreatedAt,
                                                                       ModifiedAt                 = Constant.ModifiedAt,
                                                                       Activated                  = Constant.Boolean,
                                                                   };

        public static readonly UserLoginResponse LoginResponse = new()
                                                                 {
                                                                     Token = Constant.Token,
                                                                     User  = Response,
                                                                 };
    }
}
