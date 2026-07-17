using System.Net;
using System.Net.Http.Json;
using Archz.Products.Api.Application;
using Archz.Products.Api.Infra.Models;
using Archz.Products.Tests.Fixtures;

namespace Archz.Products.Tests.Integration;

public sealed class ProductsApiIntegrationTests(ProductsIntegrationFactory factory)
    : IClassFixture<ProductsIntegrationFactory>
{
    [Fact]
    public async Task Create_product_uses_real_repository_and_persists_the_aggregate()
    {
        await factory.ResetAsync();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/v1/products", new
        {
            name = "Standing Desk",
            price = 899.00m
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        Assert.NotNull(product);
        Assert.Equal("Standing Desk", product.Name);
        Assert.Equal(899.00m, product.Price);
        Assert.True(product.IsActive);
        Assert.Equal(1, await factory.CountWrittenProductsAsync());
    }

    [Fact]
    public async Task Get_products_returns_read_models_ordered_by_id()
    {
        await factory.ResetAsync();
        await factory.SeedReadProductsAsync(
            new ProductReadModel(2, "Second", 20m, true),
            new ProductReadModel(1, "First", 10m, true));
        using var client = factory.CreateClient();

        var products = await client.GetFromJsonAsync<IReadOnlyCollection<ProductDto>>("/v1/products");

        Assert.NotNull(products);
        Assert.Collection(
            products,
            first => Assert.Equal("First", first.Name),
            second => Assert.Equal("Second", second.Name));
    }

    [Fact]
    public async Task Get_product_by_id_returns_existing_product()
    {
        await factory.ResetAsync();
        await factory.SeedReadProductsAsync(new ProductReadModel(42, "Monitor", 1500m, true));
        using var client = factory.CreateClient();

        var product = await client.GetFromJsonAsync<ProductDto>("/v1/products/42");

        Assert.NotNull(product);
        Assert.Equal(42, product.Id);
        Assert.Equal("Monitor", product.Name);
    }

    [Fact]
    public async Task Get_product_by_id_returns_not_found_for_missing_product()
    {
        await factory.ResetAsync();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/v1/products/404");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
