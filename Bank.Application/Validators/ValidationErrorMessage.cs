namespace Bank.Application.Validators;

public static partial class ValidationErrorMessage
{
    public static class User
    {
        public static class Login { }

        public static class PasswordReset { }

        public static class ActivateAccount { }
    }
}

public static partial class ValidationErrorMessage
{
    public static class Employee { }
}

public static partial class ValidationErrorMessage
{
    public static class Client { }
}

public static partial class ValidationErrorMessage
{
    public static class Global
    {
        public const string FirstNameRequired = "First name is required.";
        public const string FirstNameLength   = "First name must be at most 32 characters long.";
        public const string FirstNameInvalid  = "First name is not valid.";

        public const string LastNameRequired = "Last name is required.";
        public const string LastNameLength   = "Last name must be at most 32 characters long.";
        public const string LastNameInvalid  = "Last name is not valid.";

        public const string GenderRequired = "Gender is required.";

        public const string UINRequired            = "Unique identification number is required.";
        public const string UINLength              = "Unique identification number must be 13 digits.";
        public const string UINInvalidDate         = "Unique identification number does not contain a valid date.";
        public const string UINInvalidGender       = "Unique identification number does not contain a valid gender.";
        public const string UINInvalidControlDigit = "Unique identification number does not have a valid control digit.";

        public const string PasswordRequired                   = "Password is required.";
        public const string PasswordTooShort                   = "Password must be at least 8 charachers long.";
        public const string PasswordTooLong                    = "Password must be at most 32 characters long.";
        public const string PasswordTwoDigitsRequired          = "Password must contain at least two digits.";
        public const string PasswordLowercaseCharacterRequired = "Password must contain at least one lowercase character.";
        public const string PasswordUppercaseCharacterRequired = "Password must contain at least one uppercase character.";


        public const string DOBRequired = "Date of birth is required.";
        public const string DOBInvalid  = "Date of birth is not valid.";
        public const string DOBMismatch = "Date of birth doesn't match date of unique identification number.";

        public const string EmailRequired = "Email is required.";
        public const string EmailInvalid  = "Email is not a valid email address.";

        public const string UsernameInvalid = "Invalid username.";

        public const string PhoneNumberRequired  = "Phone number is required.";
        public const string PhoneNumberInvalid   = "Phone number is not valid.";
        public const string PhoneNumberMinLength = "Phone number does not have enough digits.";
        public const string PhoneNumberMaxLength = "Phone number has more than 13 digits.";

        public const string AddressRequired = "Address is required.";
        public const string AddressInvalid  = "Address is not valid.";

        public const string RoleRequired = "Role is required.";

        public const string DepartmentInvalid = "Department name is not valid.";
    }
}
