using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Responses;
using Bank.ExchangeService.Extensions;
using Bank.ExchangeService.Mappers;
using Bank.ExchangeService.Repositories;

namespace Bank.ExchangeService.Services;

public interface IStockExchangeService
{
    Task<Result<Page<StockExchangeResponse>>> GetAll(StockExchangeFilterQuery filter, Pageable pageable);

    Task<Result<StockExchangeResponse>> GetOne(Guid id);

    Task<Result<StockExchangeResponse>> GetOne(string acronym);

    Task<Result<StockExchangeResponse>> Create(ExchangeCreateRequest request);
}

public class StockExchangeService(IStockExchangeRepository stockExchangeRepository, IHttpClientFactory httpClientFactory) : IStockExchangeService
{
    private readonly IStockExchangeRepository m_Repo              = stockExchangeRepository;
    private readonly IHttpClientFactory       m_HttpClientFactory = httpClientFactory;

    public async Task<Result<Page<StockExchangeResponse>>> GetAll(StockExchangeFilterQuery filter, Pageable pageable)
    {
        var page = await m_Repo.FindAll(filter, pageable);

        var currencyIds = page.Items.Select(se => se.CurrencyId)
                              .Distinct();

        using var httpClient = m_HttpClientFactory.CreateClient();

        var currencyTasks = currencyIds.Select(id => httpClient.GetCurrencyByIdSimple(id))
                                       .ToList();

        var currencies = await Task.WhenAll(currencyTasks);

        var currencyMap = currencies.Where(c => c != null)
                                    .ToDictionary(c => c!.Id, c => c!);

        var responses = page.Items.Where(se => currencyMap.ContainsKey(se.CurrencyId))
                            .Select(se => se.ToResponse(currencyMap[se.CurrencyId]))
                            .ToList();

        return Result.Ok(new Page<StockExchangeResponse>(responses, page.PageNumber, page.PageSize, page.TotalElements));
    }

    public async Task<Result<StockExchangeResponse>> GetOne(Guid id)
    {
        var stockExchange = await m_Repo.FindById(id);

        if (stockExchange is null)
            return Result.NotFound<StockExchangeResponse>($"No StockExchange found with ID: {id}");

        using var httpClient = m_HttpClientFactory.CreateClient();
        var       currency   = await httpClient.GetCurrencyByIdSimple(stockExchange.CurrencyId);

        if (currency is null)
            throw new Exception($"StockExchange currency doesn't exist with id: {stockExchange.CurrencyId}");

        return Result.Ok(stockExchange.ToResponse(currency));
    }

    public async Task<Result<StockExchangeResponse>> GetOne(string acronym)
    {
        var stockExchange = await m_Repo.FindByAcronym(acronym);

        if (stockExchange is null)
            Result.NotFound<StockExchangeResponse>($"No StockExchange found with acronym: {acronym}");

        using var httpClient = m_HttpClientFactory.CreateClient();
        var       currency   = await httpClient.GetCurrencyByIdSimple(stockExchange!.CurrencyId);

        if (currency is null)
            throw new Exception($"StockExchange currency doesn't exist with id: {stockExchange.CurrencyId}");

        return Result.Ok(stockExchange.ToResponse(currency));
    }

    public async Task<Result<StockExchangeResponse>> Create(ExchangeCreateRequest request)
    {
        using var httpClient = m_HttpClientFactory.CreateClient();
        var       currency   = await httpClient.GetCurrencyByIdSimple(request.CurrencyId);

        if (currency == null)
            return Result.NotFound<StockExchangeResponse>($"Currency not found: {request.CurrencyId}");

        var entity  = request.ToStockExchange();
        var created = await m_Repo.Add(entity);

        return Result.Ok(created.ToResponse(currency));
    }
}
