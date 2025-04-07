using Bank.Application.Domain;

namespace Bank.Application.Responses;

public class OptionResponse
{
    public required Guid                      Id                           { get; set; }
    public required decimal                   StrikePrice                  { get; set; }
    public required decimal                   ImpliedVolatility            { get; set; }
    public required int                       OpenInterest                 { get; set; }
    public required DateTime                  SettlementDate               { get; set; }
    public required string                    Name                         { get; set; }
    public required string                    Ticker                       { get; set; }
    public required OptionType                OptionType                   { get; set; }
    public required StockExchangeResponse     StockExchange                { get; set; }
    public required List<QuoteSimpleResponse> SortedQuotes                 { get; set; } = [];
    public          decimal                   HighPrice                    { get; set; }
    public          decimal                   LowPrice                     { get; set; }
    public          int                       Volume                       { get; set; }
    public required decimal                   PriceChangeInInterval        { get; set; }
    public required decimal                   PriceChangePercentInInterval { get; set; }
    public          decimal                   Price                        { get; set; }
    public          DateTime                  CreatedAt                    { get; set; }
    public          DateTime                  ModifiedAt                   { get; set; }
}

public class OptionSimpleResponse
{
    public required Guid       Id                           { get; set; }
    public required decimal    StrikePrice                  { get; set; }
    public required decimal    ImpliedVolatility            { get; set; }
    public required int        OpenInterest                 { get; set; }
    public required DateTime   SettlementDate               { get; set; }
    public required string     Name                         { get; set; }
    public required string     Ticker                       { get; set; }
    public required OptionType OptionType                   { get; set; }
    public          decimal    HighPrice                    { get; set; }
    public          decimal    LowPrice                     { get; set; }
    public          int        Volume                       { get; set; }
    public          decimal    PriceChange                  { get; set; }
    public required decimal    PriceChangeInInterval        { get; set; }
    public required decimal    PriceChangePercentInInterval { get; set; }
    public          decimal    Price                        { get; set; }
    public          DateTime   CreatedAt                    { get; set; }
    public          DateTime   ModifiedAt                   { get; set; }
}
