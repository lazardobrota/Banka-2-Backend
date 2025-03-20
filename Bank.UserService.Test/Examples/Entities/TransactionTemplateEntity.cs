using Bank.Application.Requests;
using Bank.UserService.Database.Seeders;
using Bank.UserService.Models;

namespace Bank.UserService.Test.Examples.Entities;

using TransactionTemplateModel = TransactionTemplate;

public static partial class Example
{
    public static partial class Entity
    {
        public static class TransactionTemplate
        {
            public static readonly TransactionTemplateCreateRequest TransactionTemplateCreateRequest = new()
                                                                                                       {
                                                                                                           AccountNumber = "11111",
                                                                                                           Name          = "Slavko RSD"
                                                                                                       };

            public static readonly TransactionTemplateUpdateRequest TransactionTemplateUpdateRequest = new()
                                                                                                       {
                                                                                                           AccountNumber = "22222",
                                                                                                           Deleted       = false,
                                                                                                           Name          = "Mirko EUR"
                                                                                                       };

            public static readonly TransactionTemplateModel GetTransactionTemplate = new()
                                                                                    {
                                                                                        Id            = Seeder.TransactionTemplate.TransactionTemplate01.Id,
                                                                                        ClientId      = Seeder.TransactionTemplate.TransactionTemplate01.ClientId,
                                                                                        Name          = Seeder.TransactionTemplate.TransactionTemplate01.Name,
                                                                                        AccountNumber = Seeder.TransactionTemplate.TransactionTemplate01.AccountNumber,
                                                                                        Deleted       = Seeder.TransactionTemplate.TransactionTemplate01.Deleted,
                                                                                        CreatedAt     = Seeder.TransactionTemplate.TransactionTemplate01.CreatedAt,
                                                                                        ModifiedAt    = Seeder.TransactionTemplate.TransactionTemplate01.ModifiedAt,
                                                                                    };

            public static readonly TransactionTemplateModel UpdateTransactionTemplate = new()
                                                                                           {
                                                                                               Id            = Seeder.TransactionTemplate.TransactionTemplate02.Id,
                                                                                               ClientId      = Seeder.TransactionTemplate.TransactionTemplate02.ClientId,
                                                                                               Name          = Seeder.TransactionTemplate.TransactionTemplate02.Name,
                                                                                               AccountNumber = Seeder.TransactionTemplate.TransactionTemplate02.AccountNumber,
                                                                                               Deleted       = Seeder.TransactionTemplate.TransactionTemplate02.Deleted,
                                                                                               CreatedAt     = Seeder.TransactionTemplate.TransactionTemplate02.CreatedAt,
                                                                                               ModifiedAt    = Seeder.TransactionTemplate.TransactionTemplate02.ModifiedAt,
                                                                                           };
        }
    }
}
