using Bank.Application.Domain;
using Bank.Application.Requests;

namespace Bank.UserService.Test.Examples.Entities;

public static partial class Example
{
    public static partial class Entity
    {
        public static class Installment
        {
            public static readonly InstallmentCreateRequest CreateRequest = new()
                                                                            {
                                                                                InstallmentId   = Guid.Parse("a52cbe51-d29e-486a-b7dd-079aa315883f"),
                                                                                LoanId          = Guid.Parse("f5a74113-8f10-42a3-b130-54c5c691ba8e"),
                                                                                InterestRate    = 5.0m,
                                                                                ExpectedDueDate = new(2025, 6, 15),
                                                                                ActualDueDate   = new(2025, 6, 20),
                                                                                Status          = InstallmentStatus.Pending
                                                                            };

            public static readonly InstallmentUpdateRequest UpdateRequest = new()
                                                                            {
                                                                                ActualDueDate = new(2025, 6, 15),
                                                                                Status        = InstallmentStatus.Paid
                                                                            };
        }
    }
}
