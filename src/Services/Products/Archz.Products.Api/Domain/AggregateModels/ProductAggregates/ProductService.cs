using FluentResults;

namespace Archz.Products.Api.Domain.AggregateModels.ProductAggregates;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<Product>> CreateProductAsync(
       string name,
       decimal price,
       CancellationToken cancellationToken)
    {
        if(await _productRepository.ExistsWithSameNameAsync(name, cancellationToken))
        {
            Result.Fail("There is already a product with the same name");
        }

        return Product.Create(name, price);
    }
 }
