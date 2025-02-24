using Bank.Application.Responses;
using Bank.UserService.Models;

namespace Bank.UserService.Mappers;

public static class AccountMapper
{
    public static AccountResponse ToResponse(this Account account)
    {
        return new AccountResponse
               {
                   Id            = account.Id,
                   AccountNumber = account.AccountNumber,
                   User          = account.User.ToSimpleResponse()
               };
    }

    public static AccountSimpleResponse ToSimpleResponse(this Account account)
    {
        return new AccountSimpleResponse
               {
                   Id            = account.Id,
                   AccountNumber = account.AccountNumber,
               };
    }
}
