using Bank.Http.Clients.User;
using Bank.Http.Configurations;

using Microsoft.Extensions.DependencyInjection;

namespace Bank.Http;

public static class HttpExtensions
{
    public static IServiceCollection AddUserServiceHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient(Configuration.Client.Name.UserService, httpClient =>
                                                                      {
                                                                          httpClient.BaseAddress = new Uri($"{Configuration.Client.BaseUrl.UserService}");
                                                                          //TODO
                                                                          //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjEzMDU5MzY1NzI1NCwiaWQiOiJjNmY0NDEzMy0wOGYyLTRhNDMtYmQ2NS05Y2ZiNmIxM2ZhNWIiLCJwZXJtaXNzaW9uIjoiMiIsInJvbGUiOiJBZG1pbiIsImlhdCI6MTc0NDYzODQzNCwibmJmIjoxNzQ0NjM4NDM0fQ.1cA329l2bWUlENwYrq03l1yQ0Jxw597kw-YUT0WipiI");
                                                                      });

        services.AddSingleton<IUserServiceHttpClient, UserServiceHttpClient>();

        return services;
    }
}
