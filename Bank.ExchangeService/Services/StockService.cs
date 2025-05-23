using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Responses;
using Bank.ExchangeService.Mappers;
using Bank.ExchangeService.Repositories;
using Bank.Http.Clients.User;

using Result = Bank.Application.Endpoints.Result;

namespace Bank.ExchangeService.Services;

public interface IStockService
{
    Task<Result<Page<StockSimpleResponse>>> GetAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable);

    Task<Result<StockResponse>> GetOne(Guid id, QuoteFilterIntervalQuery filter);

    Task<Result<StockDailyResponse>> GetOneDaily(Guid id, QuoteFilterIntervalQuery filter);
}

public class StockService(ISecurityRepository securityRepository, IUserServiceHttpClient userServiceHttpClient) : IStockService
{
    private readonly ISecurityRepository    m_SecurityRepository    = securityRepository;
    private readonly IUserServiceHttpClient m_UserServiceHttpClient = userServiceHttpClient;

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

        var currencyResponse = await m_UserServiceHttpClient.GetOneSimpleCurrency(security.StockExchange!.CurrencyId);

        if (currencyResponse is null)
            throw new Exception($"No Currency with Id: {security.StockExchange!.CurrencyId}");

        return Result.Ok(security.ToStock()
                                 .ToResponse(currencyResponse));
    }

    public async Task<Result<StockDailyResponse>> GetOneDaily(Guid id, QuoteFilterIntervalQuery filter)
    {
        var security = await m_SecurityRepository.FindByIdDaily(id, filter);

        if (security is null)
            return Result.NotFound<StockDailyResponse>($"No Stock found wih Id: {id}");

        var currencyResponse = await m_UserServiceHttpClient.GetOneSimpleCurrency(security.StockExchange!.CurrencyId);

        if (currencyResponse is null)
            throw new Exception($"No Currency with Id: {security.StockExchange!.CurrencyId}");

        return Result.Ok(security.ToStock()
                                 .ToDailyResponse(currencyResponse));
    }
}
