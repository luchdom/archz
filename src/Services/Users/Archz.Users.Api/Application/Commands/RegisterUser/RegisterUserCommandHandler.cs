using Archz.Users.Api.Application.Services;
using Archz.Users.Api.Domain.AggregateModels.UserAggregate;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Archz.Users.Api.Application.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<int>>
    {
        private readonly ILogger<LoginUserCommandHandler> _logger;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public RegisterUserCommandHandler(
            ILogger<LoginUserCommandHandler> logger,
            UserManager<User> userManager,
            ITokenService tokenService)
        {
            _logger = logger;
            _userManager = userManager;
            _tokenService = tokenService;
        }
        public async Task<Result<int>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User { UserName = request.Email, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
                return Result.Ok(user.Id);

            var errors = result.Errors
               .Select(errors => new Error(errors.Description))
               .ToList();

            _logger.LogDebug("User with email {Email} could not be registered due to {@Errors}", 
                request.Email, errors);

            return Result.Fail(errors);
         }
    }
}
