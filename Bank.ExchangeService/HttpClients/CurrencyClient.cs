using Bank.Application.Endpoints;
using Bank.Application.Responses;

namespace Bank.ExchangeService.HttpClients;

public interface ICurrencyClient
{
    Task<CurrencyResponse?> GetCurrencyById(Guid id);

    Task<CurrencySimpleResponse?> GetCurrencyByIdSimple(Guid id);

    Task<List<CurrencySimpleResponse>?> GetAllCurrenciesSimple();
}

public class CurrencyClient(HttpClient httpClient) : ICurrencyClient
{
    private readonly HttpClient m_HttpClient = httpClient;

    public async Task<CurrencyResponse?> GetCurrencyById(Guid id)
    {
        var response = await m_HttpClient.GetAsync($"{Endpoints.Currency.Base}/{id}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<CurrencyResponse>();
    }

    public async Task<CurrencySimpleResponse?> GetCurrencyByIdSimple(Guid id)
    {
        var response = await m_HttpClient.GetAsync($"{Endpoints.Currency.GetAllSimple}/{id}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<CurrencySimpleResponse>();
    }

    public async Task<List<CurrencySimpleResponse>?> GetAllCurrenciesSimple()
    {
        var response = await m_HttpClient.GetAsync(Endpoints.Currency.GetAllSimple);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<List<CurrencySimpleResponse>>();
    }
}
