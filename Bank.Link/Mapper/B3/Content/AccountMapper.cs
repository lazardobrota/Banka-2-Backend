using Bank.Application.Responses;
using Bank.Link.Responses;

namespace Bank.Link.Mapper.B3.Content;

internal static class AccountMapper
{
    internal static AccountResponse ToNative(this Response.B3.AccountResponse response, CurrencyResponse currencyResponse, AccountTypeResponse accountTypeResponse)
    {
        return new AccountResponse
               {
                   Id                = Guid.Empty,
                   AccountNumber     = response.AccountNumber,
                   Name              = response.Name,
                   Client            = response.Owner.ToSimpleNative(),
                   Balance           = response.Balance,
                   AvailableBalance  = response.AvailableBalance,
                   Employee          = null!,
                   Currency          = currencyResponse,
                   Type              = accountTypeResponse,
                   AccountCurrencies = [],
                   DailyLimit        = response.DailyLimit,
                   MonthlyLimit      = response.MonthlyLimit,
                   CreationDate      = default,
                   ExpirationDate    = default,
                   Status            = false,
                   CreatedAt         = default,
                   ModifiedAt        = default
               };
    }
}
