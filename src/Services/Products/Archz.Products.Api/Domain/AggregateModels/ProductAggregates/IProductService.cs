using FluentResults;

namespace Archz.Products.Api.Domain.AggregateModels.ProductAggregates;

public interface IProductService
{
    Task<Result<Product>> CreateProductAsync(
       string name,
       decimal price,
       CancellationToken cancellationToken);
}