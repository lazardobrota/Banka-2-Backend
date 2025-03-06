using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.UserService.Models;

namespace Bank.UserService.Mappers;

public static class AccountCurrencyMapper
{
    public static AccountCurrencyResponse ToResponse(this AccountCurrency accountCurrency)
    {
        return new AccountCurrencyResponse
               {
                   Id      = accountCurrency.Id,
                   Account = accountCurrency.Account.ToSimpleResponse(),
                   Employee = accountCurrency.Employee.ToEmployee()
                                             .ToSimpleResponse(),
                   Currency         = null,
                   Balance          = accountCurrency.Balance,
                   AvailableBalance = accountCurrency.AvailableBalance,
                   DailyLimit       = accountCurrency.DailyLimit,
                   MonthlyLimit     = accountCurrency.MonthlyLimit,
                   CreatedAt        = accountCurrency.CreatedAt,
                   ModifiedAt       = accountCurrency.ModifiedAt
               };
    }

}
