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

public class StockService(ISecurityRepository securityRepository, ICurrencyClient currencyClient) : IStockService
{
    private readonly ISecurityRepository m_SecurityRepository = securityRepository;
    private readonly ICurrencyClient     m_Client             = currencyClient;

    public async Task<Result<Page<StockSimpleResponse>>> GetAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable)
    {
        var page = await m_SecurityRepository.FindAll(quoteFilterQuery, SecurityType.Stock, pageable);

        var responses = page.Items.Select(security => security.ToStock()
                                                              .ToSimpleResponse())
                            .ToList();

        return Result.Ok(new Page<StockSimpleResponse>(responses, page.PageNumber, page.PageSize, page.TotalElements));
    }

    public async Task<Result<StockResponse>> GetOne(Guid id, QuoteFilterIntervalQuery filter)
    {
        var security = await m_SecurityRepository.FindById(id, filter);

        if (security is null)
            return Result.NotFound<StockResponse>($"No Stock found wih Id: {id}");

        var currencyResponse = await m_Client.GetCurrencyByIdSimple(security.StockExchange!.CurrencyId);

        if (currencyResponse is null)
            throw new Exception($"No Currency with Id: {security.StockExchange!.CurrencyId}");

        return Result.Ok(security.ToStock()
                                 .ToResponse(currencyResponse));
    }
}
