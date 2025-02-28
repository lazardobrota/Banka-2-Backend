using Bank.Application.Domain;
using Bank.Application.Requests;
using Bank.Application.Responses;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Bank.UserService.Swagger.SchemeFilters;

file static class Example
{
    public static class User
    {
        public static readonly Guid     Id                         = Guid.Parse("73b8f8ee-ff51-4247-b65b-52b8b9a494e5");
        public const           string   Email                      = "marko.petrovic@example.com";
        public const           string   FirstName                  = "Marko";
        public const           string   LastName                   = "Petrović";
        public const           string   Password                   = "M4rk0Petrovic@2024";
        public static readonly DateOnly DateOfBirth                = new(1995, 7, 21);
        public const           Gender   Gender                     = Bank.Application.Domain.Gender.Male;
        public const           string   UniqueIdentificationNumber = "2107953710020";
        public const           string   Username                   = "markop";
        public const           string   PhoneNumber                = "+381641234567";
        public const           string   Address                    = "Kraljice Natalije 45";
        public const           Role     Role                       = Bank.Application.Domain.Role.Admin;
        public const           string   Department                 = "IT department";
        public static readonly DateTime CreatedAt                  = new(2024, 10, 15, 9, 30, 0);
        public static readonly DateTime ModifiedAt                 = new(2025, 2, 28, 12, 45, 0);
        public const           bool     Activated                  = true;

        public static readonly UserLoginRequest LoginRequest = new()
                                                               {
                                                                   Email    = Email,
                                                                   Password = Password
                                                               };

        public static readonly UserActivationRequest ActivationRequest = new()
                                                                         {
                                                                             Password        = Password,
                                                                             ConfirmPassword = Password
                                                                         };

        public static readonly UserRequestPasswordResetRequest RequestPasswordResetRequest = new()
                                                                                             {
                                                                                                 Email = Email
                                                                                             };

        public static readonly UserPasswordResetRequest PasswordResetRequest = new()
                                                                               {
                                                                                   Password        = Password,
                                                                                   ConfirmPassword = Password
                                                                               };

        public static readonly UserResponse Response = new()
                                                       {
                                                           Id                         = Id,
                                                           FirstName                  = FirstName,
                                                           LastName                   = LastName,
                                                           DateOfBirth                = DateOfBirth,
                                                           Gender                     = Gender,
                                                           UniqueIdentificationNumber = UniqueIdentificationNumber,
                                                           Username                   = Username,
                                                           Email                      = Email,
                                                           PhoneNumber                = PhoneNumber,
                                                           Address                    = Address,
                                                           Role                       = Role,
                                                           Department                 = Department,
                                                           Accounts                   = [],
                                                           CreatedAt                  = CreatedAt,
                                                           ModifiedAt                 = ModifiedAt,
                                                           Activated                  = Activated
                                                       };

        public static readonly UserSimpleResponse SimpleResponse = new()
                                                                   {
                                                                       Id                         = Id,
                                                                       FirstName                  = FirstName,
                                                                       LastName                   = LastName,
                                                                       DateOfBirth                = DateOfBirth,
                                                                       Gender                     = Gender,
                                                                       UniqueIdentificationNumber = UniqueIdentificationNumber,
                                                                       Username                   = Username,
                                                                       Email                      = Email,
                                                                       PhoneNumber                = PhoneNumber,
                                                                       Address                    = Address,
                                                                       Role                       = Role,
                                                                       Department                 = Department,
                                                                       CreatedAt                  = CreatedAt,
                                                                       ModifiedAt                 = ModifiedAt,
                                                                       Activated                  = Activated
                                                                   };

        public static readonly UserLoginResponse LoginResponse = new()
                                                                 {
                                                                     Token =
                                                                     "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE3NDA3ODIyNjAsImlkIjoiNjFlMTY1OTMtM2EyNC00ZDVkLTg3MmMtMTdlMjJhMzQxZDMzIiwicm9sZSI6IkFkbWluIiwiaWF0IjoxNzQwNzgwNDYwLCJuYmYiOjE3NDA3ODA0NjB9.3DsroWriDMpHvuBNOSAiFq8gxdo4TEkc9WK1r2f0Ou0",
                                                                     User = null!
                                                                 };
    }
}

public static partial class SwaggerSchemaFilter
{
    public static class User
    {
        public class LoginRequest() : SwaggerSchemaFilter<UserLoginRequest>(SchemeFilters.Example.User.LoginRequest)
        {
            protected override IOpenApiAny CreateExample(OpenApiSchema schema, SchemaFilterContext context)
            {
                return new OpenApiObject()
                       {
                           [nameof(Example.Email)]    = new OpenApiString(Example.Email),
                           [nameof(Example.Password)] = new OpenApiString(Example.Password)
                       };
            }
        }

        public class ActivationRequest() : SwaggerSchemaFilter<UserActivationRequest>(SchemeFilters.Example.User.ActivationRequest)
        {
            protected override IOpenApiAny CreateExample(OpenApiSchema schema, SchemaFilterContext context)
            {
                return new OpenApiObject()
                       {
                           [nameof(Example.Password)]        = new OpenApiString(Example.Password),
                           [nameof(Example.ConfirmPassword)] = new OpenApiString(Example.ConfirmPassword)
                       };
            }
        }

        public class RequestPasswordResetRequest() : SwaggerSchemaFilter<UserRequestPasswordResetRequest>(SchemeFilters.Example.User.RequestPasswordResetRequest)
        {
            protected override IOpenApiAny CreateExample(OpenApiSchema schema, SchemaFilterContext context)
            {
                return new OpenApiObject()
                       {
                           [nameof(Example.Email)] = new OpenApiString(Example.Email),
                       };
            }
        }

        public class PasswordResetRequest() : SwaggerSchemaFilter<UserPasswordResetRequest>(SchemeFilters.Example.User.PasswordResetRequest)
        {
            protected override IOpenApiAny CreateExample(OpenApiSchema schema, SchemaFilterContext context)
            {
                return new OpenApiObject()
                       {
                           [nameof(Example.Password)]        = new OpenApiString(Example.Password),
                           [nameof(Example.ConfirmPassword)] = new OpenApiString(Example.ConfirmPassword)
                       };
            }
        }

        public class Response() : SwaggerSchemaFilter<UserResponse>(SchemeFilters.Example.User.Response)
        {
            protected override IOpenApiAny CreateExample(OpenApiSchema schema, SchemaFilterContext context)
            {
                var accounts = new OpenApiArray { context.SchemaRepository.Schemas[nameof(AccountResponse)].Example };

                return new OpenApiObject()
                       {
                           [nameof(Example.Id)]                         = new OpenApiString(Example.Id.ToString()),
                           [nameof(Example.FirstName)]                  = new OpenApiString(Example.FirstName),
                           [nameof(Example.LastName)]                   = new OpenApiString(Example.LastName),
                           [nameof(Example.DateOfBirth)]                = new OpenApiDate(new DateTime(Example.DateOfBirth, TimeOnly.MinValue)),
                           [nameof(Example.Gender)]                     = new OpenApiInteger((int)Example.Gender),
                           [nameof(Example.UniqueIdentificationNumber)] = new OpenApiString(Example.UniqueIdentificationNumber),
                           [nameof(Example.Username)]                   = new OpenApiString(Example.Username),
                           [nameof(Example.Email)]                      = new OpenApiString(Example.Email),
                           [nameof(Example.PhoneNumber)]                = new OpenApiString(Example.PhoneNumber),
                           [nameof(Example.Address)]                    = new OpenApiString(Example.Address),
                           [nameof(Example.Role)]                       = new OpenApiInteger((int)Example.Role),
                           [nameof(Example.Department)]                 = new OpenApiString(Example.Department),
                           [nameof(Example.Accounts)]                   = accounts,
                           [nameof(Example.CreatedAt)]                  = new OpenApiDateTime(Example.CreatedAt),
                           [nameof(Example.ModifiedAt)]                 = new OpenApiDateTime(Example.ModifiedAt),
                           [nameof(Example.Activated)]                  = new OpenApiBoolean(Example.Activated)
                       };
            }
        }

        public class SimpleResponse() : SwaggerSchemaFilter<UserSimpleResponse>(SchemeFilters.Example.User.SimpleResponse)
        {
            protected override IOpenApiAny CreateExample(OpenApiSchema schema, SchemaFilterContext context)
            {
                return new OpenApiObject()
                       {
                           [nameof(Example.Id)]                         = new OpenApiString(Example.Id.ToString()),
                           [nameof(Example.FirstName)]                  = new OpenApiString(Example.FirstName),
                           [nameof(Example.LastName)]                   = new OpenApiString(Example.LastName),
                           [nameof(Example.DateOfBirth)]                = new OpenApiDate(new DateTime(Example.DateOfBirth, TimeOnly.MinValue)),
                           [nameof(Example.Gender)]                     = new OpenApiInteger((int)Example.Gender),
                           [nameof(Example.UniqueIdentificationNumber)] = new OpenApiString(Example.UniqueIdentificationNumber),
                           [nameof(Example.Username)]                   = new OpenApiString(Example.Username),
                           [nameof(Example.Email)]                      = new OpenApiString(Example.Email),
                           [nameof(Example.PhoneNumber)]                = new OpenApiString(Example.PhoneNumber),
                           [nameof(Example.Address)]                    = new OpenApiString(Example.Address),
                           [nameof(Example.Role)]                       = new OpenApiInteger((int)Example.Role),
                           [nameof(Example.Department)]                 = new OpenApiString(Example.Department),
                           [nameof(Example.CreatedAt)]                  = new OpenApiDateTime(Example.CreatedAt),
                           [nameof(Example.ModifiedAt)]                 = new OpenApiDateTime(Example.ModifiedAt),
                           [nameof(Example.Activated)]                  = new OpenApiBoolean(Example.Activated)
                       };
            }
        }

        public class LoginResponse() : SwaggerSchemaFilter<UserLoginResponse>(SchemeFilters.Example.User.LoginResponse)
        {
            protected override IOpenApiAny CreateExample(OpenApiSchema schema, SchemaFilterContext context)
            {
                var user = context.SchemaRepository.Schemas[nameof(UserResponse)].Example;

                return new OpenApiObject()
                       {
                           [nameof(Example.Token)] = new OpenApiString(Example.Token),
                           [nameof(Example.User)]  = user
                       };
            }
        }
    }
}
