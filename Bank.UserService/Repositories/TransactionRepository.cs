using Bank.Application.Domain;
using Bank.Application.Queries;
using Bank.UserService.Database;
using Bank.UserService.Models;
using Bank.UserService.Services;

using Microsoft.EntityFrameworkCore;

namespace Bank.UserService.Repositories;

public interface ITransactionRepository
{
    Task<Page<Transaction>> FindAll(TransactionFilterQuery filter, Pageable pageable);

    Task<Page<Transaction>> FindAllByAccountId(Guid accountId, TransactionFilterQuery filter, Pageable pageable);

    Task<Transaction?> FindById(Guid id);

    Task<Transaction> Add(Transaction transaction);

    Task<Transaction> Update(Transaction transaction);

    Task<bool> UpdateStatus(Guid id, TransactionStatus status);
}

public class TransactionRepository(ApplicationContext context, IAuthorizationService authorizationService, IDbContextFactory<ApplicationContext> contextFactory)
: ITransactionRepository
{
    private readonly ApplicationContext                    m_Context              = context;
    private readonly IAuthorizationService                 m_AuthorizationService = authorizationService;
    private readonly IDbContextFactory<ApplicationContext> m_ContextFactory       = contextFactory;

    private Task<ApplicationContext> CreateContext => m_ContextFactory.CreateDbContextAsync();

    public async Task<Page<Transaction>> FindAll(TransactionFilterQuery filter, Pageable pageable)
    {
        var transactionQuery = m_Context.Transactions.Include(transaction => transaction.FromAccount)
                                        .Include(transaction => transaction.ToAccount)
                                        .Include(transaction => transaction.Code)
                                        .AsQueryable();

        if (m_AuthorizationService.Role == Role.Client)
            transactionQuery = transactionQuery.Where(transaction => transaction.FromAccount!.ClientId == m_AuthorizationService.UserId ||
                                                                     transaction.ToAccount!.ClientId   == m_AuthorizationService.UserId);

        if (filter.Status != TransactionStatus.Invalid)
            transactionQuery = transactionQuery.Where(transaction => transaction.Status == filter.Status);

        if (filter.FromDate != DateOnly.MinValue)
            transactionQuery = transactionQuery.Where(transaction => DateOnly.FromDateTime(transaction.CreatedAt) >= filter.FromDate);

        if (filter.ToDate != DateOnly.MinValue)
            transactionQuery = transactionQuery.Where(transaction => DateOnly.FromDateTime(transaction.CreatedAt) <= filter.ToDate);

        transactionQuery = transactionQuery.OrderByDescending(transaction => transaction.CreatedAt);

        var transactions = await transactionQuery.Skip((pageable.Page - 1) * pageable.Size)
                                                 .Take(pageable.Size)
                                                 .ToListAsync();

        var totalElements = await transactionQuery.CountAsync();

        return new Page<Transaction>(transactions, pageable.Page, pageable.Size, totalElements);
    }

    public async Task<Page<Transaction>> FindAllByAccountId(Guid accountId, TransactionFilterQuery filter, Pageable pageable)
    {
        var transactionQuery = m_Context.Transactions.Include(transaction => transaction.FromAccount)
                                        .Include(transaction => transaction.FromAccount!.Type)
                                        .Include(transaction => transaction.FromAccount!.Client!.Bank)
                                        .Include(transaction => transaction.ToAccount)
                                        .Include(transaction => transaction.ToAccount!.Type)
                                        .Include(transaction => transaction.ToAccount!.Client!.Bank)
                                        .Include(transaction => transaction.Code)
                                        .AsQueryable();

        if (filter.Status != TransactionStatus.Invalid)
            transactionQuery = transactionQuery.Where(transaction => transaction.Status == filter.Status);

        if (filter.FromDate != DateOnly.MinValue)
            transactionQuery = transactionQuery.Where(transaction => DateOnly.FromDateTime(transaction.CreatedAt) >= filter.FromDate);

        if (filter.ToDate != DateOnly.MinValue)
            transactionQuery = transactionQuery.Where(transaction => DateOnly.FromDateTime(transaction.CreatedAt) <= filter.ToDate);

        transactionQuery = transactionQuery.Where(transaction => transaction.FromAccountId == accountId || transaction.ToAccountId == accountId);

        var transactions = await transactionQuery.Skip((pageable.Page - 1) * pageable.Size)
                                                 .Take(pageable.Size)
                                                 .ToListAsync();

        var totalElements = await transactionQuery.CountAsync();

        return new Page<Transaction>(transactions, pageable.Page, pageable.Size, totalElements);
    }

    public async Task<Transaction?> FindById(Guid id)
    {
        await using var context = await CreateContext;

        return await FindById(id, context);
    }

    public async Task<Transaction> Add(Transaction transaction)
    {
        await using var context = await CreateContext;

        var addedTransaction = await context.Transactions.AddAsync(transaction);

        await context.SaveChangesAsync();

        return addedTransaction.Entity;
    }

    public async Task<Transaction> Update(Transaction transaction)
    {
        await m_Context.Transactions.Where(dbTransaction => dbTransaction.Id == transaction.Id)
                       .ExecuteUpdateAsync(setters => setters.SetProperty(dbTransaction => dbTransaction.Status, transaction.Status)
                                                             .SetProperty(dbTransaction => dbTransaction.ModifiedAt, transaction.ModifiedAt));

        return transaction;
    }

    public async Task<bool> UpdateStatus(Guid id, TransactionStatus status)
    {
        await using var context = await CreateContext;

        return await UpdateStatus(id, status, context);
    }

    public static async Task<bool> UpdateStatus(Guid id, TransactionStatus status, ApplicationContext context)
    {
        var updatedRows = await context.Transactions.Where(transaction => transaction.Id == id)
                                       .ExecuteUpdateAsync(setProperty => setProperty.SetProperty(transaction => transaction.Status, status)
                                                                                     .SetProperty(transaction => transaction.ModifiedAt, DateTime.UtcNow));

        return updatedRows == 1;
    }

    public static async Task<Transaction?> FindById(Guid id, ApplicationContext context)
    {
        return await context.Transactions.Include(transaction => transaction.FromAccount)
                            .Include(transaction => transaction.ToAccount)
                            .Include(transaction => transaction.Code)
                            .FirstOrDefaultAsync(transaction => transaction.Id == id);
    }
}
