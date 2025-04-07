using Bank.Application.Domain;
using Bank.UserService.Database;
using Bank.UserService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.UserService.Repositories;

public interface IAccountCurrencyRepository
{
    Task<Page<AccountCurrency>> FindAll(Pageable pageable);

    Task<AccountCurrency?> FindById(Guid id);

    Task<bool> Exists(Guid id);

    Task<AccountCurrency> Add(AccountCurrency accountCurrency);

    Task<AccountCurrency> Update(AccountCurrency accountCurrency);
}

public class AccountCurrencyRepository(ApplicationContext context, IDbContextFactory<ApplicationContext> contextFactory) : IAccountCurrencyRepository
{
    private readonly ApplicationContext                    m_Context        = context;
    private readonly IDbContextFactory<ApplicationContext> m_ContextFactory = contextFactory;

    private Task<ApplicationContext> CreateContext => m_ContextFactory.CreateDbContextAsync();
    
    public async Task<Page<AccountCurrency>> FindAll(Pageable pageable)
    {
        var accountTypeQuery = m_Context.AccountCurrencies.Include(accountCurrency => accountCurrency.Employee)
                                        .Include(accountCurrency => accountCurrency.Currency)
                                        .Include(accountCurrency => accountCurrency.Account)
                                        .AsQueryable();

        var accountTypes = await accountTypeQuery.Skip((pageable.Page - 1) * pageable.Size)
                                                 .Take(pageable.Size)
                                                 .ToListAsync();

        var totalElements = await accountTypeQuery.CountAsync();

        return new Page<AccountCurrency>(accountTypes, pageable.Page, pageable.Size, totalElements);
    }

    public async Task<AccountCurrency?> FindById(Guid id)
    {
        return await m_Context.AccountCurrencies.Include(accountCurrency => accountCurrency.Employee)
                              .Include(accountCurrency => accountCurrency.Currency)
                              .Include(accountCurrency => accountCurrency.Account)
                              .FirstOrDefaultAsync(accountCurrency => accountCurrency.Id == id);
    }

    public async Task<bool> Exists(Guid id)
    {
        return await m_Context.AccountCurrencies.AnyAsync(accountCurrency => accountCurrency.Id == id);
    }

    public async Task<AccountCurrency> Add(AccountCurrency accountCurrency)
    {
        var addedAccountCurrency = await m_Context.AccountCurrencies.AddAsync(accountCurrency);

        await m_Context.SaveChangesAsync();

        return addedAccountCurrency.Entity;
    }

    public async Task<AccountCurrency> Update(AccountCurrency accountCurrency)
    {
        await m_Context.AccountCurrencies.Where(dbAccountCurrency => dbAccountCurrency.Id == accountCurrency.Id)
                       .ExecuteUpdateAsync(setters => setters.SetProperty(dbAccountCurrency => dbAccountCurrency.DailyLimit, accountCurrency.DailyLimit)
                                                             .SetProperty(dbAccountCurrency => dbAccountCurrency.MonthlyLimit, accountCurrency.MonthlyLimit)
                                                             .SetProperty(dbAccountCurrency => dbAccountCurrency.ModifiedAt,   accountCurrency.ModifiedAt));

        return accountCurrency;
    }
}
