﻿using Bank.Application.Domain;
using Bank.Application.Queries;
using Bank.Application.Requests;
using Bank.UserService.Database.Seeders;

namespace Bank.UserService.Test.Examples.Entities;

public static partial class Example
{
    public static partial class Entity
    {
        public static class Transaction
        {
            public static readonly TransactionCreateRequest CreateRequest = new()
                                                                            {
                                                                                FromAccountId   = Seeder.Account.DomesticAccount01.Id,
                                                                                FromCurrencyId  = Seeder.Currency.SerbianDinar.Id,
                                                                                ToAccountNumber = Seeder.Account.ForeignAccount01.Number,
                                                                                ToCurrencyId    = Seeder.Currency.Euro.Id,
                                                                                Amount          = 10.05m,
                                                                                CodeId          = Seeder.TransactionCode.TransactionCode224.Id,
                                                                                ReferenceNumber = "2345454333",
                                                                                Purpose         = "Za testiranje"
                                                                            };

            public static readonly TransactionUpdateRequest UpdateRequest = new()
                                                                            {
                                                                                Status = TransactionStatus.Failed
                                                                            };

            public static readonly Guid TransactionId = Seeder.Transaction.Transaction01.Id;

            public static readonly Guid AccountId = Seeder.Account.DomesticAccount01.Id;

            public static readonly TransactionFilterQuery FilterQuery = new() { };

            public static readonly Pageable Pageable = new()
                                                       {
                                                           Page = 1,
                                                           Size = 10
                                                       };
        }
    }
}
