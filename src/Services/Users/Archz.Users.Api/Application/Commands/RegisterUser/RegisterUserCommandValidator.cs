using FluentValidation;

namespace Archz.Users.Api.Application.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x=>x.PasswordConfirmation)
            .NotEmpty()
            .Equal(x=>x.Password);
    }

}
