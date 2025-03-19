using Bank.Application.Domain;
using Bank.Application.Requests;

namespace Bank.UserService.Test.Examples.Entities;

public static partial class Example
{
    public static partial class Entity
    {
        public static class Client
        {
            public static readonly ClientCreateRequest CreateRequest = new()
                                                                       {
                                                                           FirstName                  = "Aleksandar",
                                                                           LastName                   = "Ivanović",
                                                                           DateOfBirth                = new(1995, 7, 12),
                                                                           Gender                     = Gender.Male,
                                                                           UniqueIdentificationNumber = "1207995710029",
                                                                           Email                      = "aleksandar.ivanovic@gmail.com",
                                                                           PhoneNumber                = "+381698812321",
                                                                           Address                    = "Kralja Petra 12",
                                                                       };

            public static readonly ClientUpdateRequest UpdateRequest = new()
                                                                       {
                                                                           FirstName   = "Aleksandar",
                                                                           LastName    = "Ivanović",
                                                                           PhoneNumber = "+381698812321",
                                                                           Address     = "Kralja Petra 12",
                                                                           Activated   = true
                                                                       };
        }
    }
}
