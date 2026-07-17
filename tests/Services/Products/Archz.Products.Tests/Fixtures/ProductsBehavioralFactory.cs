using Archz.Products.Api;
using Archz.Products.Api.Domain.AggregateModels.ProductAggregates;
using Archz.SharedKernel.SeedWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Archz.Products.Tests.Fixtures;

internal sealed class ProductsBehavioralFactory : WebApplicationFactory<Program>
{
    private readonly FakeProductBoundary _boundary = new();

    public ProductsBehavioralFactory()
    {
        ProductTestSettings.ApplyToEnvironment();
    }

    public FakeProductBoundary Boundary => _boundary;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(ProductTestSettings.Configuration);
        });
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IProductRepository>();
            services.RemoveAll<IUnitOfWork>();
            services.AddSingleton<IProductRepository>(_boundary);
            services.AddSingleton<IUnitOfWork>(_boundary);
        });
    }
}
