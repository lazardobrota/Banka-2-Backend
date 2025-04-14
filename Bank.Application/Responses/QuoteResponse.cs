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
    public long                          Volume         { get; set; }
    public DateTime                      CreatedAt      { get; set; }
    public DateTime                      ModifiedAt     { get; set; }
}

public class QuoteSimpleResponse
{
    public required Guid     Id         { get; set; }
    public required decimal  HighPrice  { get; set; }
    public required decimal  LowPrice   { get; set; }
    public required decimal  AskPrice   { get; set; }
    public required decimal  BidPrice   { get; set; }
    public required long     Volume     { get; set; }
    public required DateTime CreatedAt  { get; set; }
    public required DateTime ModifiedAt { get; set; }
}

public class QuoteLatestSimpleResponse
{
    public required string   SecurityTicker { get; set; }
    public required decimal  AskPrice       { get; set; }
    public required decimal  BidPrice       { get; set; }
    public required decimal  HighPrice      { get; set; }
    public required decimal  LowPrice       { get; set; }
    public required long     Volume         { get; set; }
    public required DateTime CreatedAt      { get; set; }
    public required DateTime ModifiedAt     { get; set; }
}

public class QuoteDailySimpleResponse
{
    public required decimal  HighPrice  { get; set; }
    public required decimal  LowPrice   { get; set; }
    public required decimal  OpenPrice  { get; set; }
    public required decimal  ClosePrice { get; set; }
    public required long     Volume     { get; set; }
    public required DateOnly CreatedAt  { get; set; }
    public required DateOnly ModifiedAt { get; set; }
}
