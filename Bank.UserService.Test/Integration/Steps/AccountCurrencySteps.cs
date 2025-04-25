using System.Reflection;

using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.Permissions.Services;
using Bank.UserService.Services;
using Bank.UserService.Test.Examples.Entities;

using Microsoft.AspNetCore.Mvc;

using Shouldly;

namespace Bank.UserService.Test.Integration.Steps;

[Binding]
public class AccountCurrencySteps(ScenarioContext context, IAccountCurrencyService accountCurrencyService, IAuthorizationService authorizationService)
{
    private readonly ScenarioContext         m_ScenarioContext        = context;
    private readonly IAccountCurrencyService m_AccountCurrencyService = accountCurrencyService;
    private readonly IAuthorizationService   m_AuthorizationService   = authorizationService;

    [Given(@"account currency create request")]
    public void GivenAccountCurrencyCreateRequest()
    {
        m_AuthorizationService.GetType()
                              .GetField($"<{nameof(IAuthorizationService.UserId)}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!
                              .SetValue(m_AuthorizationService, Guid.Parse("5817c260-e4a9-4dc1-87d9-2fa12af157d9"));

        m_ScenarioContext[Constant.AccountCurrencyCreateRequest] = Example.Entity.AccountCurrency.CreateRequest;
    }

    [When(@"account currency is created in the database")]
    public async Task WhenAccountCurrencyIsCreatedInTheDatabase()
    {
        var accountCurrencyCreateRequest = m_ScenarioContext.Get<AccountCurrencyCreateRequest>(Constant.AccountCurrencyCreateRequest);

        var accountCurrencyResult = await m_AccountCurrencyService.Create(accountCurrencyCreateRequest);

        m_ScenarioContext[Constant.AccountCurrencyCreateResult] = accountCurrencyResult;
    }

    [When(@"account currency is fetched by Id")]
    public async Task WhenAccountCurrencyIsFetchedById()
    {
        var createAccountCurrencyResult = m_ScenarioContext.Get<Result<AccountCurrencyResponse>>(Constant.AccountCurrencyCreateResult);

        var getAccountCurrencyResult = createAccountCurrencyResult.Value != null
                                       ? await m_AccountCurrencyService.GetOne(createAccountCurrencyResult.Value.Id)
                                       : Result.BadRequest<AccountCurrencyResponse>();

        m_ScenarioContext[Constant.AccountCurrencyCreateResult] = getAccountCurrencyResult;
    }

    [Then(@"account currency details should match the created account currency")]
    public void ThenAccountCurrencyDetailsShouldMatchTheCreatedAccountCurrency()
    {
        var getAccountCurrencyResult = m_ScenarioContext.Get<Result<AccountCurrencyResponse>>(Constant.AccountCurrencyCreateResult);

        getAccountCurrencyResult.ActionResult.ShouldBeOfType<OkObjectResult>();
        getAccountCurrencyResult.Value.ShouldNotBeNull();
        getAccountCurrencyResult.Value!.Account.Id.ShouldBe(Example.Entity.AccountCurrency.CreateRequest.AccountId);
        getAccountCurrencyResult.Value!.Currency.Id.ShouldBe(Example.Entity.AccountCurrency.CreateRequest.CurrencyId);
        getAccountCurrencyResult.Value!.Employee.Id.ShouldBe(Example.Entity.AccountCurrency.CreateRequest.EmployeeId);
        getAccountCurrencyResult.Value!.DailyLimit.ShouldBe(Example.Entity.AccountCurrency.CreateRequest.DailyLimit);
        getAccountCurrencyResult.Value!.MonthlyLimit.ShouldBe(Example.Entity.AccountCurrency.CreateRequest.MonthlyLimit);
    }

    [Given(@"account currency update request")]
    public void GivenAccountCurrencyUpdateRequest()
    {
        m_ScenarioContext[Constant.AccountCurrencyUpdateRequest] = Example.Entity.AccountCurrency.ClientUpdateRequest;
    }

    [Given(@"account currency Id")]
    public void GivenAccountCurrencyId()
    {
        m_ScenarioContext[Constant.AccountCurrencyId] = Example.Entity.AccountCurrency.AccountCurrencyId;
    }

    [When(@"account currency is updated in the database")]
    public async Task WhenAccountCurrencyIsUpdatedInTheDatabase()
    {
        var accountCurrencyUpdateRequest = m_ScenarioContext.Get<AccountCurrencyClientUpdateRequest>(Constant.AccountCurrencyUpdateRequest);
        var accountCurrencyId            = m_ScenarioContext.Get<Guid>(Constant.AccountCurrencyId);

        var accountCurrencyResult = await m_AccountCurrencyService.Update(accountCurrencyUpdateRequest, accountCurrencyId);

        m_ScenarioContext[Constant.AccountCurrencyUpdateResult] = accountCurrencyResult;
    }

    [Then(@"account currency details should match the updated account currency")]
    public void ThenAccountCurrencyDetailsShouldMatchTheUpdatedAccountCurrency()
    {
        var getAccountCurrencyResult = m_ScenarioContext.Get<Result<AccountCurrencyResponse>>(Constant.AccountCurrencyUpdateResult);

        getAccountCurrencyResult.ActionResult.ShouldBeOfType<OkObjectResult>();
        getAccountCurrencyResult.Value.ShouldNotBeNull();
        getAccountCurrencyResult.Value!.DailyLimit.ShouldBe(Example.Entity.AccountCurrency.ClientUpdateRequest.DailyLimit);
        getAccountCurrencyResult.Value!.MonthlyLimit.ShouldBe(Example.Entity.AccountCurrency.ClientUpdateRequest.MonthlyLimit);
    }

    [When(@"all account currencies are fetched")]
    public async Task WhenAllAccountCurrenciesAreFetched()
    {
        var accountCurrencies = await m_AccountCurrencyService.GetAll(new Pageable());

        m_ScenarioContext[Constant.AccountCurrencies] = accountCurrencies;
    }

    [Then(@"all account currencies should be returned")]
    public void ThenAllAccountCurrenciesShouldBeReturned()
    {
        var accountCurrenciesResult = m_ScenarioContext.Get<Result<Page<AccountCurrencyResponse>>>(Constant.AccountCurrencies);

        accountCurrenciesResult.ActionResult.ShouldBeOfType<OkObjectResult>();
        accountCurrenciesResult.Value.ShouldNotBeNull();
        accountCurrenciesResult.Value!.Items.ShouldNotBeEmpty();
    }
}

file static class Constant
{
    public const string AccountCurrencyCreateRequest = "AccountCurrencyCreateRequest";
    public const string AccountCurrencyCreateResult  = "AccountCurrencyCreateResult";
    public const string AccountCurrencyUpdateRequest = "AccountCurrencyUpdateRequest";
    public const string AccountCurrencyId            = "AccountCurrencyId";
    public const string AccountCurrencyUpdateResult  = "AccountCurrencyUpdateResult";
    public const string AccountCurrencies            = "AccountCurrencies";
}
