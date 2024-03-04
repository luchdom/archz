using Archz.Application.Core;
using Archz.Products.Api.Application;
using Archz.Products.Api.Application.Commands.CreateProduct;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Archz.Products.Api.Controllers;


[ApiController]
[Route("/v{version:apiVersion}/[controller]")]
[ApiVersion(ApiVersion)]
public class ProductsController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;
    private const string ApiVersion = "1";

    [HttpPost]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    public async Task<IResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsFailed)
            return ValidationProblem(result);

        return Results.Created(new Uri(Request.GetEncodedUrl() + "/" + result.Value.Id), result.Value);
    }
}
