using Bank.Application.Domain;

namespace Bank.Application.Responses;

public class ForexPairResponse
{
    public required Guid                      Id                           { get; set; }
    public required Liquidity                 Liquidity                    { get; set; }
    public required CurrencySimpleResponse    BaseCurrency                 { get; set; }
    public required CurrencySimpleResponse    QuoteCurrency                { get; set; }
    public required decimal                   MaintenanceDecimal           { get; set; }
    public required decimal                   ExchangeRate                 { get; set; }
    public required string                    Name                         { get; set; }
    public required string                    Ticker                       { get; set; }
    public required decimal                   HighPrice                    { get; set; }
    public required decimal                   LowPrice                     { get; set; }
    public required int                       Volume                       { get; set; }
    public required decimal                   PriceChangeInInterval        { get; set; }
    public required decimal                   PriceChangePercentInInterval { get; set; }
    public required decimal                   Price                        { get; set; }
    public required DateTime                  CreatedAt                    { get; set; }
    public required DateTime                  ModifiedAt                   { get; set; }
    public required StockExchangeResponse     StockExchange                { get; set; }
    public required List<QuoteSimpleResponse> Quotes                       { get; set; } = [];
}

public class ForexPairSimpleResponse
{
    public required Guid                   Id                           { get; set; }
    public required Liquidity              Liquidity                    { get; set; }
    public required CurrencySimpleResponse BaseCurrency                 { get; set; }
    public required CurrencySimpleResponse QuoteCurrency                { get; set; }
    public required decimal                ExchangeRate                 { get; set; }
    public required decimal                MaintenanceDecimal           { get; set; }
    public required string                 Name                         { get; set; }
    public required string                 Ticker                       { get; set; }
    public required decimal                HighPrice                    { get; set; }
    public required decimal                LowPrice                     { get; set; }
    public required decimal                Count                        { get; set; }
    public required decimal                PriceChangeInInterval        { get; set; }
    public required decimal                PriceChangePercentInInterval { get; set; }
    public required decimal                Price                        { get; set; }
    public required DateTime               CreatedAt                    { get; set; }
    public required DateTime               ModifiedAt                   { get; set; }
}
