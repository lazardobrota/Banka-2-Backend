namespace Bank.ExchangeService.Models;

public class Quote
{
    //TODO add impliedVolatility
    public required Guid      Id         { get; set; }
    public required Guid      SecurityId { get; set; }
    public          Security? Security   { get; set; }
    public required decimal   Price      { get; set; } //TODO Price should not exist
    public required decimal   HighPrice  { get; set; }
    public required decimal   LowPrice   { get; set; }
    public required int       Volume     { get; set; }
    public required DateTime  CreatedAt  { get; set; }
    public required DateTime  ModifiedAt { get; set; }
}
