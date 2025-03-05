using Bank.Application.Extensions;
using Bank.Application.Responses;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Bank.UserService.Swagger.SchemeFilters;

file static class Example
{
    public static class Card
    {
        public static readonly Guid     Id         = Guid.Parse("987e6543-e21b-45d3-a123-654321abcdef");
        public const           string   Number     = "1234-5678-9876-5437";
        public const           string   Name       = "Credit Card";
        public static readonly DateOnly ExpiresAt  = new(2028, 5, 31);
        public const           string   Cvv        = "123";
        public const           decimal  Limit      = 5000.00m;
        public const           bool     Status     = true;
        public static readonly DateTime CreatedAt  = new(2024, 5, 2, 14, 12, 23);
        public static readonly DateTime ModifiedAt = new(2025, 1, 3, 11, 45, 0);

        public static readonly CardResponse Response = new()
                                                       {
                                                           Id         = Id,
                                                           Number     = Number,
                                                           Type       = null!,
                                                           Name       = Name,
                                                           ExpiresAt  = ExpiresAt,
                                                           Account    = null!,
                                                           CVV        = Cvv,
                                                           Limit      = Limit,
                                                           Status     = Status,
                                                           CreatedAt  = CreatedAt,
                                                           ModifiedAt = ModifiedAt
                                                       };
    }
}

public static partial class SwaggerSchemaFilter
{
    public static class Card
    {
        public class Response() : SwaggerSchemaFilter<CardResponse>(SchemeFilters.Example.Card.Response)
        {
            protected override IOpenApiAny CreateExample(OpenApiSchema schema, SchemaFilterContext context)
            {
                var account  = context.SchemaRepository.Schemas[nameof(AccountResponse)].Example;
                var cardType = context.SchemaRepository.Schemas[nameof(CardTypeResponse)].Example;

                return new OpenApiObject()
                       {
                           [nameof(Example.Id)
                            .ToCamelCase()] = new OpenApiString(Example.Id.ToString()),
                           [nameof(Example.Number)
                            .ToCamelCase()] = new OpenApiString(Example.Number),
                           [nameof(Example.Type)
                            .ToCamelCase()] = cardType,
                           [nameof(Example.Name)
                            .ToCamelCase()] = new OpenApiString(Example.Name),
                           [nameof(Example.ExpiresAt)
                            .ToCamelCase()] = new OpenApiDate(new DateTime(Example.ExpiresAt, TimeOnly.MinValue)),
                           [nameof(Example.Account)
                            .ToCamelCase()] = account,
                           [nameof(Example.CVV)
                            .ToCamelCase()] = new OpenApiString(Example.CVV),
                           [nameof(Example.Limit)
                            .ToCamelCase()] = new OpenApiDouble((double)Example.Limit),
                           [nameof(Example.Status)
                            .ToCamelCase()] = new OpenApiBoolean(Example.Status),
                           [nameof(Example.CreatedAt)
                            .ToCamelCase()] = new OpenApiDateTime(Example.CreatedAt),
                           [nameof(Example.ModifiedAt)
                            .ToCamelCase()] = new OpenApiDateTime(Example.ModifiedAt)
                       };
            }
        }
    }
}
