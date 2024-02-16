
using Archz.Users.Api.Infra;

namespace Archz.Users.Api;

internal static partial class Startup
{
    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var hcBuilder = services.AddHealthChecks();
        hcBuilder.AddDbContextCheck<AppDbContext>();
        return services;
    }
}
