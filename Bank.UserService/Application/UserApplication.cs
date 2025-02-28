using System.IdentityModel.Tokens.Jwt;
using System.Text;

using Bank.Application;
using Bank.Application.Domain;
using Bank.UserService.Configurations;
using Bank.UserService.Database;
using Bank.UserService.HostedServices;
using Bank.UserService.Repositories;
using Bank.UserService.Security;
using Bank.UserService.Services;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Bank.UserService.Application;

public class UserApplication
{
    public static void Run(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        builder.Services.AddApplicationAuthentication();
        builder.Services.AddApplicationAuthorization();

        builder.Services.AddServiceApplication();
        builder.Services.AddApplicationCors();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        // builder.Services.AddSwaggerGen();
        builder.Services.AddApplicationSwagger();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(Configuration.Policy.FrontendApplication);

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

    public static IServiceCollection AddApplicationSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(config =>
                               {
                                   config.SwaggerDoc("v1", new OpenApiInfo() { Title = "UserService", Version = "v1" });

                                   config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                                                                     {
                                                                         Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                                                                         Name   = "Authorization",
                                                                         In     = ParameterLocation.Header,
                                                                         Type   = SecuritySchemeType.ApiKey,
                                                                         Scheme = "Bearer"
                                                                     });

                                   config.AddSecurityRequirement(new OpenApiSecurityRequirement
                                                            {
                                                                {
                                                                    new OpenApiSecurityScheme
                                                                    {
                                                                        Reference = new OpenApiReference
                                                                                    {
                                                                                        Type =
                                                                                        ReferenceType
                                                                                        .SecurityScheme,
                                                                                        Id = "Bearer"
                                                                                    }
                                                                    },
                                                                    []
                                                                }
                                                            });
                               });

        return services;
    }

    public static IServiceCollection AddApplicationCors(this IServiceCollection services)
    {
        services.AddCors(options => options.AddPolicy(Configuration.Policy.FrontendApplication, policy => policy.WithOrigins(Configuration.Frontend.BaseUrl)
                                                                                                                .AllowAnyHeader()
                                                                                                                .AllowAnyMethod()));

        return services;
    }

    public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services)
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

        return services;
    }

    public static IServiceCollection AddApplicationAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
                .AddPolicy(Configuration.Policy.Role.Admin,    policy => policy.RequireRole(nameof(Role.Admin)))
                .AddPolicy(Configuration.Policy.Role.Employee, policy => policy.RequireRole(nameof(Role.Employee)))
                .AddPolicy(Configuration.Policy.Role.Client,   policy => policy.RequireRole(nameof(Role.Client)));

        return services;
    }
}
