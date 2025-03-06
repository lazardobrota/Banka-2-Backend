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
                   AccountNumber = account.Number,
                   Name          = account.Name,
                   Client = account.Client.ToClient()
                                   .ToSimpleResponse(),
                   Balance          = account.Balance,
                   AvailableBalance = account.AvailableBalance,
                   Employee = account.Employee.ToEmployee()
                                     .ToSimpleResponse(),
                   Currency = null,
                   Type     = account.Type.ToResponse(),
                   //TODO: accountCurrency
                   DailyLimit     = 0,
                   MonthlyLimit   = 0,
                   CreationDate   = account.CreationDate,
                   ExpirationDate = account.ExpirationDate,
                   Status         = account.Status,
                   CreatedAt      = account.CreatedAt,
                   ModifiedAt     = account.ModifiedAt
               };
    }

    public static AccountSimpleResponse ToSimpleResponse(this Account account)
    {
        return new AccountSimpleResponse
               {
                   Id            = account.Id,
                   AccountNumber = account.Number
               };
    }
}
