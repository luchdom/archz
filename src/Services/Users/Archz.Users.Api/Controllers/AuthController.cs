using Archz.Application.Core;
using Archz.Users.Api.Application.Commands.LoginUser;
using Archz.Users.Api.Application.Commands.RegisterUser;
using Archz.Users.Api.Settings;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Archz.Users.Api.Controllers;

[ApiController]
[Route("/v{version:apiVersion}/[controller]")]
[ApiVersion(ApiVersion)]
public class AuthController(IMediator mediator, IOptions<UsersApiSettings> settings) : BaseController
{
    private readonly IMediator _mediator = mediator;
    private readonly IOptions<UsersApiSettings> settings = settings;
    private const string ApiVersion = "1";

    [HttpPost]
    [Route("signup")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(LoginUserCommandResponse), StatusCodes.Status200OK)]
    public async Task<IResult> SignUp([FromBody] RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsFailed)
            return ValidationProblem(result);

        return await Login(new LoginUserCommand() { Email = command.Email, Password = command.Password });
    }

    [HttpPost]
    [Route("signin")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(LoginUserCommandResponse), StatusCodes.Status200OK)]
    public async Task<IResult> SignIn([FromBody] LoginUserCommand command) 
        => await Login(command);

    [NonAction]
    private async Task<IResult> Login(LoginUserCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsFailed)
            return ValidationProblem(result);
        return Results.Ok(result.Value);
    }
}
