using Bank.Application.Domain;

namespace Bank.ExchangeService.Models;

public class Order
{
    public required Guid        Id                { set; get; }
    public required Guid        ActuaryId         { set; get; }
    public          Guid?       SupervisorId      { set; get; }
    public          Guid?       AccountId         { set; get; }
    public required Guid        SecurityId        { set; get; }
    public          Security?   Security          { set; get; }
    public required OrderType   OrderType         { set; get; }
    public required int         Quantity          { set; get; }
    public required int         ContractCount     { set; get; }
    public required decimal     PricePerUnit      { set; get; }
    public required Direction   Direction         { set; get; }
    public required OrderStatus Status            { set; get; }
    public required bool        Done              { set; get; }
    public required int         RemainingPortions { set; get; }
    public required bool        AfterHours        { set; get; }
    public required DateTime    CreatedAt         { set; get; }
    public required DateTime    ModifiedAt        { set; get; }
}
