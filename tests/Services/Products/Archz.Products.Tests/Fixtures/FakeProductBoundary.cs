using Archz.Products.Api.Domain.AggregateModels.ProductAggregates;
using Archz.SharedKernel.SeedWork;

namespace Archz.Products.Tests.Fixtures;

internal sealed class FakeProductBoundary : IProductRepository, IUnitOfWork
{
    private int _nextId = 1;

    public bool ProductWithSameNameExists { get; set; }

    public List<Product> InsertedProducts { get; } = [];

    public int ExistsCalls { get; private set; }

    public int InsertCalls { get; private set; }

    public int SaveChangesCalls { get; private set; }

    public Task<bool> ExistsWithSameNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ExistsCalls++;
        return Task.FromResult(ProductWithSameNameExists);
    }

    public Task InsertAsync(Product product)
    {
        InsertCalls++;
        SetId(product, _nextId++);
        InsertedProducts.Add(product);
        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SaveChangesCalls++;
        return Task.FromResult(1);
    }

    private static void SetId(Product product, int id)
    {
        typeof(Entity)
            .GetProperty(nameof(Entity.Id))!
            .SetValue(product, id);
    }
}
