using FluentValidation;

namespace Archz.Users.Api.Application.Commands.RegisterUser;

public class LoginUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }

}
