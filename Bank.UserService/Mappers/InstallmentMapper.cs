﻿using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.UserService.Models;

namespace Bank.UserService.Mappers;

public static class InstallmentMapper
{
    public static Installment ToInstallment(this InstallmentRequest request)
    {
        return new Installment
               {
                   Id              = Guid.NewGuid(),
                   LoanId          = request.LoanId,
                   InterestRate    = request.InterestRate,
                   ExpectedDueDate = DateTime.SpecifyKind(request.ExpectedDueDate.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc),
                   ActualDueDate   = DateTime.SpecifyKind(request.ActualDueDate.ToDateTime(TimeOnly.MinValue),   DateTimeKind.Utc),
                   Status          = request.Status,
                   CreatedAt       = DateTime.UtcNow,
                   ModifiedAt      = DateTime.UtcNow
               };
    }

    public static InstallmentResponse ToResponse(this Installment installment)
    {
        return new InstallmentResponse
               {
                   Id              = installment.Id,
                   Loan            = installment.Loan.ToLoanResponse(),
                   InterestRate    = installment.InterestRate,
                   ExpectedDueDate = DateOnly.FromDateTime(installment.ExpectedDueDate),
                   ActualDueDate   = DateOnly.FromDateTime(installment.ActualDueDate),
                   Status          = installment.Status,
                   CreatedAt       = installment.CreatedAt,
                   ModifiedAt      = installment.ModifiedAt
               };
    }

    public static Installment ToEntity(this InstallmentUpdateRequest request, Installment oldInstallment)
    {
        var updatedInstallment = new Installment
                                 {
                                     Id            = oldInstallment.Id,
                                     LoanId        = oldInstallment.LoanId,
                                     Loan          = oldInstallment.Loan,
                                     ActualDueDate = request.ActualDueDate ?? oldInstallment.ActualDueDate,
                                     InterestRate  = oldInstallment.InterestRate,
                                     Status        = request.Status ?? oldInstallment.Status,
                                     CreatedAt     = oldInstallment.CreatedAt,
                                     ModifiedAt    = DateTime.UtcNow
                                 };

        return updatedInstallment;
    }
}
