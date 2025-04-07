namespace Bank.ExchangeService.Models;

public class Quote
{
    public required Guid            Id                { get; set; }
    public          Guid?           StockId           { get; set; }
    public          Stock?          Stock             { get; set; }
    public          Guid?           ForexPairId       { get; set; }
    public          ForexPair?      ForexPair         { get; set; }
    public          Guid?           FuturesContractId { get; set; }
    public          FutureContract? FuturesContract   { get; set; }
    public          Guid?           OptionId          { get; set; }
    public          Option?         Option            { get; set; }
    public required decimal         Price             { get; set; }
    public required decimal         HighPrice         { get; set; }
    public required decimal         LowPrice          { get; set; }
    public required int             Volume            { get; set; }
    public required DateTime        CreatedAt         { get; set; }
    public required DateTime        ModifiedAt        { get; set; }
}
