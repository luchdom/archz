using Archz.SharedKernel.SeedWork;
using FluentResults;

namespace Archz.Products.Api.Domain.AggregateModels.ProductAggregates;

public sealed class Product : Entity
{
    private Product(string name, decimal price, bool isActive)
    {
        Name = name;
        Price = price;
        IsActive = isActive;
    }

    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public bool IsActive { get; set; }

    public static Result<Product> Create(string name, decimal price)
    {
        //Validate any additional business rules and return with Result.Fail in case of an error
        var product = new Product(name, price, isActive: true);
        product.AddDomainEvent(new ProductCreatedEvent(product));
        return Result.Ok(product);
    }
}
