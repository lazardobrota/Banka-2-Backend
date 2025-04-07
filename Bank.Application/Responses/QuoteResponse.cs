namespace Bank.Application.Responses;

public class QuoteResponse
{
    public Guid                          Id             { get; set; }
    public StockSimpleResponse?          Stock          { get; set; }
    public OptionSimpleResponse?         Option         { get; set; }
    public ForexPairSimpleResponse?      ForexPair      { get; set; }
    public FutureContractSimpleResponse? FutureContract { get; set; }
    public decimal                       Price          { get; set; }
    public decimal                       HighPrice      { get; set; }
    public decimal                       LowPrice       { get; set; }
    public int                           Count          { get; set; }
    public DateTime                      CreatedAt      { get; set; }
    public DateTime                      ModifiedAt     { get; set; }
}

public class QuoteCreateRequest
{
    public Guid    Listing      { get; set; }
    public decimal ClosingPrice { get; set; }
    public decimal HighPrice    { get; set; }
    public decimal LowPrice     { get; set; }
    public int     Count        { get; set; }
}

public class QuoteSimpleResponse
{
    public required Guid     Id         { get; set; }
    public required decimal  Price      { get; set; }
    public required decimal  HighPrice  { get; set; }
    public required decimal  LowPrice   { get; set; }
    public required int      Volume     { get; set; }
    public required DateTime CreatedAt  { get; set; }
    public required DateTime ModifiedAt { get; set; }
}
