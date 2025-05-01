using System.Net.Http.Json;

using Bank.Application.Domain;
using Bank.Application.Responses;
using Bank.Link.Endpoints;
using Bank.Link.Mapper.B3;
using Bank.Link.Mapper.B3.Query;
using Bank.Link.Queries;
using Bank.Link.Responses;

namespace Bank.Link.Core.B3;

internal class B3UserDataLink(BankData bankData, IHttpClientFactory httpClientFactory) : IBankUserDataLink
{
    private readonly IHttpClientFactory m_HttpClientFactory = httpClientFactory;

    public BankData BankData { get; } = bankData;

    public async Task<AccountResponse?> GetAccount(string accountNumber)
    {
        var httpClient = m_HttpClientFactory.CreateClient(BankData.Code);

        var filter   = new Query.B3.AccountFilter(accountNumber: accountNumber);
        var pageable = Pageable.Create(0, 1);

        var domain = Endpoint.B3.Account.GetAll;

        var query = filter.ToQuery()
                          .Map(pageable);

        var response = await httpClient.GetAsync($"{domain}?{query}");

        if (!response.IsSuccessStatusCode)
            return null;

        var responseList = await response.Content.ReadFromJsonAsync<List<Response.B3.AccountResponse>>();

        return responseList?.FirstOrDefault()
                           ?.ToNative();
    }
}
