using System.Text.Json.Serialization;

namespace Bank.Application.Responses;

public class StockResponse 
{
    public required Guid Id { get; set; }

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

public class StockSimpleResponse
{
    public required Guid     Id                           { get; set; }
    public required string   Name                         { get; set; }
    public required string   Ticker                       { get; set; }
    public required decimal  HighPrice                    { get; set; }
    public required decimal  LowPrice                     { get; set; }
    public required int      Volume                       { get; set; }
    public required decimal  PriceChangeInInterval        { get; set; }
    public required decimal  PriceChangePercentInInterval { get; set; }
    public required decimal  Price                        { get; set; }
    public required DateTime CreatedAt                    { get; set; }
    public required DateTime ModifiedAt                   { get; set; }
}

public class FetchStockBarResponse
{
    [JsonPropertyName("next_page_token")]
    public string? NextPage { get; set; }

    public required Dictionary<string, List<FetchBarResponse>> Bars { get; set; }
}

public class FetchStockBarLatestResponse
{
    public required Dictionary<string, FetchBarResponse> Bars { get; set; }
}

public class FetchBarResponse
{
    [JsonPropertyName("c")]
    public required decimal LatestPrice { get; set; }

    [JsonPropertyName("h")]
    public required decimal HighPrice { get; set; }

    [JsonPropertyName("l")]
    public required decimal LowPrice { get; set; }

    [JsonPropertyName("n")]
    public required int NumberOfTradesInInterval { get; set; }

    [JsonPropertyName("o")]
    public required decimal OpeningPrice { get; set; }

    [JsonPropertyName("t")]
    public required DateTime Date { get; set; }

    [JsonPropertyName("v")]
    public required long NumberOfSharesInInterval { get; set; }

    [JsonPropertyName("vw")]
    public required decimal VolumeWeightedAveragePrice { get; set; }
}

public class FetchStockResponse
{
    [JsonPropertyName("exchange")]
    public required string StockExchangeAcronym { get; set; }

    [JsonPropertyName("symbol")]
    public required string Ticker { get; set; }

    public required string Name     { get; set; }
    public required bool   Tradable { get; set; }
}
