using FluentResults;
using MediatR;

namespace Archz.Products.Api.Application.Commands.CreateProduct;

public record CreateProductCommand(string Name, decimal Price) : IRequest<Result<ProductDto>>;
