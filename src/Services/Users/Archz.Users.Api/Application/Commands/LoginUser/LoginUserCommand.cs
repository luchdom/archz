using FluentResults;
using MediatR;

namespace Archz.Users.Api.Application.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<Result<LoginUserCommandResponse>>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
