using Bank.Application.Requests;
using Bank.UserService.Database.Seeders;

namespace Bank.UserService.Database.Examples;

public static partial class Example
{
    public static class Card
    {
        public static readonly CardCreateRequest CreateRequest = new()
                                                                 {
                                                                     CardTypeId = Seeder.CardType.VisaDebitCard.Id,
                                                                     AccountId  = Seeder.Account.DomesticAccount02.Id,
                                                                     Name       = "Credit Card",
                                                                     Limit      = 5000.00m,
                                                                     Status     = true
                                                                 };

        public static readonly CardUpdateStatusRequest UpdateStatusRequest = new()
                                                                             {
                                                                                 Status = false
                                                                             };

        public static readonly CardUpdateLimitRequest UpdateLimitRequest = new()
                                                                           {
                                                                               Limit = 10000.00m
                                                                           };
    }
}
