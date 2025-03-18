using Bank.Application.Domain;
using Bank.UserService.Database;
using Bank.UserService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.UserService.Repositories;

public interface IInstallmentRepository
{
    Task<Page<Installment>> FindAllByLoanId(Guid loanId, Pageable pageable);

    Task<Installment?> FindById(Guid id);

    Task<Installment> Add(Installment installment);

    Task<Installment> Update(Installment oldInstallment, Installment installment);

    Task<List<Installment>> GetDueInstallmentsForLoanAsync(Guid loanId, DateTime dueDate);

    Task<int> GetInstallmentCountForLoanAsync(Guid loanId);

    Task<int> GetPaidInstallmentsCountBeforeDateAsync(Guid loanId, DateTime date);

    Task<bool> AreAllInstallmentsPaidAsync(Guid loanId);

    Task<Installment?> GetLatestInstallmentForLoanAsync(Guid loanId);
}

public class InstallmentRepository(ApplicationContext context) : IInstallmentRepository
{
    private readonly ApplicationContext m_Context = context;

    public async Task<Page<Installment>> FindAllByLoanId(Guid loanId, Pageable pageable)
    {
        var query = m_Context.Installments.Include(i => i.Loan)
                             .Include(i => i.Loan.Account)
                             .Include(i => i.Loan.Account.Client)
                             .Include(i => i.Loan.Currency)
                             .Include(i => i.Loan.LoanType)
                             .Where(i => i.LoanId == loanId)
                             .OrderBy(i => i.ExpectedDueDate);

        var total = await query.CountAsync();

        var items = await query.Skip((pageable.Page - 1) * pageable.Size)
                               .Take(pageable.Size)
                               .ToListAsync();

        return new Page<Installment>(items, pageable.Page, pageable.Size, total);
    }

    public async Task<Installment?> FindById(Guid id)
    {
        return await m_Context.Installments.Include(i => i.Loan)
                              .Include(i => i.Loan.Account)
                              .Include(i => i.Loan.Account)
                              .Include(i => i.Loan.Currency)
                              .Include(i => i.Loan.LoanType)
                              .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Installment> Add(Installment installment)
    {
        m_Context.Installments.Add(installment);
        await m_Context.SaveChangesAsync();
        return installment;
    }

    public async Task<Installment> Update(Installment oldInstallment, Installment installment)
    {
        m_Context.Entry(oldInstallment)
                 .State = EntityState.Detached;

        var updatedInstallment = m_Context.Installments.Update(installment);

        await m_Context.SaveChangesAsync();

        return updatedInstallment.Entity;
    }

    public async Task<List<Installment>> GetDueInstallmentsForLoanAsync(Guid loanId, DateTime dueDate)
    {
        return await m_Context.Installments.Where(i => i.LoanId == loanId && i.ExpectedDueDate.Date <= dueDate.Date && i.Status == InstallmentStatus.Pending)
                              .OrderBy(i => i.ExpectedDueDate)
                              .ToListAsync();
    }

    public async Task<int> GetInstallmentCountForLoanAsync(Guid loanId)
    {
        return await m_Context.Installments.CountAsync(i => i.LoanId == loanId);
    }

    public async Task<int> GetPaidInstallmentsCountBeforeDateAsync(Guid loanId, DateTime date)
    {
        return await m_Context.Installments.CountAsync(i => i.LoanId == loanId && i.Status == InstallmentStatus.Paid && i.ExpectedDueDate < date);
    }

    public async Task<bool> AreAllInstallmentsPaidAsync(Guid loanId)
    {
        return !await m_Context.Installments.AnyAsync(i => i.LoanId == loanId && i.Status == InstallmentStatus.Pending);
    }

    public async Task<Installment?> GetLatestInstallmentForLoanAsync(Guid loanId)
    {
        return await m_Context.Installments.Where(i => i.LoanId == loanId)
                              .OrderByDescending(i => i.ExpectedDueDate)
                              .FirstOrDefaultAsync();
    }
}
