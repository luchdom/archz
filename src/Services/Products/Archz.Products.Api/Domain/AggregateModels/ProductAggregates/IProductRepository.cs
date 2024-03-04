namespace Archz.Products.Api.Domain.AggregateModels.ProductAggregates;

public interface IProductRepository
{
    Task<bool> ExistsWithSameNameAsync(
       string name,
       CancellationToken cancellationToken = default);

    Task InsertAsync(Product product);
}
