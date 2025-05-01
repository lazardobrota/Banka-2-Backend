using Bank.Link.Configurations;
using Bank.Link.Core;
using Bank.Link.Core.B3;

using Microsoft.Extensions.DependencyInjection;

namespace Bank.Link;

public static class LinkExtensions
{
    public static IServiceCollection AddLinkServices(this IServiceCollection services)
    {
        services.AddSingleton<IBankUserData, BankUserData>();
        services.AddSingleton<IBankExchangeData, BankExchangeData>();

        return services;
    }

    public static IServiceCollection AddB3Link(this IServiceCollection services, BankData data)
    {
        return services.AddSingleton<IBankUserDataLink>(serviceProvider => new B3UserDataLink(data, serviceProvider.GetRequiredService<IHttpClientFactory>()))
                       .AddSingleton<IBankExchangeDataLink>(serviceProvider => new B3ExchangeDataLink(data, serviceProvider.GetRequiredService<IHttpClientFactory>()))
                       .CreateHttpClient(data);
    }

    internal static IServiceCollection CreateHttpClient(this IServiceCollection services, BankData data)
    {
        services.AddHttpClient(data.Code, httpClient =>
                                          {
                                              httpClient.BaseAddress = new Uri(data.BaseUrl);
                                              httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Configuration.Jwt.B3Token}");
                                          });

        return services;
    }
}
