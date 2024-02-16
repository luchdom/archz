using Archz.Users.Api.Settings;

namespace Archz.Users.Api;

internal static partial class Startup
{
    public static IServiceCollection AddCustomSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<UsersApiSettings>()
            .Bind(configuration.GetSection(UsersApiSettings.Settings))
            .ValidateDataAnnotations();
        services.AddOptions<JwtTokenSettings>()
           .Bind(configuration.GetSection(JwtTokenSettings.Settings))
           .ValidateDataAnnotations();
        return services;
    }
}
