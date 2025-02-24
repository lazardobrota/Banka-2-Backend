using Bank.Application.Domain;

namespace DefaultNamespace;

public class ClientFilterQuery
{
    public string?  Email     { set; get; }
    public string?  FirstName { set; get; }
    public string?  LastName  { set; get; }
    public Role     Role      { set; get; } = Role.Invalid;
    public Pageable Pagable   { set; get; } = new Pageable();
}
