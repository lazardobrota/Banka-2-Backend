using Bank.Application.Extensions;
using Bank.Application.Responses;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Bank.UserService.Swagger.SchemeFilters;

file static class Example
{
    public static class Account
    {
        public static readonly Guid   Id            = Guid.Parse("8cf284fa-d8f2-44f5-872a-cfa47a0f7199");
        public const           string AccountNumber = "222-0001123456789-10";

        public static readonly AccountResponse Response = new()
                                                          {
                                                              Id            = Id,
                                                              AccountNumber = AccountNumber,
                                                              User          = null!
                                                          };

        public static readonly AccountSimpleResponse SimpleResponse = new()
                                                                      {
                                                                          Id            = Id,
                                                                          AccountNumber = AccountNumber
                                                                      };
    }
}

public static partial class SwaggerSchemaFilter
{
    public static class Account
    {
        public class Response() : SwaggerSchemaFilter<AccountResponse>(SchemeFilters.Example.Account.Response)
        {
            protected override IOpenApiAny CreateExample(OpenApiSchema schema, SchemaFilterContext context)
            {
                var user = context.SchemaRepository.Schemas[nameof(UserSimpleResponse)].Example;

                return new OpenApiObject()
                       {
                           [nameof(Example.Id).ToCamelCase()]            = new OpenApiString(Example.Id.ToString()),
                           [nameof(Example.AccountNumber).ToCamelCase()] = new OpenApiString(Example.AccountNumber),
                           [nameof(Example.User).ToCamelCase()]          = user
                       };
            }
        }

        public class SimpleResponse() : SwaggerSchemaFilter<AccountSimpleResponse>(SchemeFilters.Example.Account.SimpleResponse)
        {
            protected override IOpenApiAny CreateExample(OpenApiSchema schema, SchemaFilterContext context)
            {
                return new OpenApiObject()
                       {
                           [nameof(Example.Id).ToCamelCase()]            = new OpenApiString(Example.Id.ToString()),
                           [nameof(Example.AccountNumber).ToCamelCase()] = new OpenApiString(Example.AccountNumber),
                       };
            }
        }
    }
}
