using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Responses;
using Bank.ExchangeService.HttpClients;
using Bank.ExchangeService.Mappers;
using Bank.ExchangeService.Repositories;

using Result = Bank.Application.Endpoints.Result;

namespace Bank.ExchangeService.Services;

public interface IFutureContractService
{
    Task<Result<Page<FutureContractSimpleResponse>>> GetAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable);

    Task<Result<FutureContractResponse>> GetOne(Guid id);
}

public class FutureContractService(IFutureContractRepository iFutureContractRepository, ICurrencyClient currencyClient) : IFutureContractService
{
    private readonly IFutureContractRepository m_FutureContractRepository = iFutureContractRepository;
    private readonly ICurrencyClient           m_CurrencyClient           = currencyClient;

    public async Task<Result<Page<FutureContractSimpleResponse>>> GetAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable)
    {
        var page = await m_FutureContractRepository.FindAll(quoteFilterQuery, pageable);

        var responses = page.Items.Select(contract => contract.ToSimpleResponse())
                            .ToList();

        return Result.Ok(new Page<FutureContractSimpleResponse>(responses, page.PageNumber, page.PageSize, page.TotalElements));
    }

    public async Task<Result<FutureContractResponse>> GetOne(Guid id)
    {
        var futureContract = await m_FutureContractRepository.FindById(id);

        if (futureContract is null)
            return Result.NotFound<FutureContractResponse>($"No Stock found wih Id: {id}");

        var currencyResponse = await m_CurrencyClient.GetCurrencyByIdSimple(futureContract.StockExchange!.CurrencyId);

        if (currencyResponse is null)
            throw new Exception($"No Currency with Id: {futureContract.StockExchange!.CurrencyId}");

        return Result.Ok(futureContract.ToResponse(currencyResponse));
    }
}
