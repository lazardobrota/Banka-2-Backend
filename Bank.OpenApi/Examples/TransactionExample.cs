using Bank.Application.Requests;
using Bank.Application.Responses;

namespace Bank.OpenApi.Examples;

public static partial class Example
{
    public static class Transaction
    {
        public static readonly TransactionCreateRequest CreateRequest = new()
                                                                        {
                                                                            FromAccountNumber = Constant.AccountNumber,
                                                                            FromCurrencyId    = Constant.Id,
                                                                            ToAccountNumber   = Constant.AccountNumber,
                                                                            ToCurrencyId      = Constant.Id,
                                                                            Amount            = Constant.Amount,
                                                                            CodeId            = Constant.Id,
                                                                            ReferenceNumber   = Constant.UniqueIdentificationNumber,
                                                                            Purpose           = Constant.Description,
                                                                        };

        public static readonly TransactionUpdateRequest UpdateRequest = new()
                                                                        {
                                                                            Status = Constant.TransactionStatus,
                                                                        };

        public static readonly TransactionResponse Response = new()
                                                              {
                                                                  Id              = Constant.Id,
                                                                  FromAccount     = Account.SimpleResponse,
                                                                  FromCurrency    = Currency.Response,
                                                                  FromAmount      = Constant.Amount,
                                                                  ToAccount       = Account.SimpleResponse,
                                                                  ToCurrency      = Currency.Response,
                                                                  ToAmount        = Constant.Amount,
                                                                  Code            = TransactionCode.Response,
                                                                  ReferenceNumber = Constant.UniqueIdentificationNumber,
                                                                  Purpose         = Constant.Description,
                                                                  Status          = Constant.TransactionStatus,
                                                                  CreatedAt       = Constant.CreatedAt,
                                                                  ModifiedAt      = Constant.ModifiedAt,
                                                              };

        public static readonly TransactionCreateResponse CreateResponse = new()
                                                                          {
                                                                              Id              = Constant.Id,
                                                                              FromAmount      = Constant.Amount,
                                                                              Code            = TransactionCode.Response,
                                                                              ReferenceNumber = Constant.UniqueIdentificationNumber,
                                                                              Purpose         = Constant.Description,
                                                                              Status          = Constant.TransactionStatus,
                                                                              CreatedAt       = Constant.CreatedAt,
                                                                              ModifiedAt      = Constant.ModifiedAt,
                                                                          };
    }
}
