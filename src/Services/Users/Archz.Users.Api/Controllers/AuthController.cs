using Archz.Application.Core;
using Archz.Users.Api.Application.Commands.LoginUser;
using Archz.Users.Api.Application.Commands.RegisterUser;
using Archz.Users.Api.Controllers.Requests;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Archz.Users.Api.Controllers;

[ApiController]
[Route("/v{version:apiVersion}/[controller]")]
[ApiVersion(ApiVersion)]
public class AuthController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;
    private const string ApiVersion = "1";

    [HttpPost]
    [Route("signup")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(LoginUserCommandResponse), StatusCodes.Status200OK)]
    public async Task<IResult> SignUp([FromBody] RegisterUserRequest command)
    {
        var result = await _mediator.Send(new RegisterUserCommand
        {
            Email = command.Email,
            Password = command.Password,
            PasswordConfirmation = command.PasswordConfirmation
        });
        if (result.IsFailed)
            return ValidationProblem(result);

        //You can ask for EmailConfirmation and only return Created (201) here

        return await Login(new LoginUserCommand { Email = command.Email, Password = command.Password });
    }

    [HttpPost]
    [Route("signin")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(LoginUserCommandResponse), StatusCodes.Status200OK)]
    public async Task<IResult> SignIn([FromBody] LoginUserRequest command)
        => await Login(new LoginUserCommand { Email = command.Email, Password = command.Password });

    [NonAction]
    private async Task<IResult> Login(LoginUserCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsFailed)
            return ValidationProblem(result);
        return Results.Ok(result.Value);
    }
}
