using Bank.Application;
using Bank.OpenApi;
using Bank.Permissions;
using Bank.UserService.BackgroundServices;
using Bank.UserService.Configurations;
using Bank.UserService.Database;
using Bank.UserService.HostedServices;
using Bank.UserService.Repositories;
using Bank.UserService.Services;

using DotNetEnv;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.EntityFrameworkCore;

using Example = Bank.UserService.Database.Examples.Example;

namespace Bank.UserService.Application;

public class UserApplication
{
    public static void Run(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Env.Load();

        builder.Services.AddValidation();
        builder.Services.AddServices();
        builder.Services.AddDatabase();
        builder.Services.AddHostedServices();
        builder.Services.AddBackgroundServices();
        builder.Services.AddHttpServices();

        builder.Services.AddCors();
        builder.Services.AddAuthenticationServices();
        builder.Services.AddAuthorizationServices();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApiServices();
        builder.Services.AddOpenApiExamples();

        var app = builder.Build();

        app.MapOpenApiServices();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        
        app.UseCors(Configuration.Policy.FrontendApplication);

        app.Run();
    }
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IBankRepository, BankRepository>();
        services.AddScoped<IBankService, BankService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IUserService, Services.UserService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IAccountCurrencyRepository, AccountCurrencyRepository>();
        services.AddScoped<IEmailRepository, EmailRepository>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<ICardTypeService, CardTypeService>();
        services.AddScoped<ICardService, CardService>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<ICardTypeRepository, CardTypeRepository>();
        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IAccountTypeRepository, AccountTypeRepository>();
        services.AddScoped<IAccountTypeService, AccountTypeService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAccountCurrencyService, AccountCurrencyService>();
        services.AddScoped<IExchangeRepository, ExchangeRepository>();
        services.AddScoped<IExchangeService, ExchangeService>();
        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<ILoanTypeRepository, LoanTypeRepository>();
        services.AddScoped<IInstallmentRepository, InstallmentRepository>();
        services.AddScoped<ITransactionCodeRepository, TransactionCodeRepository>();
        services.AddScoped<ITransactionCodeService, TransactionCodeService>();
        services.AddScoped<ITransactionTemplateRepository, TransactionTemplateRepository>();
        services.AddScoped<ITransactionTemplateService, TransactionTemplateService>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ILoanService, LoanService>();
        services.AddScoped<IInstallmentService, InstallmentService>();
        services.AddScoped<ILoanTypeService, LoanTypeService>();
        services.AddScoped<Lazy<ITransactionService>>(provider => new Lazy<ITransactionService>(provider.GetRequiredService<ITransactionService>));

        return services;
    }

    public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        services.AddSingleton<TransactionBackgroundService>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(Configuration.Database.GetConnectionString()), ServiceLifetime.Scoped, ServiceLifetime.Singleton);
        services.AddDbContextFactory<ApplicationContext>(options => options.UseNpgsql(Configuration.Database.GetConnectionString()));

        return services;
    }

    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddSingleton<DatabaseHostedService>();
        services.AddSingleton<ExchangeHostedService>();
        services.AddSingleton<LoanHostedService>();

        services.AddHostedService<ApplicationHostedService>();

        return services;
    }

    public static IServiceCollection AddHttpServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddHttpContextAccessor();

        return services;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Continue;
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode  = CascadeMode.Stop;

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<AssemblyInfo>();

        return services;
    }

    public static IServiceCollection AddCors(this IServiceCollection services)
    {
        services.AddCors(options => options.AddPolicy(Configuration.Policy.FrontendApplication, policy => policy.WithOrigins(Configuration.Frontend.BaseUrl)
                                                                                                                .AllowAnyHeader()
                                                                                                                .AllowAnyMethod()));

        return services;
    }

    public static IServiceCollection AddOpenApiExamples(this IServiceCollection services)
    {
        services.AddOpenApiExample(Example.AccountCurrency.CreateRequest);
        services.AddOpenApiExample(Example.AccountCurrency.ClientUpdateRequest);
        // services.AddOpenApiExample(Example.AccountCurrency.Response);
        services.AddOpenApiExample(Example.Account.CreateRequest);
        services.AddOpenApiExample(Example.Account.UpdateClientRequest);
        services.AddOpenApiExample(Example.Account.UpdateEmployeeRequest);
        // services.AddOpenApiExample(Example.Account.Response);
        // services.AddOpenApiExample(Example.Account.SimpleResponse);
        services.AddOpenApiExample(Example.AccountType.CreateRequest);
        services.AddOpenApiExample(Example.AccountType.UpdateRequest);
        // services.AddOpenApiExample(Example.AccountType.Response);
        // services.AddOpenApiExample(Example.Bank.Response);
        services.AddOpenApiExample(Example.Card.CreateRequest);
        services.AddOpenApiExample(Example.Card.UpdateStatusRequest);
        services.AddOpenApiExample(Example.Card.UpdateLimitRequest);
        // services.AddOpenApiExample(Example.Card.Response);
        // services.AddOpenApiExample(Example.CardType.Response);
        services.AddOpenApiExample(Example.Client.CreateRequest);
        services.AddOpenApiExample(Example.Client.UpdateRequest);
        // services.AddOpenApiExample(Example.Client.Response);
        // services.AddOpenApiExample(Example.Client.SimpleResponse);
        services.AddOpenApiExample(Example.Company.CreateRequest);
        services.AddOpenApiExample(Example.Company.UpdateRequest);
        // services.AddOpenApiExample(Example.Company.Response);
        // services.AddOpenApiExample(Example.Company.SimpleResponse);
        // services.AddOpenApiExample(Example.Country.Response);
        // services.AddOpenApiExample(Example.Country.SimpleResponse);
        // services.AddOpenApiExample(Example.Currency.Response);
        // services.AddOpenApiExample(Example.Currency.SimpleResponse);
        services.AddOpenApiExample(Example.Employee.CreateRequest);
        services.AddOpenApiExample(Example.Employee.UpdateRequest);
        // services.AddOpenApiExample(Example.Employee.Response);
        // services.AddOpenApiExample(Example.Employee.SimpleResponse);
        services.AddOpenApiExample(Example.Exchange.MakeExchangeRequest);
        services.AddOpenApiExample(Example.Exchange.UpdateRequest);
        // services.AddOpenApiExample(Example.Exchange.Response);
        services.AddOpenApiExample(Example.Installment.CreateRequest);
        services.AddOpenApiExample(Example.Installment.UpdateRequest);
        // services.AddOpenApiExample(Example.Installment.Response);
        services.AddOpenApiExample(Example.Loan.CreateRequest);
        services.AddOpenApiExample(Example.Loan.UpdateRequest);
        // services.AddOpenApiExample(Example.Loan.Response);
        services.AddOpenApiExample(Example.LoanType.CreateRequest);
        services.AddOpenApiExample(Example.LoanType.UpdateRequest);
        // services.AddOpenApiExample(Example.LoanType.Response);
        // services.AddOpenApiExample(Example.TransactionCode.Response);
        services.AddOpenApiExample(Example.Transaction.CreateRequest);
        services.AddOpenApiExample(Example.Transaction.UpdateRequest);
        // services.AddOpenApiExample(Example.Transaction.Response);
        // services.AddOpenApiExample(Example.Transaction.CreateResponse);
        services.AddOpenApiExample(Example.TransactionTemplate.CreateRequest);
        services.AddOpenApiExample(Example.TransactionTemplate.UpdateRequest);
        // services.AddOpenApiExample(Example.TransactionTemplate.Response);
        // services.AddOpenApiExample(Example.TransactionTemplate.SimpleResponse);
        services.AddOpenApiExample(Example.User.LoginRequest);
        services.AddOpenApiExample(Example.User.ActivationRequest);
        services.AddOpenApiExample(Example.User.PasswordResetRequest);
        services.AddOpenApiExample(Example.User.RequestPasswordResetRequest);
        // services.AddOpenApiExample(Example.User.Response);
        // services.AddOpenApiExample(Example.User.SimpleResponse);
        // services.AddOpenApiExample(Example.User.LoginResponse);
        
        return services;
    }
}
