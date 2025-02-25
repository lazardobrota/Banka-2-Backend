using Bank.Application.Responses;
using Bank.UserService.Models;

namespace Bank.UserService.Mappers;

public static class UserMapper
{
    public static UserResponse ToResponse(this User user)
    {
        return new UserResponse
               {
                   Id                         = user.Id,
                   FirstName                  = user.FirstName,
                   LastName                   = user.LastName,
                   DateOfBirth                = user.DateOfBirth,
                   Gender                     = user.Gender,
                   UniqueIdentificationNumber = user.UniqueIdentificationNumber,
                   Email                      = user.Email,
                   Username                   = user.Username,
                   PhoneNumber                = user.PhoneNumber,
                   Address                    = user.Address,
                   Role                       = user.Role,
                   Department                 = user.Department,
                   Accounts                   = MapAccounts(user.Accounts),
                   CreatedAt                  = user.CreatedAt,
                   ModifiedAt                 = user.ModifiedAt,
                   Activated                  = user.Activated
               };
    }

    private static List<AccountSimpleResponse> MapAccounts(List<Account> accounts) =>
    accounts.Select(account => account.ToSimpleResponse())
            .ToList();

    public static UserSimpleResponse ToSimpleResponse(this User user)
    {
        return new UserSimpleResponse
               {
                   Id                         = user.Id,
                   FirstName                  = user.FirstName,
                   LastName                   = user.LastName,
                   DateOfBirth                = user.DateOfBirth,
                   Gender                     = user.Gender,
                   UniqueIdentificationNumber = user.UniqueIdentificationNumber,
                   Email                      = user.Email,
                   Username                   = user.Username,
                   PhoneNumber                = user.PhoneNumber,
                   Address                    = user.Address,
                   Role                       = user.Role,
                   Department                 = user.Department,
                   CreatedAt                  = user.CreatedAt,
                   ModifiedAt                 = user.ModifiedAt,
                   Activated                  = user.Activated
               };
    }

    public static Employee ToEmployee(this User user)
    {
        return new Employee
               {
                   Id                         = user.Id,
                   FirstName                  = user.FirstName,
                   LastName                   = user.LastName,
                   DateOfBirth                = user.DateOfBirth,
                   Gender                     = user.Gender,
                   UniqueIdentificationNumber = user.UniqueIdentificationNumber,
                   Email                      = user.Email,
                   Username                   = user.Username,
                   PhoneNumber                = user.PhoneNumber,
                   Address                    = user.Address,
                   Password                   = user.Password,
                   Salt                       = user.Salt,
                   Role                       = user.Role,
                   Department                 = user.Department!,
                   CreatedAt                  = user.CreatedAt,
                   ModifiedAt                 = user.ModifiedAt,
                   Employed                   = user.Employed ?? true,
                   Activated                  = user.Activated
               };
    }

    public static User ToUser(this Employee employee)
    {
        return new User
               {
                   Id                         = employee.Id,
                   FirstName                  = employee.FirstName,
                   LastName                   = employee.LastName,
                   DateOfBirth                = employee.DateOfBirth,
                   Gender                     = employee.Gender,
                   UniqueIdentificationNumber = employee.UniqueIdentificationNumber,
                   Email                      = employee.Email,
                   Username                   = employee.Username,
                   PhoneNumber                = employee.PhoneNumber,
                   Address                    = employee.Address,
                   Password                   = employee.Password,
                   Salt                       = employee.Salt,
                   Role                       = employee.Role,
                   Department                 = employee.Department,
                   CreatedAt                  = employee.CreatedAt,
                   ModifiedAt                 = employee.ModifiedAt,
                   Employed                   = employee.Employed,
                   Activated                  = employee.Activated
               };
    }
}
