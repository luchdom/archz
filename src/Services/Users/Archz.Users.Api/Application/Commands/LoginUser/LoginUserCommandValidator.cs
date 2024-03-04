using Archz.Users.Api.Application.Commands.LoginUser;
using FluentValidation;

namespace Archz.Users.Api.Application.Commands.RegisterUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
