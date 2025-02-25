using Bank.Application.Domain;
using Bank.Application.Extensions;
using Bank.Application.Requests;

using FluentValidation;

using ValidatorUtilities = Bank.Application.Utilities.ValidatorUtilities.UserService;

namespace Bank.Application.Validators;

public class EmployeeValidator
{
    public class Create : AbstractValidator<EmployeeCreateRequest>
    {
        private DateOnly m_UINDate;
        private Gender   m_Gender;

        public Create()
        {
            RuleFor(employeeRequest => employeeRequest.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(32)
            .WithMessage("First name must be at most 32 characters long.")
            .Must(ValidatorUtilities.ValidateName)
            .WithMessage("First name is not valid.");

            RuleFor(employeeRequest => employeeRequest.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(32)
            .WithMessage("Last name must be at most 32 characters long.")
            .Must(ValidatorUtilities.ValidateName)
            .WithMessage("Last name is not valid.");

            RuleFor(employeeRequest => employeeRequest.Gender)
            .NotEqual(Gender.Invalid)
            .WithMessage("Gender is required.");

            RuleFor(employeeRequest => employeeRequest.UniqueIdentificationNumber)
            .NotEmpty()
            .WithMessage("Unique identification number is required.")
            .Must(ValidatorUtilities.MatchUniqueIdentificationNumberRule)
            .WithMessage("Unique identification number must be 13 digits.")
            .Must(ValidateUniqueIdentificationNumberDate)
            .WithMessage("Unique identification number does not contain a valid date.")
            .Must(ValidateGender)
            .WithMessage("Unique identification number does not contain a valid gender.")
            .Must(ValidatorUtilities.ValidateControlNumber)
            .WithMessage("Unique identification number does not have a valid control digit.");

            RuleFor(employeeRequest => employeeRequest.DateOfBirth)
            .NotEqual(DateOnly.MinValue)
            .WithMessage("Date of birth is required.")
            .Must(ValidatorUtilities.ValidateDateOfBirth)
            .WithMessage("Date of birth is not valid.")
            .Must(DatesMatch)
            .WithMessage("Date of birth doesn't match date of unique identification number.");

            RuleFor(employeeRequest => employeeRequest.Email)
            .EmailAddress()
            .WithMessage("Email is not a valid email address.");

            RuleFor(employeeRequest => employeeRequest.Username)
            .Must(ValidatorUtilities.ValidateUsername)
            .WithMessage("Invalid username.");

            RuleFor(employeeRequest => employeeRequest.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Must(ValidatorUtilities.ValidatePhoneNumber)
            .WithMessage("Phone number is not valid.")
            .MinimumLength(12)
            .WithMessage("Phone number does not have enough digits.")
            .MaximumLength(13)
            .WithMessage("Phone number has more than 13 digits.");

            RuleFor(employeeRequest => employeeRequest.Address)
            .NotEmpty()
            .WithMessage("Address is required.")
            .Must(ValidatorUtilities.ValidateNameWithNumbers)
            .WithMessage("Address is not valid.");

            RuleFor(employeeRequest => employeeRequest.Role)
            .NotEqual(Role.Invalid)
            .WithMessage("Role is required.");

            RuleFor(employeeRequest => employeeRequest.Department)
            .Must(ValidatorUtilities.ValidateNameWithNumbers)
            .WithMessage("Department name is not valid.");
        }

        private bool ValidateUniqueIdentificationNumberDate(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            var result = value.Substring(0, 7)
                              .TryParseUINToDate(out var date);

            m_UINDate = date;

            return result;
        }

        private bool DatesMatch(DateOnly dateOfBirth)
        {
            return m_UINDate == dateOfBirth;
        }

        private bool ValidateGender(string value)
        {
            return !(value[9] - '0' < 5 ^ m_Gender == 0);
        }
    }

    public class Update : AbstractValidator<EmployeeUpdateRequest>
    {
        public Update()
        {
            RuleFor(employeeRequest => employeeRequest.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(32)
            .WithMessage("First name must be at most 32 characters long.")
            .Must(ValidatorUtilities.ValidateName)
            .WithMessage("First name is not valid.");

            RuleFor(employeeRequest => employeeRequest.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(32)
            .WithMessage("Last name must be at most 32 characters long.")
            .Must(ValidatorUtilities.ValidateName)
            .WithMessage("Last name is not valid.");

            RuleFor(employeeRequest => employeeRequest.Username)
            .Must(ValidatorUtilities.ValidateUsername)
            .WithMessage("Invalid username.");

            RuleFor(employeeRequest => employeeRequest.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Must(ValidatorUtilities.ValidatePhoneNumber)
            .WithMessage("Phone number is not valid.")
            .MinimumLength(12)
            .WithMessage("Phone number does not have enough digits.")
            .MaximumLength(13)
            .WithMessage("Phone number has more than 13 digits.");

            RuleFor(employeeRequest => employeeRequest.Address)
            .NotEmpty()
            .WithMessage("Address is required.")
            .Must(ValidatorUtilities.ValidateNameWithNumbers)
            .WithMessage("Address is not valid.");

            RuleFor(employeeRequest => employeeRequest.Department)
            .Must(ValidatorUtilities.ValidateNameWithNumbers)
            .WithMessage("Department name is not valid.");
        }
    }
}
