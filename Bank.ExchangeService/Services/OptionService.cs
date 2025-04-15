using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Responses;
using Bank.ExchangeService.Extensions;
using Bank.ExchangeService.Mappers;
using Bank.ExchangeService.Repositories;

namespace Bank.ExchangeService.Services;

public interface IOptionService
{
    Task<Result<Page<OptionSimpleResponse>>> GetAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable);

    Task<Result<OptionResponse>> GetOne(Guid id, QuoteFilterIntervalQuery filter);

    Task<Result<OptionDailyResponse>> GetOneDaily(Guid id, QuoteFilterIntervalQuery filter);
}

public class OptionService(ISecurityRepository securityRepository, IHttpClientFactory httpClientFactory) : IOptionService
{
    private readonly ISecurityRepository m_SecurityRepository = securityRepository;
    private readonly IHttpClientFactory  m_HttpClientFactory  = httpClientFactory;

    public async Task<Result<Page<OptionSimpleResponse>>> GetAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable)
    {
        var page = await m_SecurityRepository.FindAll(quoteFilterQuery, SecurityType.Option, pageable, false);

        var responses = page.Items.Select(security => security.ToOption()
                                                              .ToSimpleResponse())
                            .ToList();

        return Result.Ok(new Page<OptionSimpleResponse>(responses, page.PageNumber, page.PageSize, page.TotalElements));
    }

    public async Task<Result<OptionResponse>> GetOne(Guid id, QuoteFilterIntervalQuery filter)
    {
        var security = await m_SecurityRepository.FindById(id, filter);

        if (security is null)
            return Result.NotFound<OptionResponse>($"No Option found with Id: {id}");

        using var httpClient = m_HttpClientFactory.CreateClient();

        var currencyResponse = await httpClient.GetCurrencyByIdSimple(security.StockExchange!.CurrencyId);

        if (currencyResponse is null)
            throw new Exception($"No Currency with Id: {security.StockExchange!.CurrencyId}");

        return Result.Ok(security.ToOption()
                                 .ToResponse(currencyResponse));
    }

    public async Task<Result<OptionDailyResponse>> GetOneDaily(Guid id, QuoteFilterIntervalQuery filter)
    {
        var security = await m_SecurityRepository.FindByIdDaily(id, filter);

        if (security is null)
            return Result.NotFound<OptionDailyResponse>($"No Option found with Id: {id}");

        using var httpClient = m_HttpClientFactory.CreateClient();

        var currencyResponse = await httpClient.GetCurrencyByIdSimple(security.StockExchange!.CurrencyId);

        if (currencyResponse is null)
            throw new Exception($"No Currency with Id: {security.StockExchange!.CurrencyId}");

        return Result.Ok(security.ToOption()
                                 .ToCandleResponse(currencyResponse));
    }
}
