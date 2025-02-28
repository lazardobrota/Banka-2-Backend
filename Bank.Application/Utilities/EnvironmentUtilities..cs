using Bank.Application.Extensions;

namespace Bank.Application.Utilities;

public static class EnvironmentUtilities
{
    public static string GetStringVariable(string variableName, string defaultValue = "")
    {
        return Environment.GetEnvironmentVariable(variableName)
                          .OrDefault(defaultValue);
    }

    public static int GetIntVariable(string variableName, int defaultValue = 0)
    {
        return Environment.GetEnvironmentVariable(variableName)
                          .ParseIntOrDefault(defaultValue);
    }
}
