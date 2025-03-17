using Bank.Application.Domain;
using Bank.Application.Requests;

namespace Bank.UserService.Test.Examples.Entities;

public static partial class Example
{
    public static partial class Entity
    {
        public static class Employee
        {
            public static readonly EmployeeCreateRequest CreateRequest = new()
                                                                         {
                                                                             FirstName                  = "Nikola",
                                                                             LastName                   = "Jovanović",
                                                                             DateOfBirth                = new(2005, 5, 17),
                                                                             Gender                     = Gender.Male,
                                                                             UniqueIdentificationNumber = "1705005710032",
                                                                             Username                   = "nikolaj",
                                                                             Email                      = "nikola.jovanovic@example.com",
                                                                             PhoneNumber                = "+381632318592",
                                                                             Address                    = "Kneza Miloša 88",
                                                                             Role                       = Role.Employee,
                                                                             Department                 = "HR",
                                                                             Employed                   = true
                                                                         };
        }
    }
}
