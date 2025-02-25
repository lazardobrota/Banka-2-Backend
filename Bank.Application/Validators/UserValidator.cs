using Bank.Application.Requests;

using FluentValidation;

namespace Bank.Application.Validators;

public static class UserValidator
{
    public class Login : AbstractValidator<UserLoginRequest>
    {
        public Login()
        {
            RuleFor(userRequest => userRequest.Email)
            .NotEmpty()
            .WithMessage(ValidationErrorMessage.Global.EmailRequired)
            .EmailAddress()
            .WithMessage(ValidationErrorMessage.Global.EmailInvalid);
        }
    }
}
