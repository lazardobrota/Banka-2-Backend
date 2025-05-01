using System.Net.Http.Json;

using Bank.Application.Responses;
using Bank.Link.Core;
using Bank.Link.Endpoints;

namespace Bank.Link.B3;

internal class B3UserDataLink(BankData bankData, IHttpClientFactory httpClientFactory) : IBankUserDataLink
{
    private readonly IHttpClientFactory m_HttpClientFactory = httpClientFactory;

    public BankData BankData { get; } = bankData;

    public async Task<AccountResponse?> GetAccount(string accountNumber)
    {
        var httpClient = m_HttpClientFactory.CreateClient(BankData.Code);

        var domain = Endpoint.B3.Account.GetAll;
        
        var response = await httpClient.GetAsync(domain);
        
        if (!response.IsSuccessStatusCode)
            return null;

        var responseList = await response.Content.ReadFromJsonAsync<List<AccountResponse>>();

        return responseList?.FirstOrDefault();
    }
}
