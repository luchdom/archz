using Archz.Products.Api.Application;
using Archz.Products.Api.Infra;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Archz.Products.Api.Application.Queries.GetProducts;

public sealed class GetProductsQueryHandler(AppReadDbContext dbContext)
    : IRequestHandler<GetProductsQuery, IReadOnlyCollection<ProductDto>>
{
    public async Task<IReadOnlyCollection<ProductDto>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken) =>
        await dbContext.Products
            .OrderBy(product => product.Id)
            .Select(product => new ProductDto(
                product.Id,
                product.Name,
                product.Price,
                product.IsActive))
            .ToListAsync(cancellationToken);
}
