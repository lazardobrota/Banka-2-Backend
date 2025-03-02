namespace Bank.UserService.Models;

public class Card
{
    public required Guid Id { get; set; }
    public required string Number { get; set; }
    public required CardType Type { get; set; }
    public required string Name { get; set; }
    public required DateOnly ExpiresAt { get; set; }
    public required Account Account { get; set; }
    public required string CVV { get; set; }
    public required decimal Limit { get; set; }
    public required bool Status { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime ModifiedAt { get; set; }
}
