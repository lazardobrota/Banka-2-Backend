using System.Collections.Specialized;

using Bank.Application.Extensions;
using Bank.Application.Queries;

namespace Bank.Http.Mapper.Query;

public static class UserMapper
{
    public static NameValueCollection ToQuery(this UserFilterQuery query)
    {
        return MapQuery(query, new NameValueCollection());
    }

    public static NameValueCollection Map(this NameValueCollection collection, UserFilterQuery query)
    {
        return MapQuery(query, collection);
    }

    public static NameValueCollection MapQuery(this UserFilterQuery query, NameValueCollection collection)
    {
        if (query.Email is not null)
            collection.Add(nameof(query.Email)
                           .ToCamelCase(), query.Email);

        if (query.FirstName is not null)
            collection.Add(nameof(query.FirstName)
                           .ToCamelCase(), query.FirstName);

        if (query.LastName is not null)
            collection.Add(nameof(query.LastName)
                           .ToCamelCase(), query.LastName);

        collection.Add(nameof(query.Role)
                       .ToCamelCase(), query.Role.ToString());

        if (query.Ids.Count > 0)
            collection.Add(nameof(query.Ids)
                           .ToCamelCase(), string.Join(",", query.Ids));

        return collection;
    }
}
