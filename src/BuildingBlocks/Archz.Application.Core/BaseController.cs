using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Archz.Application.Core;
public class BaseController : ControllerBase
{
    [NonAction]
    public IResult ValidationProblem(ResultBase result)
    {
        var problemDetails = new ProblemDetails()
        {
            Instance = ControllerContext.HttpContext.Request.Path,
            Status = StatusCodes.Status400BadRequest,
            Detail = "Please refer to the errors property for additional details.",
        };
        problemDetails.Extensions["results"] = result.Errors;
        return Results.Problem(
            title: "One or more validation errors occurred.",
            instance: ControllerContext.HttpContext.Request.Path,
            detail: "Please refer to the errors property for additional details.",
            type: "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            statusCode: StatusCodes.Status400BadRequest,
            extensions: new Dictionary<string, object?>
            {
                { "errors", result.Errors }
            });
    }
}
