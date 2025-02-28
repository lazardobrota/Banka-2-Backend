using Bank.Application.Domain;
using Bank.Application.Requests;
using Bank.Application.Responses;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Bank.UserService.Swagger.SchemeFilters;

file static class Example
{
    public static class Employee
    {
        public static readonly Guid     Id                         = Guid.Parse("ae45452a-81fa-413b-9a3f-4e044ff13939");
        public const           string   Email                      = "nikola.jovanovic@example.com";
        public const           string   FirstName                  = "Nikola";
        public const           string   LastName                   = "Jovanović";
        public static readonly DateOnly DateOfBirth                = new(2005, 5, 17);
        public const           Gender   Gender                     = Bank.Application.Domain.Gender.Male;
        public const           string   UniqueIdentificationNumber = "1705005710032";
        public const           string   Username                   = "nikolaj";
        public const           string   PhoneNumber                = "+381632318592";
        public const           string   Address                    = "Kneza Miloša 88";
        public const           Role     Role                       = Bank.Application.Domain.Role.Employee;
        public const           string   Department                 = "HR";
        public static readonly DateTime CreatedAt                  = new(2025, 2, 26, 18, 0, 0);
        public static readonly DateTime ModifiedAt                 = new(2025, 2, 28, 19, 17, 10);
        public const           bool     Activated                  = true;
        public const           bool     Employed                   = true;

        public static readonly EmployeeCreateRequest CreateRequest = new()
                                                                     {
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
                                                                         Employed                   = Employed
                                                                     };

        public static readonly EmployeeUpdateRequest UpdateRequest = new()
                                                                     {
                                                                         FirstName   = FirstName,
                                                                         LastName    = LastName,
                                                                         Username    = Username,
                                                                         PhoneNumber = PhoneNumber,
                                                                         Address     = Address,
                                                                         Role        = Role,
                                                                         Department  = Department,
                                                                         Employed    = Employed,
                                                                         Activated   = false
                                                                     };

        public static readonly EmployeeResponse Response = new()
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
                                                               Employed                   = Employed,
                                                               Activated                  = Activated
                                                           };
    }
}

public static partial class SwaggerSchemaFilter
{
    public static class Employee
    {
        public class CreateRequest() : SwaggerSchemaFilter<EmployeeCreateRequest>(SchemeFilters.Example.Employee.CreateRequest)
        {
            protected override IOpenApiAny CreateExample(OpenApiSchema schema, SchemaFilterContext context)
            {
                return new OpenApiObject()
                       {
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
                           [nameof(Example.Employed)]                   = new OpenApiBoolean(Example.Employed)
                       };
            }
        }

        public class UpdateRequest() : SwaggerSchemaFilter<EmployeeUpdateRequest>(SchemeFilters.Example.Employee.UpdateRequest)
        {
            protected override IOpenApiAny CreateExample(OpenApiSchema schema, SchemaFilterContext context)
            {
                return new OpenApiObject()
                       {
                           [nameof(Example.FirstName)]   = new OpenApiString(Example.FirstName),
                           [nameof(Example.LastName)]    = new OpenApiString(Example.LastName),
                           [nameof(Example.Username)]    = new OpenApiString(Example.Username),
                           [nameof(Example.PhoneNumber)] = new OpenApiString(Example.PhoneNumber),
                           [nameof(Example.Address)]     = new OpenApiString(Example.Address),
                           [nameof(Example.Role)]        = new OpenApiInteger((int)Example.Role),
                           [nameof(Example.Department)]  = new OpenApiString(Example.Department),
                           [nameof(Example.Employed)]    = new OpenApiBoolean(Example.Employed),
                           [nameof(Example.Activated)]   = new OpenApiBoolean(false)
                       };
            }
        }

        public class Response() : SwaggerSchemaFilter<EmployeeResponse>(SchemeFilters.Example.Employee.Response)
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
                           [nameof(Example.Employed)]                   = new OpenApiBoolean(Example.Employed),
                           [nameof(Example.Activated)]                  = new OpenApiBoolean(Example.Activated)
                       };
            }
        }
    }
}
