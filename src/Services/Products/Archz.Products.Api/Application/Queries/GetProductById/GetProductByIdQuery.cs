using Archz.Products.Api.Application;
using MediatR;

namespace Archz.Products.Api.Application.Queries.GetProductById;

public sealed record GetProductByIdQuery(int Id) : IRequest<ProductDto?>;
