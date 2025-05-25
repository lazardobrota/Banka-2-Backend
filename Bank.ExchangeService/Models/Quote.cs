namespace Bank.ExchangeService.Models;

public class Quote
{
    public required Guid      Id                { get; set; }
    public required Guid      SecurityId        { get; set; }
    public          Security? Security          { get; set; }
    public required decimal   AskPrice          { get; set; }
    public required decimal   BidPrice          { get; set; }
    public required int       AskSize           { get; set; }
    public required int       BidSize           { get; set; }
    public required decimal   HighPrice         { get; set; }
    public required decimal   LowPrice          { get; set; }
    public required decimal   ClosePrice        { get; set; }
    public required decimal   OpeningPrice      { get; set; }
    public          decimal   ImpliedVolatility { get; set; }
    public required long      Volume            { get; set; }
    public required int       ContractCount     { get; set; }

    public required DateTime CreatedAt  { get; set; }
    public required DateTime ModifiedAt { get; set; }
}

public class DailyQuote
{
    public required decimal  HighPrice    { get; set; }
    public required decimal  LowPrice     { get; set; }
    public required decimal  ClosePrice   { get; set; }
    public required decimal  OpeningPrice { get; set; }
    public required long     Volume       { get; set; }
    public required DateTime Date         { get; set; }
}
