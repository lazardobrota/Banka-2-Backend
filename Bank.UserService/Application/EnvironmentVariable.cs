namespace Bank.UserService.Application;

public static class EnvironmentVariable
{
    public const string SecretKey          = "BANK_SECRET_KEY";
    public const string SecretKeyElseValue = "J0jU9gBDuxpAWiHpAUwO4nwqFfvXxTxZvM0F82X1Q9K8sdku/WUuV1ajytrOFXXy";

    public const string ExpirationInMinutes          = "BANK_JWT_TIMER";
    public const string ExpirationInMinutesElseValue = "30";
}
