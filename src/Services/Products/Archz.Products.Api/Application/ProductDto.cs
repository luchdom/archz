using Archz.Products.Api.Domain.AggregateModels.ProductAggregates;
using System.Numerics;

namespace Archz.Products.Api.Application;

public sealed record ProductDto(int Id, string Name, decimal Price, bool IsActive)
{
    public static implicit operator ProductDto(Product product) => 
        new ProductDto(product.Id, product.Name, product.Price, product.IsActive);

}
