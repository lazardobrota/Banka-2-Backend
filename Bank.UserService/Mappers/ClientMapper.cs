using Bank.Application.Responses;
using Bank.UserService.Models;

namespace Bank.UserService.Mappers;

public static class ClientMapper
{
    public static ClientResponse ToResponse(this Client client)
    {
        return new ClientResponse
               {
                   Id                         = client.Id,
                   FirstName                  = client.FirstName,
                   LastName                   = client.LastName,
                   DateOfBirth                = client.DateOfBirth,
                   Gender                     = client.Gender,
                   UniqueIdentificationNumber = client.UniqueIdentificationNumber,
                   Email                      = client.Email,
                   PhoneNumber                = client.PhoneNumber,
                   Address                    = client.Address,
                   Role                       = client.Role,
                   CreatedAt                  = client.CreatedAt,
                   ModifiedAt                 = client.ModifiedAt,
                   Activated                  = client.Activated
               };
    }
}
