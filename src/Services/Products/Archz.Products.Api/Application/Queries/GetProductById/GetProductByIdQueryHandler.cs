using Archz.Products.Api.Application;
using Archz.Products.Api.Infra;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Archz.Products.Api.Application.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler(AppReadDbContext dbContext)
    : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken) =>
        await dbContext.Products
            .Where(product => product.Id == request.Id)
            .Select(product => new ProductDto(
                product.Id,
                product.Name,
                product.Price,
                product.IsActive))
            .FirstOrDefaultAsync(cancellationToken);
}
