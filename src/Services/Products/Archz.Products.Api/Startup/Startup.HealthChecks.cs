
using Archz.Products.Api.Infra;

namespace Archz.Products.Api;

internal static partial class Startup
{
    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var hcBuilder = services.AddHealthChecks();
        hcBuilder.AddDbContextCheck<AppWriteDbContext>();
        return services;
    }
}
