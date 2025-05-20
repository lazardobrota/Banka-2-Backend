using System.Net.Http.Json;

using Bank.Application.Domain;
using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.Link.Endpoints;
using Bank.Link.Mapper.B3.Content;
using Bank.Link.Mapper.B3.Query;
using Bank.Link.Queries;
using Bank.Link.Responses;
using Bank.Link.Service;

namespace Bank.Link.Core.B3;

internal class B3UserDataLink(BankData bankData, IHttpClientFactory httpClientFactory, IDataService dataService) : IExternalUserDataLink
{
    private readonly IHttpClientFactory m_HttpClientFactory = httpClientFactory;
    private readonly IDataService       m_DataService       = dataService;

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

        if (responseList is null || responseList.Count == 0)
            return null;

        var accountResponse  = responseList.First();
        var currencyResponse = m_DataService.GetCurrencyByCode(accountResponse.CurrencyCode);

        return currencyResponse is null ? null : accountResponse.ToNative(currencyResponse);
    }

    public async Task<object?> CreateTransaction(TransactionCreateRequest createRequest)
    {
        var httpClient = m_HttpClientFactory.CreateClient(BankData.Code);

        //TODO: Implementation Missing

        return null;
    }

    public async Task<bool> NotifyTransactionStatus(TransactionNotifyStatusRequest notifyStatusRequest)
    {
        var httpClient = m_HttpClientFactory.CreateClient(BankData.Code);

        //TODO: Implementation Missing

        return false;
    }
}
