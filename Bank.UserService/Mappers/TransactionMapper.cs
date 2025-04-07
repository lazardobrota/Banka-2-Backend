using Bank.Application.Domain;
using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.UserService.Database.Seeders;
using Bank.UserService.Models;

namespace Bank.UserService.Mappers;

public static class TransactionMapper
{
    public static TransactionCreateResponse ToCreateResponse(this Transaction transaction)
    {
        return new TransactionCreateResponse
               {
                   Id              = transaction.Id,
                   FromAmount      = transaction.FromAmount,
                   Code            = transaction.Code?.ToResponse()!,
                   ReferenceNumber = transaction.ReferenceNumber!,
                   Purpose         = transaction.Purpose,
                   Status          = transaction.Status,
                   CreatedAt       = transaction.CreatedAt,
                   ModifiedAt      = transaction.ModifiedAt,
               };
    }

    public static TransactionResponse ToResponse(this Transaction transaction)
    {
        return new TransactionResponse
               {
                   Id              = transaction.Id,
                   FromAccount     = transaction.FromAccount?.ToSimpleResponse()!,
                   ToAccount       = transaction.ToAccount?.ToSimpleResponse()!,
                   FromAmount      = transaction.FromAmount,
                   ToAmount        = transaction.ToAmount,
                   Code            = transaction.Code?.ToResponse()!,
                   ReferenceNumber = transaction.ReferenceNumber!,
                   Purpose         = transaction.Purpose,
                   Status          = transaction.Status,
                   CreatedAt       = transaction.CreatedAt,
                   ModifiedAt      = transaction.ModifiedAt,
               };
    }

    public static Transaction ToTransaction(this TransactionCreateRequest transactionCreateRequest, TransactionCode code)
    {
        return new Transaction
               {
                   Id              = Guid.NewGuid(),
                   FromAccountId   = transactionCreateRequest.FromAccountId,
                   FromCurrencyId  = transactionCreateRequest.FromCurrencyId,
                   ToAccountId     = null, // TODO: vrv null 
                   ToCurrencyId    = transactionCreateRequest.ToCurrencyId,
                   FromAmount      = transactionCreateRequest.Amount, // TODO: currency
                   ToAmount        = 0,                               // TODO: currency optional
                   CodeId          = transactionCreateRequest.CodeId,
                   Code            = code,
                   ReferenceNumber = transactionCreateRequest.ReferenceNumber,
                   Purpose         = transactionCreateRequest.Purpose,
                   Status          = TransactionStatus.Pending,
                   CreatedAt       = DateTime.UtcNow,
                   ModifiedAt      = DateTime.UtcNow,
               };
    }

    public static Transaction ToTransaction(this TransactionUpdateRequest transactionUpdateRequest, Transaction oldTransaction)
    {
        return new Transaction
               {
                   Id              = oldTransaction.Id,
                   FromAccountId   = oldTransaction.FromAccountId,
                   FromCurrencyId  = oldTransaction.FromCurrencyId,
                   ToAccountId     = oldTransaction.ToAccountId,
                   ToCurrencyId    = oldTransaction.ToCurrencyId,
                   FromAmount      = oldTransaction.FromAmount,
                   ToAmount        = oldTransaction.ToAmount,
                   Code            = oldTransaction.Code,
                   ReferenceNumber = oldTransaction.ReferenceNumber,
                   Purpose         = oldTransaction.Purpose,
                   Status          = transactionUpdateRequest.Status,
                   CreatedAt       = oldTransaction.CreatedAt,
                   CodeId          = oldTransaction.CodeId,
                   ModifiedAt      = DateTime.UtcNow,
               };
    }

    public static Transaction ToTransaction(this PrepareWithdrawTransaction withdrawTransaction)
    {
        return new Transaction
               {
                   Id             = Guid.NewGuid(),
                   FromAccountId  = withdrawTransaction.Account!.Id,
                   FromCurrencyId = withdrawTransaction.Currency!.Id,
                   FromAmount     = withdrawTransaction.Amount,
                   CodeId         = Seeder.TransactionCode.TransactionCode266.Id,
                   Status         = TransactionStatus.Pending,
                   CreatedAt      = DateTime.UtcNow,
                   ModifiedAt     = DateTime.UtcNow
               };
    }

    public static Transaction ToTransaction(this PrepareDepositTransaction depositTransaction)
    {
        return new Transaction
               {
                   Id           = Guid.NewGuid(),
                   ToAccountId  = depositTransaction.Account!.Id,
                   ToCurrencyId = depositTransaction.Currency!.Id,
                   ToAmount     = depositTransaction.Amount,
                   CodeId       = Seeder.TransactionCode.TransactionCode289.Id,
                   Status       = TransactionStatus.Pending,
                   CreatedAt    = DateTime.UtcNow,
                   ModifiedAt   = DateTime.UtcNow
               };
    }

    public static Transaction ToTransaction(this PrepareInternalTransaction internalTransaction)
    {
        return new Transaction
               {
                   Id              = Guid.NewGuid(),
                   FromAccountId   = internalTransaction.FromAccount!.Id,
                   FromCurrencyId  = internalTransaction.FromCurrency!.Id,
                   FromAmount      = internalTransaction.FromAmount,
                   ToAccountId     = internalTransaction.ToAccount!.Id,
                   ToCurrencyId    = internalTransaction.ToCurrency!.Id,
                   ToAmount        = internalTransaction.ToAmount,
                   CodeId          = internalTransaction.TransactionCode!.Id,
                   Status          = TransactionStatus.Pending,
                   Purpose         = internalTransaction.Purpose,
                   ReferenceNumber = internalTransaction.ReferenceNumber,
                   CreatedAt       = DateTime.UtcNow,
                   ModifiedAt      = DateTime.UtcNow
               };
    }

    public static ProcessTransaction ToProcessTransaction(this PrepareWithdrawTransaction withdrawTransaction, Guid transactionId)
    {
        return new ProcessTransaction
               {
                   TransactionId  = transactionId,
                   FromAccountId  = withdrawTransaction.Account!.Id,
                   FromCurrencyId = withdrawTransaction.Currency!.Id,
                   FromAmount     = withdrawTransaction.Amount,
                   ToAccountId    = Guid.Empty,
                   ToCurrencyId   = Guid.Empty,
                   ToAmount       = 0
               };
    }

    public static ProcessTransaction ToProcessTransaction(this PrepareDepositTransaction depositTransaction, Guid transactionId)
    {
        return new ProcessTransaction
               {
                   TransactionId  = transactionId,
                   FromAccountId  = Guid.Empty,
                   FromCurrencyId = Guid.Empty,
                   FromAmount     = 0,
                   ToAccountId    = depositTransaction.Account!.Id,
                   ToCurrencyId   = depositTransaction.Currency!.Id,
                   ToAmount       = depositTransaction.Amount
               };
    }

    public static ProcessTransaction ToProcessTransaction(this PrepareInternalTransaction internalTransaction, Guid transactionId)
    {
        return new ProcessTransaction
               {
                   TransactionId  = transactionId,
                   FromAccountId  = internalTransaction.FromAccount!.Id,
                   FromCurrencyId = internalTransaction.FromCurrency!.Id,
                   FromAmount     = internalTransaction.FromAmount,
                   ToAccountId    = internalTransaction.ToAccount!.Id,
                   ToCurrencyId   = internalTransaction.ToCurrency!.Id,
                   ToAmount       = internalTransaction.ToAmount,
               };
    }
}
