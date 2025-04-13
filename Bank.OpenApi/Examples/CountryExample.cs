using Bank.Application.Responses;

namespace Bank.OpenApi.Examples;

public static partial class Example
{
    public class Country
    {
        public static readonly CountryResponse Response = new()
                                                          {
                                                              Id         = Constant.Id,
                                                              Name       = Constant.CountryName,
                                                              Currency   = Currency.SimpleResponse,
                                                              CreatedAt  = Constant.CreatedAt,
                                                              ModifiedAt = Constant.ModifiedAt,
                                                          };

        public static readonly CountrySimpleResponse SimpleResponse = new()
                                                                      {
                                                                          Id         = Constant.Id,
                                                                          Name       = Constant.CountryName,
                                                                          CreatedAt  = Constant.CreatedAt,
                                                                          ModifiedAt = Constant.ModifiedAt,
                                                                      };
    }
}
