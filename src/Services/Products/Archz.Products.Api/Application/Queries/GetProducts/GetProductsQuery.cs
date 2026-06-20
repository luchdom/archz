using Archz.Products.Api.Application;
using MediatR;

namespace Archz.Products.Api.Application.Queries.GetProducts;

public sealed record GetProductsQuery : IRequest<IReadOnlyCollection<ProductDto>>;
