using Archz.Application.Core;
using Archz.Users.Api.Application.Commands.RegisterUser;
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
    public async Task<IResult> SignUp(RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsFailed)
            return ValidationProblem(result);

        return Results.Created($"{Request.Scheme}://{Request.Host}/v{ApiVersion}/users/{1}", result);
    }

    //[HttpPost]
    //[Route("signin")]
    //public async Task<IResult> SignIn(LoginCommand command)
    //{
    //    var result = await _mediator.Send(command);
    //    if (result.IsFailed)
    //        return ValidationProblem(result);

    //    return Created($"{Request.Scheme}://{Request.Host}/v{ApiVersion}/users/{result.Value}", result);
    //}
}
