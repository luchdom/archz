
using Archz.Users.Api.Application.Services;
using Archz.Users.Api.Infra.Seed;

namespace Archz.Users.Api;

internal static partial class Startup
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddTransient<ITokenService, TokenService>();
        services.AddScoped<AppDbContextSeed>();
        return services;
    }
}
