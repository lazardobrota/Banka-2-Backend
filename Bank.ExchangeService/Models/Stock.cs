using Bank.ExchangeService.Model;

namespace Bank.ExchangeService.Models;

public class Stock
{
    public required Guid           Id              { get; set; }
    public required string         Name            { get; set; }
    public required string         Ticker          { get; set; }
    public required Guid           StockExchangeId { get; set; }
    public          List<Quote>    SortedQuotes    { get; set; } = [];
    public          StockExchange? StockExchange   { get; set; }

    public decimal  HighPrice          => SortedQuotes.Count > 0 ? SortedQuotes[0].HighPrice : 0;
    public decimal  LowPrice           => SortedQuotes.Count > 0 ? SortedQuotes[0].LowPrice : 0;
    public int      Volume             => SortedQuotes.Count > 0 ? SortedQuotes[0].Volume : 0;
    public decimal  PriceChange        => SortedQuotes.Count > 0 ? CalculatePriceChange() : 0;
    public decimal  PriceChangePercent => SortedQuotes.Count > 0 ? CalculatePriceChangePercent() : 0;
    public decimal  Price              => SortedQuotes.Count > 0 ? SortedQuotes[0].Price : 0;
    public DateTime CreatedAt          => SortedQuotes.Count > 0 ? SortedQuotes[0].CreatedAt : default;
    public DateTime ModifiedAt         => SortedQuotes.Count > 0 ? SortedQuotes[0].ModifiedAt : default;

    public decimal CalculatePriceChange()
    {
        var lastPrice = SortedQuotes.First()
                                    .Price;

        var openingPrice = SortedQuotes.Last()
                                       .Price;

        return lastPrice - openingPrice;
    }

    public decimal CalculatePriceChangePercent()
    {
        var lastPrice = SortedQuotes.First()
                                    .Price;

        var openingPrice = SortedQuotes.Last()
                                       .Price;

        return (lastPrice - openingPrice) / openingPrice;
    }
}
