using Bank.Application.Requests;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Bank.UserService.Swagger.SchemeFilters;

file static class Example
{
    public static class User
    {
        public const string Email    = "example@mail.com";
        public const string Password = "ExamplePassword01";
        
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
    }
}
