namespace Bank.Application.Domain;

public enum Role
{
    Invalid,
    Admin,
    Employee,
    Client,
}

public enum Gender
{
    Invalid,
    Male,
    Female
}

public enum TransactionStatus
{
    Invalid,
    Pending,
    Canceled,
    Completed,
    Failed
}

public enum LoanStatus
{
    Pending,
    Active,
    Rejected,
    Closed,
    DefaultWarning,
    Default
}

public enum InterestType
{
    Fixed,
    Variable,
    Mixed
}

public enum InstallmentStatus
{
    Pending,
    Paid,
    Overdue,
    Cancelled
}
