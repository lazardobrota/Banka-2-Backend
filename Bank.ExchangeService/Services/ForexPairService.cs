using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Responses;
using Bank.ExchangeService.HttpClients;
using Bank.ExchangeService.Mappers;
using Bank.ExchangeService.Repositories;

using Result = Bank.Application.Endpoints.Result;

namespace Bank.ExchangeService.Services;

public interface IForexPairService
{
    Task<Result<Page<ForexPairSimpleResponse>>> GetAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable);

    Task<Result<ForexPairResponse>> GetOne(Guid id);
}

public class ForexPairService(IForexPairRepository forexPairRepository, ICurrencyClient currencyClient) : IForexPairService
{
    private readonly IForexPairRepository m_ForexPairRepository = forexPairRepository;
    private readonly ICurrencyClient      m_CurrencyClient      = currencyClient;

    public async Task<Result<Page<ForexPairSimpleResponse>>> GetAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable)
    {
        var page = await m_ForexPairRepository.FindAll(quoteFilterQuery, pageable);

        var list = m_CurrencyClient.GetAllCurrenciesSimple();

        if (list.Result is null)
            throw new Exception("There are no currencies in a database");

        var response = page
                       .Items.Select(pair => pair.ToSimpleResponse(list.Result.Find(curr => curr.Id == pair.BaseCurrencyId)!,
                                                                   list.Result.Find(curr => curr.Id == pair.QuoteCurrencyId)!))
                       .ToList();

        return Result.Ok(new Page<ForexPairSimpleResponse>(response, page.PageNumber, page.PageSize, page.TotalElements));
    }

    public async Task<Result<ForexPairResponse>> GetOne(Guid id)
    {
        var forexPair = await m_ForexPairRepository.FindById(id);

        if (forexPair is null)
            return Result.NotFound<ForexPairResponse>($"No Forex pair found wih Id: {id}");

        var currencyBaseResponseTask  = Task.Run(() => m_CurrencyClient.GetCurrencyByIdSimple(forexPair.BaseCurrencyId));
        var currencyQuoteResponseTask = Task.Run(() => m_CurrencyClient.GetCurrencyByIdSimple(forexPair.QuoteCurrencyId));
        var currencyResponseTask      = Task.Run(() => m_CurrencyClient.GetCurrencyByIdSimple(forexPair.StockExchange!.CurrencyId));

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

        return Result.Ok(forexPair.ToResponse(currencyResponse, currencyBaseResponse, currencyQuoteResponse));
    }
}
