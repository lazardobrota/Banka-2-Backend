using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.UserService.Models;
using Bank.UserService.Utils;

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

    public static Card ToCard(this CardCreateRequest card, CardType type, Account account)
    {
        var (cardNumber, cvv) = CardNumberGeneratorUtil.GenerateCardDetails(type.Name);

        return new Card
               {
                   Id         = Guid.NewGuid(),
                   TypeId     = card.CardTypeId,
                   Name       = card.Name,
                   Limit      = card.Limit,
                   Status     = card.Status,
                   CreatedAt  = DateTime.UtcNow,
                   ModifiedAt = DateTime.UtcNow,
                   Number     = cardNumber,
                   CVV        = cvv,
                   ExpiresAt  = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)),
                   Account    = account,
                   Type       = type,
                   AccountId  = account.Id,
               };
    }

    public static Card ToCard(this CardUpdateStatusRequest cardUpdate, Card oldCard)
    {
        return new Card
               {
                   Id         = oldCard.Id,
                   Name       = oldCard.Name,
                   CreatedAt  = oldCard.CreatedAt,
                   ModifiedAt = oldCard.ModifiedAt,
                   CVV        = oldCard.CVV,
                   Status     = cardUpdate.Status,
                   Type       = oldCard.Type,
                   TypeId     = oldCard.Type.Id,
                   ExpiresAt  = oldCard.ExpiresAt,
                   Account    = oldCard.Account,
                   AccountId  = oldCard.Account.Id,
                   Limit      = oldCard.Limit,
                   Number     = oldCard.Number
               };
    }

    public static Card ToCard(this CardUpdateLimitRequest cardUpdate, Card oldCard)
    {
        return new Card
               {
                   Id         = oldCard.Id,
                   Name       = oldCard.Name,
                   CreatedAt  = oldCard.CreatedAt,
                   ModifiedAt = oldCard.ModifiedAt,
                   CVV        = oldCard.CVV,
                   Status     = oldCard.Status,
                   Type       = oldCard.Type,
                   TypeId     = oldCard.Type.Id,
                   ExpiresAt  = oldCard.ExpiresAt,
                   Account    = oldCard.Account,
                   AccountId  = oldCard.Account.Id,
                   Limit      = cardUpdate.Limit,
                   Number     = oldCard.Number
               };
    }
}
