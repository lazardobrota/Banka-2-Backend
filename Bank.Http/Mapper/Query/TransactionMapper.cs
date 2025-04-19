using System.Collections.Specialized;

using Bank.Application.Extensions;
using Bank.Application.Queries;

namespace Bank.Http.Mapper.Query;

public static class TransactionMapper
{
    public static NameValueCollection ToQuery(this TransactionFilterQuery query)
    {
        return MapQuery(query, new NameValueCollection());
    }

    public static NameValueCollection Map(this NameValueCollection collection, TransactionFilterQuery query)
    {
        return MapQuery(query, collection);
    }

    public static NameValueCollection MapQuery(this TransactionFilterQuery query, NameValueCollection collection)
    {
        collection.Add(nameof(query.Status)
                       .ToCamelCase(), query.Status.ToString());

        collection.Add(nameof(query.FromDate)
                       .ToCamelCase(), query.FromDate.ToString());

        collection.Add(nameof(query.ToDate)
                       .ToCamelCase(), query.ToDate.ToString());

        return collection;
    }
}
