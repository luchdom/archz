using Archz.Products.Api.Settings;

namespace Archz.Products.Api;

internal static partial class Startup
{
    public static IServiceCollection AddCustomSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtTokenSettings>()
           .Bind(configuration.GetSection(JwtTokenSettings.Settings))
           .ValidateDataAnnotations();
        return services;
    }
}
