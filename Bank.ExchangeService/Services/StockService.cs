using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Responses;
using Bank.ExchangeService.HttpClients;
using Bank.ExchangeService.Mappers;
using Bank.ExchangeService.Repositories;

using Result = Bank.Application.Endpoints.Result;

namespace Bank.ExchangeService.Services;

public interface IStockService
{
    Task<Result<Page<StockSimpleResponse>>> GetAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable);

    Task<Result<StockResponse>> GetOne(Guid id, QuoteFilterIntervalQuery filter);
}

public class StockService(IStockRepository stockRepository, ICurrencyClient currencyClient) : IStockService
{
    private readonly IStockRepository m_StockRepository = stockRepository;
    private readonly ICurrencyClient  m_Client          = currencyClient;

    public async Task<Result<Page<StockSimpleResponse>>> GetAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable)
    {
        var page = await m_StockRepository.FindAll(quoteFilterQuery, pageable);

        var responses = page.Items.Select(stock => stock.ToSimpleResponse())
                            .ToList();

        return Result.Ok(new Page<StockSimpleResponse>(responses, page.PageNumber, page.PageSize, page.TotalElements));
    }

    public async Task<Result<StockResponse>> GetOne(Guid id, QuoteFilterIntervalQuery filter)
    {
        var stock = await m_StockRepository.FindById(id, filter);

        if (stock is null)
            return Result.NotFound<StockResponse>($"No Stock found wih Id: {id}");

        var currencyResponse = await m_Client.GetCurrencyByIdSimple(stock.StockExchange!.CurrencyId);

        if (currencyResponse is null)
            throw new Exception($"No Currency with Id: {stock.StockExchange!.CurrencyId}");

        return Result.Ok(stock.ToResponse(currencyResponse));
    }
}
