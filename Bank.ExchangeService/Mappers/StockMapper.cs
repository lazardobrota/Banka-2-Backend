using Bank.Application.Responses;
using Bank.ExchangeService.Models;

namespace Bank.ExchangeService.Mappers;

public static class StockMapper
{
    public static StockResponse ToResponse(this Stock stock, CurrencySimpleResponse currencyResponse)
    {
        return new StockResponse
               {
                   Id            = stock.Id,
                   Name          = stock.Name,
                   Ticker        = stock.Ticker,
                   StockExchange = stock.StockExchange!.ToResponse(currencyResponse),
                   Quotes = stock.SortedQuotes.Select(quote => quote.ToSimpleResponse())
                                 .ToList(),
                   HighPrice                    = stock.HighPrice,
                   LowPrice                     = stock.LowPrice,
                   Volume                       = stock.Volume,
                   PriceChangeInInterval        = stock.PriceChange,
                   PriceChangePercentInInterval = stock.PriceChangePercent,
                   Price                        = stock.Price,
                   CreatedAt                    = stock.CreatedAt,
                   ModifiedAt                   = stock.ModifiedAt,
               };
    }

    public static StockSimpleResponse ToSimpleResponse(this Stock stock)
    {
        return new StockSimpleResponse
               {
                   Id                           = stock.Id,
                   Name                         = stock.Name,
                   Ticker                       = stock.Ticker,
                   HighPrice                    = stock.HighPrice,
                   LowPrice                     = stock.LowPrice,
                   Volume                       = stock.Volume,
                   PriceChangeInInterval        = stock.PriceChange,
                   PriceChangePercentInInterval = stock.PriceChangePercent,
                   Price                        = stock.Price,
                   CreatedAt                    = stock.CreatedAt,
                   ModifiedAt                   = stock.ModifiedAt
               };
    }

    public static Stock ToStock(this FetchStockResponse stockResponse, Guid stockExchangeId)
    {
        return new Stock
               {
                   Id              = Guid.NewGuid(),
                   Name            = stockResponse.Name,
                   Ticker          = stockResponse.Ticker,
                   StockExchangeId = stockExchangeId,
               };
    }
}
