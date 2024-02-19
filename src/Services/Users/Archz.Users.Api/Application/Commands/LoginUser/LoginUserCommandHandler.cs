using Archz.Application.Core.Extensions;
using Archz.Users.Api.Application.Commands.LoginUser;
using Archz.Users.Api.Application.Services;
using Archz.Users.Api.Domain.AggregateModels.UserAggregate;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return Result.Fail("Invalid credentials");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password!);
            if (!isPasswordValid)
            {
                return Result.Fail("Invalid credentials");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var createdToken = _tokenService.CreateToken(user, userRoles);

            return Result.Ok(new LoginUserCommandResponse
            {
                AccessToken = createdToken.Token,
                ExpiresIn = createdToken.ValidTo.ToTimestamp()
            });
         }
    }
}
