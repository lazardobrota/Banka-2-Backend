using Bank.Application.Requests;
using Bank.Application.Responses;

namespace Bank.OpenApi.Examples;

public static partial class Example
{
    public static class Card
    {
        public static readonly CardCreateRequest CreateRequest = new()
                                                                 {
                                                                     CardTypeId = Constant.Id,
                                                                     AccountId  = Constant.Id,
                                                                     Name       = Constant.CardName,
                                                                     Limit      = Constant.DailyLimit,
                                                                     Status     = Constant.Boolean,
                                                                 };

        public static readonly CardUpdateStatusRequest StatusUpdateRequest = new()
                                                                             {
                                                                                 Status = Constant.Boolean,
                                                                             };

        public static readonly CardUpdateLimitRequest LimitUpdateRequest = new()
                                                                           {
                                                                               Limit = Constant.DailyLimit,
                                                                           };

        public static readonly CardResponse Response = new()
                                                       {
                                                           Id         = Constant.Id,
                                                           Number     = Constant.AccountNumber,
                                                           Type       = CardType.Response,
                                                           Name       = Constant.CardName,
                                                           ExpiresAt  = Constant.CreationDate,
                                                           Account    = Account.Response,
                                                           CVV        = Constant.CardCVV,
                                                           Limit      = Constant.DailyLimit,
                                                           Status     = Constant.Boolean,
                                                           CreatedAt  = Constant.CreatedAt,
                                                           ModifiedAt = Constant.ModifiedAt,
                                                       };
    }
}
