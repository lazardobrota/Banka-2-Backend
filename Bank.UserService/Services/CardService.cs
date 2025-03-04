using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Responses;
using Bank.UserService.Mappers;
using Bank.UserService.Repositories;

namespace Bank.UserService.Services;

public interface ICardService
{
    Task<Result<Page<CardResponse>>> GetAll(CardFilterQuery userFilterQuery, Pageable pageable);

    Task<Result<CardResponse>> GetOne(Guid id);
}

public class CardService(ICardRepository repository) : ICardService
{
    private readonly ICardRepository m_CardRepository = repository;

    public async Task<Result<Page<CardResponse>>> GetAll(CardFilterQuery cardFilterQuery, Pageable pageable)
    {
        var cards = await m_CardRepository.FindAll(cardFilterQuery, pageable);

        var result = cards.Items.Select(card => card.ToResponse())
                          .ToList();

        return Result.Ok(new Page<CardResponse>(result, cards.PageNumber, cards.PageSize, cards.TotalElements));
    }

    public async Task<Result<CardResponse>> GetOne(Guid id)
    {
        var card = await m_CardRepository.FindById(id);

        if (card == null)
            return Result.NotFound<CardResponse>();

        return Result.Ok(card.ToResponse());
    }
}
