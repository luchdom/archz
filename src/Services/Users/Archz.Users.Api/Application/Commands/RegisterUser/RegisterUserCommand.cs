using FluentResults;
using MediatR;

namespace Archz.Users.Api.Application.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<Result>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordConfirmation { get; set; }
    }
}
