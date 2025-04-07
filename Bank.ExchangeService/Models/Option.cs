using Bank.Application.Domain;
using Bank.ExchangeService.Model;

namespace Bank.ExchangeService.Models;

public class Option
{
    public required Guid           Id                { get; set; }
    public required OptionType     OptionType        { get; set; }
    public required decimal        StrikePrice       { get; set; }
    public required decimal        ImpliedVolatility { get; set; }
    public required int            OpenInterest      { get; set; }
    public required DateTime       SettlementDate    { get; set; }
    public required string         Name              { get; set; }
    public required string         Ticker            { get; set; }
    public required Guid           StockExchangeId   { get; set; }
    public          StockExchange? StockExchange     { get; set; }
    public          List<Quote>    SortedQuotes      { get; set; } = [];

    public decimal  HighPrice          => SortedQuotes.Count > 0 ? SortedQuotes[0].HighPrice : 0;
    public decimal  LowPrice           => SortedQuotes.Count > 0 ? SortedQuotes[0].LowPrice : 0;
    public int      Volume             => SortedQuotes.Count > 0 ? SortedQuotes[0].Volume : 0;
    public decimal  PriceChange        => SortedQuotes.Count > 0 ? CalculatePriceChange() : 0;
    public decimal  PriceChangePercent => SortedQuotes.Count > 0 ? CalculatePriceChangePercent() : 0;
    public decimal  Price              => SortedQuotes.Count > 0 ? SortedQuotes[0].Price : 0;
    public DateTime CreatedAt          => SortedQuotes.Count > 0 ? SortedQuotes[0].CreatedAt : default;
    public DateTime ModifiedAt         => SortedQuotes.Count > 0 ? SortedQuotes[0].ModifiedAt : default;

    public int ContractSize => 100;
    // public          int        MaintenanceMargin => ContractSize *;

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
