using System.Collections.Specialized;

using Bank.Application.Extensions;
using Bank.Application.Queries;

namespace Bank.Http.Mapper.Query;

public static class AccountTypeMapper
{
    public static NameValueCollection ToQuery(this AccountTypeFilterQuery query)
    {
        return MapQuery(query, new NameValueCollection());
    }

    public static NameValueCollection Map(this NameValueCollection collection, AccountTypeFilterQuery query)
    {
        return MapQuery(query, collection);
    }

    public static NameValueCollection MapQuery(this AccountTypeFilterQuery query, NameValueCollection collection)
    {
        if (query.Name is not null)
            collection.Add(nameof(query.Name)
                           .ToCamelCase(), query.Name);

        if (query.Code is not null)
            collection.Add(nameof(query.Code)
                           .ToCamelCase(), query.Code);

        return collection;
    }
}
