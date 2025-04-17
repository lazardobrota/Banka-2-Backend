using System.Collections.Specialized;

using Bank.Application.Extensions;
using Bank.Application.Queries;

namespace Bank.Http.Mapper.Query;

public static class ExchangeMapper
{
    public static NameValueCollection ToQuery(this ExchangeFilterQuery query)
    {
        return MapQuery(query, new NameValueCollection());
    }

    public static NameValueCollection Map(this NameValueCollection collection, ExchangeFilterQuery query)
    {
        return MapQuery(query, collection);
    }

    public static NameValueCollection MapQuery(this ExchangeFilterQuery query, NameValueCollection collection)
    {
        if (query.CurrencyId != Guid.Empty)
            collection.Add(nameof(query.CurrencyId)
                           .ToCamelCase(), query.CurrencyId.ToString());

        if (query.CurrencyCode is not null)
            collection.Add(nameof(query.CurrencyCode)
                           .ToCamelCase(), query.CurrencyCode);

        if (query.Date != default)
            collection.Add(nameof(query.Date)
                           .ToCamelCase(), query.Date.ToString());

        return collection;
    }

    public static NameValueCollection ToQuery(this ExchangeBetweenQuery query)
    {
        return MapQuery(query, new NameValueCollection());
    }

    public static NameValueCollection Map(this NameValueCollection collection, ExchangeBetweenQuery query)
    {
        return MapQuery(query, collection);
    }

    public static NameValueCollection MapQuery(this ExchangeBetweenQuery query, NameValueCollection collection)
    {
        collection.Add(nameof(query.CurrencyFromCode)
                       .ToCamelCase(), query.CurrencyFromCode);

        collection.Add(nameof(query.CurrencyToCode)
                       .ToCamelCase(), query.CurrencyToCode);

        return collection;
    }
}
