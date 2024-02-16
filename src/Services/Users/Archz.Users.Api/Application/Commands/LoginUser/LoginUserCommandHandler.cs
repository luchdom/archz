using Archz.Users.Api.Application.Commands.LoginUser;
using Archz.Users.Api.Domain.AggregateModels.UserAggregate;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OrderManager.Api.Application.Services;
using System.Data;

namespace Archz.Users.Api.Application.Commands.RegisterUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginUserCommandResponse>>
    {
        private readonly ILogger<LoginUserCommandHandler> _logger;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public LoginUserCommandHandler(
            ILogger<LoginUserCommandHandler> logger,
            UserManager<User> userManager,
            ITokenService tokenService)
        {
            _logger = logger;
            _userManager = userManager;
            _tokenService = tokenService;
        }
        public async Task<Result<LoginUserCommandResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            return Result.Ok();
         }
    }
}
