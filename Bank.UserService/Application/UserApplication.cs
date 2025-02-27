using System.IdentityModel.Tokens.Jwt;
using System.Text;

using Bank.Application;
using Bank.UserService.Database;
using Bank.UserService.HostedServices;
using Bank.UserService.Repositories;
using Bank.UserService.Security;
using Bank.UserService.Services;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Bank.UserService.Application;

public class UserApplication
{
    public static void Run(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        builder.Services.AddAuthenticationAndAuthorization();

        builder.Services.AddServiceApplication();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceApplication(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<AssemblyInfo>();

        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<IUserService, Services.UserService>();
        services.AddTransient<IClientService, ClientService>();
        services.AddTransient<IEmployeeService, EmployeeService>();
        services.AddTransient<IEmailRepository, EmailRepository>();
        services.AddTransient<IEmailService, EmailService>();

        services.AddSingleton<TokenProvider>();
        services.AddSingleton<DatabaseHostedService>();

        services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(DatabaseConfig.GetConnectionString()), ServiceLifetime.Scoped, ServiceLifetime.Singleton);

        services.AddHostedService<ApplicationHostedService>();

        return services;
    }

    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services)
    {
        var secretKey = Environment.GetEnvironmentVariable(EnvironmentVariable.SecretKey) ?? EnvironmentVariable.SecretKeyElseValue;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtOptions => jwtOptions.TokenValidationParameters = new TokenValidationParameters
                                                                                   {
                                                                                       IssuerSigningKey =
                                                                                       new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                                                                                       ValidateIssuerSigningKey = true,
                                                                                       ValidateLifetime         = true,
                                                                                       ValidateIssuer           = false,
                                                                                       ValidateAudience         = false,
                                                                                       ClockSkew                = TimeSpan.Zero
                                                                                   });

        services.AddAuthorization(ConfigureAuthorizationOptions);

        return services;
    }

    private static void ConfigureAuthorizationOptions(AuthorizationOptions jwtOptions) { }
}
