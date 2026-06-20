using Archz.Application.Core;
using Archz.Products.Api.Application;
using Archz.Products.Api.Application.Commands.CreateProduct;
using Archz.Products.Api.Application.Queries.GetProductById;
using Archz.Products.Api.Application.Queries.GetProducts;
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

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IResult> GetProducts()
    {
        var products = await _mediator.Send(new GetProductsQuery());
        return Results.Ok(products);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<IResult> GetProductById([FromRoute] int id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));
        return product is null ? Results.NotFound() : Results.Ok(product);
    }

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
