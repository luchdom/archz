using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Archz.Users.Api.Infra;
using Archz.Users.Api.Settings;

namespace Archz.Users.Api;

internal static partial class Startup
{
    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(ConnectionStringsSettings.Database),
                           sqlServerOptionsAction: sqlOptions =>
                           {
                               sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                               sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                           });
            },
            ServiceLifetime.Scoped);
    }
}
