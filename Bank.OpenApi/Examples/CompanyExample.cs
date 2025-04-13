using Bank.Application.Requests;
using Bank.Application.Responses;

namespace Bank.OpenApi.Examples;

public static partial class Example
{
    public static class Company
    {
        public static readonly CompanyCreateRequest CreateRequest = new()
                                                                    {
                                                                        Name                    = Constant.CompanyName,
                                                                        RegistrationNumber      = Constant.UniqueIdentificationNumber,
                                                                        TaxIdentificationNumber = Constant.UniqueIdentificationNumber,
                                                                        ActivityCode            = Constant.CompanyActivityCode,
                                                                        Address                 = Constant.Address,
                                                                        MajorityOwnerId         = Constant.Id,
                                                                    };

        public static readonly CompanyUpdateRequest UpdateRequest = new()
                                                                    {
                                                                        Name            = Constant.CompanyName,
                                                                        ActivityCode    = Constant.CompanyActivityCode,
                                                                        Address         = Constant.Address,
                                                                        MajorityOwnerId = Constant.Id,
                                                                    };

        public static readonly CompanyResponse Response = new()
                                                          {
                                                              Id                      = Constant.Id,
                                                              Name                    = Constant.CompanyName,
                                                              RegistrationNumber      = Constant.UniqueIdentificationNumber,
                                                              TaxIdentificationNumber = Constant.UniqueIdentificationNumber,
                                                              ActivityCode            = Constant.CompanyActivityCode,
                                                              Address                 = Constant.Address,
                                                              MajorityOwner           = User.SimpleResponse,
                                                          };

        public static readonly CompanySimpleResponse SimpleResponse = new()
                                                                      {
                                                                          Id                      = Constant.Id,
                                                                          Name                    = Constant.CompanyName,
                                                                          RegistrationNumber      = Constant.UniqueIdentificationNumber,
                                                                          TaxIdentificationNumber = Constant.UniqueIdentificationNumber,
                                                                          ActivityCode            = Constant.CompanyActivityCode,
                                                                          Address                 = Constant.Address,
                                                                      };
    }
}
