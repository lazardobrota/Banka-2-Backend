using Bank.Application.Responses;
using Bank.ExchangeService.Models;

namespace Bank.ExchangeService.Mappers;

public static class FutureContractMapper
{
    public static FutureContractResponse ToResponse(this FutureContract futureContract, CurrencySimpleResponse currencyResponse)
    {
        return new FutureContractResponse()
               {
                   Id             = futureContract.Id,
                   ContractSize   = futureContract.ContractSize,
                   ContractUnit   = futureContract.ContractUnit,
                   SettlementDate = futureContract.SettlementDate,
                   Name           = futureContract.Name,
                   Ticker         = futureContract.Ticker,
                   StockExchange  = futureContract.StockExchange!.ToResponse(currencyResponse),
                   Quotes = futureContract.Quotes.Select(quote => quote.ToSimpleResponse())
                                          .ToList(),
                   HighPrice                    = futureContract.HighPrice,
                   LowPrice                     = futureContract.LowPrice,
                   Volume                       = futureContract.Volume,
                   PriceChangeInInterval        = futureContract.PriceChange,
                   PriceChangePercentInInterval = futureContract.PriceChangePercent,
                   Price                        = futureContract.Price,
                   CreatedAt                    = futureContract.CreatedAt,
                   ModifiedAt                   = futureContract.ModifiedAt
               };
    }

    public static FutureContractSimpleResponse ToSimpleResponse(this FutureContract futureContract)
    {
        return new FutureContractSimpleResponse
               {
                   Id                           = futureContract.Id,
                   ContractSize                 = futureContract.ContractSize,
                   ContractUnit                 = futureContract.ContractUnit,
                   SettlementDate               = futureContract.SettlementDate,
                   Name                         = futureContract.Name,
                   Ticker                       = futureContract.Ticker,
                   HighPrice                    = futureContract.HighPrice,
                   LowPrice                     = futureContract.LowPrice,
                   Volume                       = futureContract.Volume,
                   Price                        = futureContract.Price,
                   CreatedAt                    = futureContract.CreatedAt,
                   ModifiedAt                   = futureContract.ModifiedAt,
                   PriceChangeInInterval        = futureContract.PriceChange,
                   PriceChangePercentInInterval = futureContract.PriceChangePercent
               };
    }
}
