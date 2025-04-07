using Bank.Application.Domain;
using Bank.ExchangeService.Model;

namespace Bank.ExchangeService.Models;

public class Security
{
    public required Guid Id { get; set; }

    public required SecurityType SecurityType { get; set; }

    //All
    public required string         Name            { get; set; }
    public required string         Ticker          { get; set; }
    public required Guid           StockExchangeId { get; set; }
    public          List<Quote>    Quotes          { get; set; } = [];
    public          StockExchange? StockExchange   { get; set; }

    //ForexPair 
    public Guid       BaseCurrencyId  { get; set; }
    public Guid       QuoteCurrencyId { get; set; }
    public decimal    ExchangeRate    { get; set; }
    public Liquidity? Liquidity       { get; set; }

    //Option
    public decimal  StrikePrice       { get; set; }
    public decimal  ImpliedVolatility { get; set; }
    public int      OpenInterest      { get; set; }
    public DateOnly SettlementDate    { get; set; }

    public OptionType? OptionType { get; set; }

    //Future
    public int           ContractSize { get; set; }
    public ContractUnit? ContractUnit { get; set; }

    public decimal  HighPrice          => Quotes.Count > 0 ? Quotes[0].HighPrice : 0;
    public decimal  LowPrice           => Quotes.Count > 0 ? Quotes[0].LowPrice : 0;
    public int      Volume             => Quotes.Count > 0 ? Quotes[0].Volume : 0;
    public decimal  PriceChange        => Quotes.Count > 0 ? SecurityUtils.CalculatePriceChange(Quotes) : 0;
    public decimal  PriceChangePercent => Quotes.Count > 0 ? SecurityUtils.CalculatePriceChangePercent(Quotes) : 0;
    public decimal  Price              => Quotes.Count > 0 ? Quotes[0].Price : 0;
    public DateTime CreatedAt          => Quotes.Count > 0 ? Quotes[0].CreatedAt : default;
    public DateTime ModifiedAt         => Quotes.Count > 0 ? Quotes[0].ModifiedAt : default;
}

public class Stock
{
    public required Guid           Id              { get; set; }
    public required string         Name            { get; set; }
    public required string         Ticker          { get; set; }
    public required Guid           StockExchangeId { get; set; }
    public          List<Quote>    Quotes          { get; set; } = [];
    public          StockExchange? StockExchange   { get; set; }

    public decimal  HighPrice          => Quotes.Count > 0 ? Quotes[0].HighPrice : 0;
    public decimal  LowPrice           => Quotes.Count > 0 ? Quotes[0].LowPrice : 0;
    public int      Volume             => Quotes.Count > 0 ? Quotes[0].Volume : 0;
    public decimal  PriceChange        => Quotes.Count > 0 ? SecurityUtils.CalculatePriceChange(Quotes) : 0;
    public decimal  PriceChangePercent => Quotes.Count > 0 ? SecurityUtils.CalculatePriceChangePercent(Quotes) : 0;
    public decimal  Price              => Quotes.Count > 0 ? Quotes[0].Price : 0;
    public DateTime CreatedAt          => Quotes.Count > 0 ? Quotes[0].CreatedAt : default;
    public DateTime ModifiedAt         => Quotes.Count > 0 ? Quotes[0].ModifiedAt : default;
}

public class FutureContract
{
    public required Guid           Id              { get; set; }
    public required int            ContractSize    { get; set; }
    public required ContractUnit   ContractUnit    { get; set; }
    public required DateOnly       SettlementDate  { get; set; }
    public required string         Name            { get; set; }
    public required string         Ticker          { get; set; }
    public required Guid           StockExchangeId { get; set; }
    public          StockExchange? StockExchange   { get; set; }
    public          List<Quote>    Quotes          { get; set; } = [];

    public decimal  HighPrice          => Quotes.Count > 0 ? Quotes[0].HighPrice : 0;
    public decimal  LowPrice           => Quotes.Count > 0 ? Quotes[0].LowPrice : 0;
    public int      Volume             => Quotes.Count > 0 ? Quotes[0].Volume : 0;
    public decimal  PriceChange        => Quotes.Count > 0 ? SecurityUtils.CalculatePriceChange(Quotes) : 0;
    public decimal  PriceChangePercent => Quotes.Count > 0 ? SecurityUtils.CalculatePriceChangePercent(Quotes) : 0;
    public decimal  Price              => Quotes.Count > 0 ? Quotes[0].Price : 0;
    public DateTime CreatedAt          => Quotes.Count > 0 ? Quotes[0].CreatedAt : default;
    public DateTime ModifiedAt         => Quotes.Count > 0 ? Quotes[0].ModifiedAt : default;
}

public class Option
{
    public required Guid           Id                { get; set; }
    public required OptionType     OptionType        { get; set; }
    public required decimal        StrikePrice       { get; set; }
    public required decimal        ImpliedVolatility { get; set; }
    public required int            OpenInterest      { get; set; }
    public required DateOnly       SettlementDate    { get; set; }
    public required string         Name              { get; set; }
    public required string         Ticker            { get; set; }
    public required Guid           StockExchangeId   { get; set; }
    public          StockExchange? StockExchange     { get; set; }
    public          List<Quote>    Quotes            { get; set; } = [];

    public decimal  HighPrice          => Quotes.Count > 0 ? Quotes[0].HighPrice : 0;
    public decimal  LowPrice           => Quotes.Count > 0 ? Quotes[0].LowPrice : 0;
    public int      Volume             => Quotes.Count > 0 ? Quotes[0].Volume : 0;
    public decimal  PriceChange        => Quotes.Count > 0 ? SecurityUtils.CalculatePriceChange(Quotes) : 0;
    public decimal  PriceChangePercent => Quotes.Count > 0 ? SecurityUtils.CalculatePriceChangePercent(Quotes) : 0;
    public decimal  Price              => Quotes.Count > 0 ? Quotes[0].Price : 0;
    public DateTime CreatedAt          => Quotes.Count > 0 ? Quotes[0].CreatedAt : default;
    public DateTime ModifiedAt         => Quotes.Count > 0 ? Quotes[0].ModifiedAt : default;
}

public class ForexPair
{
    public required Guid           Id              { get; set; }
    public required Guid           BaseCurrencyId  { get; set; }
    public required Guid           QuoteCurrencyId { get; set; }
    public required decimal        ExchangeRate    { get; set; }
    public          Liquidity      Liquidity       { get; set; }
    public required int            ContractSize    { get; set; } = 1000;
    public required string         Name            { get; set; }
    public required string         Ticker          { get; set; }
    public required Guid           StockExchangeId { get; set; }
    public          StockExchange? StockExchange   { get; set; }
    public          List<Quote>    Quotes          { get; set; } = [];

    public decimal MaintenanceDecimal => (decimal)0.1 * ContractSize * ExchangeRate;

    public decimal  HighPrice          => Quotes.Count > 0 ? Quotes[0].HighPrice : 0;
    public decimal  LowPrice           => Quotes.Count > 0 ? Quotes[0].LowPrice : 0;
    public int      Volume             => Quotes.Count > 0 ? Quotes[0].Volume : 0;
    public decimal  PriceChange        => Quotes.Count > 0 ? SecurityUtils.CalculatePriceChange(Quotes) : 0;
    public decimal  PriceChangePercent => Quotes.Count > 0 ? SecurityUtils.CalculatePriceChangePercent(Quotes) : 0;
    public decimal  Price              => Quotes.Count > 0 ? Quotes[0].Price : 0;
    public DateTime CreatedAt          => Quotes.Count > 0 ? Quotes[0].CreatedAt : default;
    public DateTime ModifiedAt         => Quotes.Count > 0 ? Quotes[0].ModifiedAt : default;
}

file static class SecurityUtils
{
    public static decimal CalculatePriceChange(List<Quote> quotes)
    {
        var lastPrice = quotes.First()
                              .Price;

        var openingPrice = quotes.Last()
                                 .Price;

        return lastPrice - openingPrice;
    }

    public static decimal CalculatePriceChangePercent(List<Quote> quotes)
    {
        var lastPrice = quotes.First()
                              .Price;

        var openingPrice = quotes.Last()
                                 .Price;

        return (lastPrice - openingPrice) / openingPrice;
    }
}
