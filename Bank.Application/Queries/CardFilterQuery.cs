using Bank.Application.Domain;

namespace Bank.Application.Queries;

public class CardFilterQuery
{
    public string? Number       { get; set; }
    public string? Name         { get; set; }
    public Guid    TypeId       { get; set; }
    public Guid    AccountId    { get; set; }
    public bool?   Status       { get; set; }
    public string? CardTypeName { get; set; }
}
