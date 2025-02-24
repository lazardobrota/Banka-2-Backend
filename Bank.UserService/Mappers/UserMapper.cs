using Bank.Application.Responses;
using Bank.UserService.Models;

namespace Bank.UserService.Mappers;

public static class UserMapper
{
    public static UserResponse ToResponse(this User user)
    {
        return new UserResponse
               {
                   Id                         = user.Id,
                   FirstName                  = user.FirstName,
                   LastName                   = user.LastName,
                   DateOfBirth                = user.DateOfBirth,
                   Gender                     = user.Gender,
                   UniqueIdentificationNumber = user.UniqueIdentificationNumber,
                   Email                      = user.Email,
                   Username                   = user.Username,
                   PhoneNumber                = user.PhoneNumber,
                   Address                    = user.Address,
                   Role                       = user.Role,
                   Department                 = user.Department,
                   Accounts                   = MapAccounts(user.Accounts),
                   CreatedAt                  = user.CreatedAt,
                   ModifiedAt                 = user.ModifiedAt,
                   Activated                  = user.Activated
               };
    }

    private static List<AccountSimpleResponse> MapAccounts(List<Account> accounts) =>
    accounts.Select(account => account.ToSimpleResponse())
            .ToList();

    public static UserSimpleResponse ToSimpleResponse(this User user)
    {
        return new UserSimpleResponse
               {
                   Id                         = user.Id,
                   FirstName                  = user.FirstName,
                   LastName                   = user.LastName,
                   DateOfBirth                = user.DateOfBirth,
                   Gender                     = user.Gender,
                   UniqueIdentificationNumber = user.UniqueIdentificationNumber,
                   Email                      = user.Email,
                   Username                   = user.Username,
                   PhoneNumber                = user.PhoneNumber,
                   Address                    = user.Address,
                   Role                       = user.Role,
                   Department                 = user.Department,
                   CreatedAt                  = user.CreatedAt,
                   ModifiedAt                 = user.ModifiedAt,
                   Activated                  = user.Activated
               };
    }
}
