using Archz.Products.Api;
using Archz.Products.Api.Infra;
using Archz.Products.Api.Infra.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Archz.Products.Tests.Fixtures;

public sealed class ProductsIntegrationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"products-tests-{Guid.NewGuid():N}";
    private readonly InMemoryDatabaseRoot _databaseRoot = new();

    public ProductsIntegrationFactory()
    {
        ProductTestSettings.ApplyToEnvironment();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(ProductTestSettings.Configuration);
        });
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<AppWriteDbContext>();
            services.RemoveAll<AppReadDbContext>();
            services.RemoveAll<DbContextOptions<AppWriteDbContext>>();
            services.RemoveAll<DbContextOptions<AppReadDbContext>>();

            services.AddDbContext<AppWriteDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName, _databaseRoot));
            services.AddDbContext<AppReadDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName, _databaseRoot)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        });
    }

    public async Task ResetAsync()
    {
        using var scope = Services.CreateScope();
        var writeDbContext = scope.ServiceProvider.GetRequiredService<AppWriteDbContext>();
        var readDbContext = scope.ServiceProvider.GetRequiredService<AppReadDbContext>();

        await writeDbContext.Database.EnsureDeletedAsync();
        await writeDbContext.Database.EnsureCreatedAsync();
        await readDbContext.Database.EnsureCreatedAsync();
    }

    public async Task SeedReadProductsAsync(params ProductReadModel[] products)
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppReadDbContext>();

        dbContext.Products.AddRange(products);
        await dbContext.SaveChangesAsync();
    }

    public async Task<int> CountWrittenProductsAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppWriteDbContext>();

        return await dbContext.Products.CountAsync();
    }
}
