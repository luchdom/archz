using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Archz.Products.Api.Application;
using Archz.Products.Tests.Fixtures;

namespace Archz.Products.Tests.Behavioral;

public sealed class CreateProductBehavioralTests
{
    [Fact]
    public async Task Valid_product_request_creates_product_through_real_application_flow()
    {
        using var factory = new ProductsBehavioralFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/v1/products", new
        {
            name = "Mechanical Keyboard",
            price = 249.90m
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.EndsWith("/v1/products/1", response.Headers.Location?.ToString());

        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        Assert.NotNull(product);
        Assert.Equal(1, product.Id);
        Assert.Equal("Mechanical Keyboard", product.Name);
        Assert.Equal(249.90m, product.Price);
        Assert.True(product.IsActive);

        Assert.Equal(1, factory.Boundary.ExistsCalls);
        Assert.Equal(1, factory.Boundary.InsertCalls);
        Assert.Equal(1, factory.Boundary.SaveChangesCalls);
        Assert.Single(factory.Boundary.InsertedProducts);
    }

    [Fact]
    public async Task Duplicate_product_name_returns_business_failure_without_persisting()
    {
        using var factory = new ProductsBehavioralFactory();
        factory.Boundary.ProductWithSameNameExists = true;
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/v1/products", new
        {
            name = "Mechanical Keyboard",
            price = 249.90m
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("There is already a product with the same name", body);
        Assert.Equal(1, factory.Boundary.ExistsCalls);
        Assert.Equal(0, factory.Boundary.InsertCalls);
        Assert.Equal(0, factory.Boundary.SaveChangesCalls);
    }

    [Fact]
    public async Task Invalid_request_shape_returns_validation_failure_before_boundaries_are_called()
    {
        using var factory = new ProductsBehavioralFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/v1/products", new
        {
            name = "",
            price = 0m
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("'Name' must not be empty.", body);
        Assert.Contains("'Price' must be greater than '0'.", body);
        Assert.Equal(0, factory.Boundary.ExistsCalls);
        Assert.Equal(0, factory.Boundary.InsertCalls);
        Assert.Equal(0, factory.Boundary.SaveChangesCalls);
    }

    [Fact]
    public async Task Null_request_body_uses_the_existing_model_binding_error_keys()
    {
        using var factory = new ProductsBehavioralFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync<object?>("/v1/products", null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var problem = await response.Content.ReadFromJsonAsync<JsonDocument>();
        Assert.NotNull(problem);
        var errors = problem.RootElement.GetProperty("errors");
        Assert.Equal("A non-empty request body is required.", errors.GetProperty("")[0].GetString());
        Assert.Equal("The command field is required.", errors.GetProperty("command")[0].GetString());
        Assert.Equal(0, factory.Boundary.InsertCalls);
    }
}
