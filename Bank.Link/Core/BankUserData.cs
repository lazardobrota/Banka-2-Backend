using Bank.Application.Responses;

namespace Bank.Link.Core;

public interface IBankUserData
{
    Task<AccountResponse?> GetAccount(string accountNumber);
}

internal interface IBankUserDataLink : IBankUserData
{
    internal BankData BankData { get; }
}

internal class BankUserData(IEnumerable<IBankUserDataLink> userDataLinks) : IBankUserData
{
    private readonly Dictionary<string, IBankUserDataLink> m_DataLinkDictionary = userDataLinks.ToDictionary(link => link.BankData.Code, link => link);

    public async Task<AccountResponse?> GetAccount(string accountNumber)
    {
        var bankCode = accountNumber[..3];

        if (!m_DataLinkDictionary.TryGetValue(bankCode, out var dataLink))
            return null;

        return await dataLink.GetAccount(accountNumber);
    }
}
