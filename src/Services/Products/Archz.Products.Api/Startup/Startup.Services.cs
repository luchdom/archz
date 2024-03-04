using Archz.Products.Api.Domain.AggregateModels.ProductAggregates;
using Archz.Products.Api.Infra;
using Archz.Products.Api.Infra.Repositories;
using Archz.SharedKernel.SeedWork;

namespace Archz.Products.Api;

internal static partial class Startup
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppWriteDbContext>());

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
