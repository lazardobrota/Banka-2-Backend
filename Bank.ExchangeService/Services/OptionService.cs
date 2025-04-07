using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Responses;
using Bank.ExchangeService.HttpClients;
using Bank.ExchangeService.Mappers;
using Bank.ExchangeService.Repositories;

namespace Bank.ExchangeService.Services;

public interface IOptionService
{
    Task<Result<Page<OptionSimpleResponse>>> GetAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable);

    Task<Result<OptionResponse>> GetOne(Guid id);
}

public class OptionService(IOptionRepository optionRepository, ICurrencyClient currencyClient) : IOptionService
{
    private readonly IOptionRepository m_OptionRepository = optionRepository;
    private readonly ICurrencyClient   m_Client           = currencyClient;

    public async Task<Result<Page<OptionSimpleResponse>>> GetAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable)
    {
        var page = await m_OptionRepository.FindAll(quoteFilterQuery, pageable);

        var responses = page.Items.Select(option => option.ToSimpleResponse())
                            .ToList();

        return Result.Ok(new Page<OptionSimpleResponse>(responses, page.PageNumber, page.PageSize, page.TotalElements));
    }

    public async Task<Result<OptionResponse>> GetOne(Guid id)
    {
        var option = await m_OptionRepository.FindById(id);

        if (option is null)
            return Result.NotFound<OptionResponse>($"No Option found with Id: {id}");

        var currencyResponse = await m_Client.GetCurrencyByIdSimple(option.StockExchange!.CurrencyId);

        if (currencyResponse is null)
            throw new Exception($"No Currency with Id: {option.StockExchange!.CurrencyId}");

        return Result.Ok(option.ToResponse(currencyResponse));
    }
}
