using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Responses;
using Bank.ExchangeService.Extensions;
using Bank.ExchangeService.Mappers;
using Bank.ExchangeService.Repositories;

using Result = Bank.Application.Endpoints.Result;

namespace Bank.ExchangeService.Services;

public interface IForexPairService
{
    Task<Result<Page<ForexPairSimpleResponse>>> GetAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable);

    Task<Result<ForexPairResponse>> GetOne(Guid id, QuoteFilterIntervalQuery filter);

    Task<Result<ForexPairDailyResponse>> GetOneDaily(Guid id, QuoteFilterIntervalQuery filter);
}

public class ForexPairService(ISecurityRepository securityRepository, IHttpClientFactory httpClientFactory) : IForexPairService
{
    private readonly ISecurityRepository m_SecurityRepository = securityRepository;
    private readonly IHttpClientFactory  m_HttpClientFactory  = httpClientFactory;

    public async Task<Result<Page<ForexPairSimpleResponse>>> GetAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable)
    {
        var page = await m_SecurityRepository.FindAll(quoteFilterQuery, SecurityType.ForexPair, pageable);

        using var httpClient = m_HttpClientFactory.CreateClient();

        var list = httpClient.GetAllCurrenciesSimple();

        if (list.Result is null)
            throw new Exception("There are no currencies in a database");

        var response = page.Items.Select(security => security.ToForexPair()
                                                             .ToSimpleResponse(list.Result.Find(curr => curr.Id == security.BaseCurrencyId)!,
                                                                               list.Result.Find(curr => curr.Id == security.QuoteCurrencyId)!))
                           .ToList();

        return Result.Ok(new Page<ForexPairSimpleResponse>(response, page.PageNumber, page.PageSize, page.TotalElements));
    }

    public async Task<Result<ForexPairResponse>> GetOne(Guid id, QuoteFilterIntervalQuery filter)
    {
        var forexPair = await m_SecurityRepository.FindById(id, filter);

        if (forexPair is null)
            return Result.NotFound<ForexPairResponse>($"No Forex pair found wih Id: {id}");

        using var httpClient = m_HttpClientFactory.CreateClient();

        var currencyBaseResponseTask  = httpClient.GetCurrencyByIdSimple(forexPair.BaseCurrencyId);
        var currencyQuoteResponseTask = httpClient.GetCurrencyByIdSimple(forexPair.QuoteCurrencyId);
        var currencyResponseTask      = httpClient.GetCurrencyByIdSimple(forexPair.StockExchange!.CurrencyId);

        Task.WaitAll(currencyBaseResponseTask, currencyQuoteResponseTask, currencyResponseTask);

        var currencyBaseResponse  = currencyBaseResponseTask.Result;
        var currencyQuoteResponse = currencyQuoteResponseTask.Result;
        var currencyResponse      = currencyResponseTask.Result;

        if (currencyBaseResponse is null)
            throw new Exception($"No Currency with Id: {forexPair.BaseCurrencyId}");

        if (currencyQuoteResponse is null)
            throw new Exception($"No Currency with Id: {forexPair.QuoteCurrencyId}");

        if (currencyResponse is null)
            throw new Exception($"No Currency with Id: {forexPair.StockExchange!.CurrencyId}");

        return Result.Ok(forexPair.ToForexPair()
                                  .ToResponse(currencyResponse, currencyBaseResponse, currencyQuoteResponse));
    }

    public async Task<Result<ForexPairDailyResponse>> GetOneDaily(Guid id, QuoteFilterIntervalQuery filter)
    {
        var forexPair = await m_SecurityRepository.FindByIdDaily(id, filter);

        if (forexPair is null)
            return Result.NotFound<ForexPairDailyResponse>($"No Forex pair found wih Id: {id}");

        using var httpClient = m_HttpClientFactory.CreateClient();

        var currencyBaseResponseTask  = httpClient.GetCurrencyByIdSimple(forexPair.BaseCurrencyId);
        var currencyQuoteResponseTask = httpClient.GetCurrencyByIdSimple(forexPair.QuoteCurrencyId);
        var currencyResponseTask      = httpClient.GetCurrencyByIdSimple(forexPair.StockExchange!.CurrencyId);

        Task.WaitAll(currencyBaseResponseTask, currencyQuoteResponseTask, currencyResponseTask);

        var currencyBaseResponse  = currencyBaseResponseTask.Result;
        var currencyQuoteResponse = currencyQuoteResponseTask.Result;
        var currencyResponse      = currencyResponseTask.Result;

        if (currencyBaseResponse is null)
            throw new Exception($"No Currency with Id: {forexPair.BaseCurrencyId}");

        if (currencyQuoteResponse is null)
            throw new Exception($"No Currency with Id: {forexPair.QuoteCurrencyId}");

        if (currencyResponse is null)
            throw new Exception($"No Currency with Id: {forexPair.StockExchange!.CurrencyId}");

        return Result.Ok(forexPair.ToForexPair()
                                  .ToCandleResponse(currencyResponse, currencyBaseResponse, currencyQuoteResponse));
    }
}
