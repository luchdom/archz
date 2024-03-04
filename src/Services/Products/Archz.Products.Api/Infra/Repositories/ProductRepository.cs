using Archz.Products.Api.Domain.AggregateModels.ProductAggregates;
using Microsoft.EntityFrameworkCore;

namespace Archz.Products.Api.Infra.Repositories;

internal sealed class ProductRepository(AppWriteDbContext context) : IProductRepository
{
    public Task<bool> ExistsWithSameNameAsync(
       string name,
       CancellationToken cancellationToken = default) =>
       context.Products.AnyAsync(f => f.Name.ToLower() == name.ToLower(), cancellationToken);

    public async Task InsertAsync(Product product)
    {
        await context.Products.AddAsync(product);
    }
}
