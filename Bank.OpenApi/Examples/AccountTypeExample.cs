using Bank.Application.Requests;
using Bank.Application.Responses;

namespace Bank.OpenApi.Examples;

public static partial class Example
{
    public static class AccountType
    {
        public static readonly AccountTypeCreateRequest CreateRequest = new()
                                                                        {
                                                                            Name = Constant.AccountTypeName,
                                                                            Code = Constant.AccountTypeCode
                                                                        };

        public static readonly AccountTypeUpdateRequest UpdateRequest = new()
                                                                        {
                                                                            Name = Constant.AccountTypeName,
                                                                            Code = Constant.AccountTypeCode
                                                                        };

        public static readonly AccountTypeResponse Response = new()
                                                              {
                                                                  Id         = Constant.Id,
                                                                  Name       = Constant.AccountTypeName,
                                                                  Code       = Constant.AccountTypeCode,
                                                                  CreatedAt  = Constant.CreatedAt,
                                                                  ModifiedAt = Constant.ModifiedAt
                                                              };
    }
}
