using Bank.Application.Requests;

namespace Bank.OpenApi.Examples;

public static partial class Example
{
    public static class TransactionTemplate
    {
        public static readonly TransactionTemplateCreateRequest CreateRequest = new()
                                                                                {
                                                                                    AccountNumber = Constant.AccountNumber,
                                                                                    Name          = Constant.TemplateName,
                                                                                };

        public static readonly TransactionTemplateUpdateRequest UpdateRequest = new()
                                                                                {
                                                                                    AccountNumber = Constant.AccountNumber,
                                                                                    Deleted       = Constant.Boolean,
                                                                                    Name          = Constant.TemplateName,
                                                                                };
    }
}
