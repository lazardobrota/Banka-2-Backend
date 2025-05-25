using Bank.Application.Domain;

using MessagePack;

namespace Bank.ExchangeService.Models;

public class Order
{
    public required Guid        Id            { set; get; }
    public required Guid        ActuaryId     { set; get; }
    public          Guid?       SupervisorId  { set; get; }
    public required Guid        AccountId     { set; get; }
    public required Guid        SecurityId    { set; get; }
    public          Security?   Security      { set; get; }
    public required OrderType   OrderType     { set; get; }
    public required int         Quantity      { set; get; }
    public required int         ContractCount { set; get; }
    public required decimal     LimitPrice    { set; get; }
    public required decimal     StopPrice     { set; get; }
    public required Direction   Direction     { set; get; }
    public required OrderStatus Status        { set; get; }
    public required DateTime    CreatedAt     { set; get; }
    public required DateTime    ModifiedAt    { set; get; }
}

[MessagePackObject]
public class RedisOrder
{
    [IgnoreMember]
    public Guid Id { set; get; }

    [Key(0)]
    public required string Ticker { set; get; }

    [IgnoreMember]
    public OrderType OrderType { set; get; }

    [Key(1)]
    public required Guid AccountId { get; set; }

    [Key(2)]
    public required int RemainingPortions { set; get; }

    [Key(3)]
    public required decimal LimitPrice { set; get; }

    [Key(4)]
    public required decimal StopPrice { set; get; }

    [Key(5)]
    public required Direction Direction { set; get; }
}

[MessagePackObject]
public class TmpQuote
{
    [IgnoreMember]
    public Guid Id { get; set; }

    [Key(0)]
    public required Guid SecurityId { get; set; }

    [IgnoreMember]
    public string Ticker { get; set; } = null!;

    [IgnoreMember]
    public Security? Security { get; set; }

    [Key(1)]
    public required decimal AskPrice { get; set; }

    [Key(2)]
    public required decimal BidPrice { get; set; }

    [Key(3)]
    public required decimal HighPrice { get; set; }

    [Key(4)]
    public required decimal LowPrice { get; set; }

    [Key(5)]
    public required decimal ClosePrice { get; set; }

    [Key(6)]
    public required decimal OpeningPrice { get; set; }

    [Key(7)]
    public decimal ImpliedVolatility { get; set; }

    [Key(8)]
    public required long Volume { get; set; }

    [IgnoreMember]
    public DateTime CreatedAt { get; set; }

    [IgnoreMember]
    public DateTime ModifiedAt { get; set; }
}

public class ProcessOrder
{
    public required Guid      Id           { set; get; }
    public required Guid      ActuaryId    { set; get; }
    public          Guid      SupervisorId { set; get; }
    public          Guid      AccountId    { set; get; }
    public required Guid      SecurityId   { set; get; }
    public required OrderType OrderType    { set; get; }
    public required Direction Direction    { set; get; }
}
