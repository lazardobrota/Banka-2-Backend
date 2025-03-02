using Bank.Application.Responses;
using Bank.UserService.Models;

namespace Bank.UserService.Mappers;

public static class CardMapper
{
    public static CardTypeResponse ToResponse(this CardType cardType)
    {
        return new CardTypeResponse
               {
                   Id         = cardType.Id,
                   Name       = cardType.Name,
                   CreatedAt  = cardType.CreatedAt,
                   ModifiedAt = cardType.ModifiedAt,
               };
    }

    public static CardResponse ToResponse(this Card card)
    {
        return new CardResponse
               {
                   Id         = card.Id,
                   Name       = card.Name,
                   CreatedAt  = card.CreatedAt,
                   ModifiedAt = card.ModifiedAt,
                   CVV        = card.CVV,
                   Status     = card.Status,
                   Type       = card.Type.ToResponse(),
                   ExpiresAt  = card.ExpiresAt,
                   Account    = card.Account.ToResponse(),
                   Limit      = card.Limit,
                   Number     = card.Number,
               };
    }
}
