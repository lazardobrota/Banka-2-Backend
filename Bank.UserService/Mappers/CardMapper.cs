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
}
