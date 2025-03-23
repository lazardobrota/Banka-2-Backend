using Bank.Application.Domain;
using Bank.UserService.Database;
using Bank.UserService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.UserService.Repositories;

public interface ILoanTypeRepository
{
    Task<Page<LoanType>> FindAll(Pageable pageable);

    Task<LoanType?> FindById(Guid id);

    Task<LoanType> Add(LoanType loanType);

    Task<LoanType> Update(LoanType loanType);
}

public class LoanTypeRepository(ApplicationContext context) : ILoanTypeRepository
{
    private readonly ApplicationContext m_Context = context;

    public async Task<Page<LoanType>> FindAll(Pageable pageable)
    {
        var query = m_Context.LoanTypes.AsQueryable();

        var total = await query.CountAsync();

        var items = await query.Skip((pageable.Page - 1) * pageable.Size)
                               .Take(pageable.Size)
                               .ToListAsync();

        return new Page<LoanType>(items, pageable.Page, pageable.Size, total);
    }

    public async Task<LoanType?> FindById(Guid id)
    {
        return await m_Context.LoanTypes.FindAsync(id);
    }

    public async Task<LoanType> Add(LoanType loanType)
    {
        m_Context.LoanTypes.Add(loanType);
        await m_Context.SaveChangesAsync();
        return loanType;
    }

    public async Task<LoanType> Update(LoanType loanType)
    {
        await m_Context.LoanTypes.Where(dbLoanType => dbLoanType.Id == loanType.Id)
                       .ExecuteUpdateAsync(setters => setters.SetProperty(dbLoanType => dbLoanType.Name, loanType.Name)
                                                             .SetProperty(dbLoanType => dbLoanType.Margin,     loanType.Margin)
                                                             .SetProperty(dbLoanType => dbLoanType.ModifiedAt, loanType.ModifiedAt));

        return loanType;
    }
}
